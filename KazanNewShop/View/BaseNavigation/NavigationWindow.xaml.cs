using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.Services;
using KazanNewShop.View.Pages.LoadedPage;
using KazanNewShop.View.Pages.MainPages;
using KazanNewShop.View.Pages.UserCreation;
using KazanNewShop.ViewModel;
using KazanNewShop.ViewModel.PageVM;
using KazanNewShop.ViewModel.WindowsVM;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace KazanNewShop.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class NavigationWindow : UiWindow
    {
        public static NavigationWindow Instance { get; private set; } = null!;

        // Страница на которой ты находишься
        public static Type CurrentVM { get; private set; } = null!;

        /// <summary>
        /// Словарь для хранения VM и его View
        /// Dictionary<Type VM, Type Page>
        /// </summary>
        public static readonly IDictionary<Type, Type> ViewModelPageTypes =
            new Dictionary<Type, Type>()
            {
                {typeof(FillingClientDataVM) , typeof(FillingClientData)},
                {typeof(FillingSalesmanDataVM) , typeof(FillingSalesmanData)},
                {typeof(LoadingScreenVM) , typeof(LoadingScreenPage)},
                {typeof(NavigationPageMarketplaceVM) , typeof(NavigationPageMarketplace)},
                {typeof(NavigationSelecmanPageMarketplaceVM) , typeof(NavigationSelecmanPageMarketplace)},
                {typeof(BasketPageVM) , typeof(BasketPage)},
                {typeof(OrderPageVM) , typeof(OrderPage)},
                {typeof(ListOrderPageVM) , typeof(ListOrderPage)},
                {typeof(ListOrderPageForClientVM) , typeof(ListOrderPageForClient)},
            };

        public NavigationWindow()
        {
            InitializeComponent();

            Instance = this;
        }

        /// <summary>
        /// Создание экрана загрузки при переходе на страницу со всеми товарами
        /// </summary>
        public static void TransitionProductList(Type VMToNavigate)
        {
            Navigate(typeof(LoadingScreenVM));

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);

                DatabaseContext.Entities.SaveChanges();

                if (DatabaseContext.LoadingFlag == false)
                {
                    // загрузка таблиц
                    DatabaseContext.LoadEntitesForMarketplace();

                    // Добавление в категории "Все" 
                    if (DatabaseContext.Entities.Categories.Local.ToObservableCollection().Any(c => c.Name == "Все") == false)
                        DatabaseContext.Entities.Categories.Local.ToObservableCollection().Insert(0, new Category() { Name = "Все" });

                    // Создание дефолтной картинкой для товара
                    ConvernMainPhoto();

                    // Создание дефолтной картинки для профиля
                    ConvernProfilePhoto();

                    // Выдача первой картинки продукту для отображения картинки в списке продуктов
                    IssuingImage();
                }

            }).ContinueWith(task => { Navigate(VMToNavigate); },
                                      TaskScheduler.FromCurrentSynchronizationContext());

            CurrentVM = VMToNavigate;
        }

        // метод выдачи первой картинки товару
        public static void IssuingImage()
        {
            foreach (Product item in DatabaseContext.Entities.Products.Local)
                item.MainPhoto = DatabaseContext.Entities.PhotoProducts.Local.FirstOrDefault(p => p.Product == item)?.Photo;
        }

        /// <summary>
        /// Создание в байтах картинки для продуктов без картинок
        /// </summary>
        private static void ConvernMainPhoto() =>
           CommonMethods.MainForProductNullPhoto =
               CommonMethods.ConvertImage(".\\Resources\\Images\\iconforNullValueProduct.png")!;

        /// <summary>
        /// Создание в байтах картинки для клиента без картиноки
        /// </summary>
        private static void ConvernProfilePhoto() =>
           CommonMethods.MainForProfileClientNullPhoto =
               CommonMethods.ConvertImage(".\\Resources\\Images\\ProfilePhotoNull.png")!;

        /// <summary>
        /// Переход на нужный Page
        /// </summary>
        /// <param name="viewModelType">*Page*VM</param>
        public static void Navigate(Type viewModelType, string? title = null)
        {
            var viewModelInstance = Activator.CreateInstance(viewModelType);

            var pageType = ViewModelPageTypes[viewModelType];
            var pageInstance = (Page)Activator.CreateInstance(pageType)!;
            pageInstance.DataContext = viewModelInstance;

            Instance.NavigationFrame.Navigate(pageInstance);

            if (title != null)
                Instance.Title = title;
            else
                Instance.Title = "Marketplace";

            CurrentVM = viewModelType;
        }

        /// <summary>
        /// Переход на предыдущую страницу
        /// </summary>
        public static void GoBack()
        {
            if (Instance.NavigationFrame.CanGoBack)
                Instance.NavigationFrame.GoBack();
        }

        /// <summary>
        /// При закрытии окна
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (DatabaseContext.Entities.ChangeTracker.HasChanges() == false ||
                CurrentVM == typeof(FillingSalesmanDataVM) || CurrentVM == typeof(FillingClientDataVM))
            {
                base.OnClosing(e);
                return;
            }

            Wpf.Ui.Controls.MessageBox dialog = CreatingDialogMessageBox();

            var dialogResult = false;

            dialog.ButtonLeftClick += (_, _) =>
            {
                dialogResult = true;
                dialog.Close();
            };

            dialog.ButtonRightClick += (_, _) => { dialog.Close(); };

            dialog.ShowDialog();

            if (dialogResult)
                DatabaseContext.Entities.SaveChanges();

            base.OnClosing(e);
        }

        /// <summary>
        /// Создание окна сообщения
        /// </summary>
        public static void CreatingDialogMessageBoxWithOk(string message, string title)
        {
            var messageBox = new Wpf.Ui.Controls.MessageBox
            {
                Content = message,
                Title = title,
                FontSize = 16,
                FontWeight = FontWeights.Medium
            };
            var okButton = new System.Windows.Controls.Button
            {
                Content = "Ок",
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 200
            };
            messageBox.Footer = okButton;
            okButton.Click += (_, _) => messageBox.Close();
            messageBox.ShowDialog();
        }

        /// <summary>
        /// Создание окна сообщения
        /// </summary>
        /// <returns>Wpf.Ui.Controls.MessageBox</returns>
        public static Wpf.Ui.Controls.MessageBox CreatingDialogMessageBox() =>
            new Wpf.Ui.Controls.MessageBox
            {
                Content = "Хотите ли вы сохранить изменения",
                Title = "Уведомление",
                ButtonLeftName = "Да",
                ButtonRightName = "Нет",
            };
    }
}

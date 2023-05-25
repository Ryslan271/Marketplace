using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.Services;
using KazanNewShop.View.Pages.LoadedPage;
using KazanNewShop.View.Pages.MainPages;
using KazanNewShop.View.Pages.UserCreation;
using KazanNewShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        /// <summary>
        /// Словарь для хранения VM и его View
        /// Dictionary<Type VM, Type Page>
        /// </summary>
        public static readonly IDictionary<Type, Type> ViewModelPageTypes =
            new Dictionary<Type, Type>()
            {
                {typeof(FillingClientDataVM) , typeof(FillingClientData)},
                {typeof(FillingSalesmanDataVM) , typeof(FillingSalesmanData)},
                {typeof(NavigationPageMarketplaceVM) , typeof(NavigationPageMarketplace)},
                {typeof(LoadingScreenVM) , typeof(LoadingScreenPage)},
                {typeof(BasketPageVM) , typeof(BasketPage)},
            };

        public NavigationWindow()
        {
            InitializeComponent();

            Instance = this;

            // Загрузка основных страниц во втором потоке
            if (DatabaseContext.LodingFlag == false)
                TransitionProductList();
        }

        /// <summary>
        /// Создание экрана загрузки при переходе на страницу со всеми товарами
        /// </summary>
        public static void TransitionProductList()
        {
            Navigate(typeof(LoadingScreenVM));

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);

                if (DatabaseContext.LodingFlag == false)
                {
                    // загрузка таблиц
                    DatabaseContext.LoadEntitesForMarketplace();

                    // Добавление в категории "Все" 
                    DatabaseContext.Entities.Categories.Local.ToObservableCollection().Insert(0, new Category() { Name = "Все" });

                    // Создание дефолтной картинкой
                    ConvernMainPhoto();

                    // Выдача первой картинки продукту для отображения картинки в списке продуктов
                    foreach (Product item in DatabaseContext.Entities.Products.Local)
                        item.MainPhoto = DatabaseContext.Entities.PhotoProducts.Local.FirstOrDefault(p => p.IdProductNavigation == item)?.Photo;
                }

            }).ContinueWith(task => { Navigate(typeof(NavigationPageMarketplaceVM)); },
                                                  TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Создание в байтах картинки для продуктов без картинок
        /// </summary>
        private static void ConvernMainPhoto() =>
           CommonMethods.MainForProductNullPhoto =
            CommonMethods.ConvertImage(".\\Resources\\Images\\iconforNullValueProduct.png")!;

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
        }

        /// <summary>
        /// Переход на предыдущую страницу
        /// </summary>
        public static void GoBack()
        {
            if (Instance.NavigationFrame.CanGoBack)
                Instance.NavigationFrame.GoBack();
        }
    }
}

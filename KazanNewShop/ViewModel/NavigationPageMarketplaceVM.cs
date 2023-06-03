using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.Services;
using KazanNewShop.View.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace KazanNewShop.ViewModel
{
    public partial class NavigationPageMarketplaceVM : ObservableValidator
    {
        /// <summary>
        /// Словарь сортировки
        /// </summary>
        public static Dictionary<string, SortDescription> SortContentTag { get; } = new()
        {
            { "От А до Я", new SortDescription { PropertyName = nameof(Product.Name), Direction = ListSortDirection.Ascending } },
            { "От Я до А", new SortDescription { PropertyName = nameof(Product.Name), Direction = ListSortDirection.Descending } },
            { "Цена по возрастанию", new SortDescription { PropertyName = nameof(Product.Cost), Direction = ListSortDirection.Ascending } },
            { "Цена по убыванию", new SortDescription { PropertyName = nameof(Product.Cost), Direction = ListSortDirection.Descending } }
        };

        public static NavigationPageMarketplaceVM Instance { get; private set; } = null!;

        // Иконка клиента
        [ObservableProperty]
        private byte[] _clientPhoto = App.CurrentUser!.Client!.ProfilePhoto! == null ? CommonMethods.MainForProfileClientNullPhoto : App.CurrentUser.Client!.ProfilePhoto!;

        // Список категорий
        public ObservableCollection<Category> Category { get; } = DatabaseContext.Entities.Categories.Local.ToObservableCollection();

        // Список всех продуктов
        public ICollectionView ViewProducts { get; } 
            = CollectionViewSource.GetDefaultView
                (
                    DatabaseContext.Entities.Products.Local.ToObservableCollection().Where(p => p.IdStatus == 1 && p.Removed == false)
                );

        // Количество товара в корзине
        [ObservableProperty]
        private int _countProdutsInBasket = 0;

        // Строка поиска
        [ObservableProperty]
        private string? _search;

        /// <summary>
        /// Свойство сортировки
        /// </summary>
        private KeyValuePair<string, SortDescription>? _sortSelectedItem = SortContentTag.First();
        public KeyValuePair<string, SortDescription>? SortSelectedItem
        {
            get => _sortSelectedItem;
            set
            {
                _sortSelectedItem = value;

                ProductSorted();

                ViewProducts.Refresh();
            }
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public NavigationPageMarketplaceVM() =>
            Instance = this;

        /// <summary>
        /// Сортировка товаров
        /// </summary>
        public void ProductSorted()
        {
            ViewProducts.SortDescriptions.Clear();

            ViewProducts.SortDescriptions.Add
                (
                    SortSelectedItem!.Value.Value
                );
        }

        /// <summary>
        /// Свйоства категорий
        /// </summary>
        private Category? _cetegorySelectedItem = DatabaseContext.Entities.Categories.Local.ToObservableCollection().First();
        public Category? CetegorySelectedItem
        {
            get => _cetegorySelectedItem;
            set
            {
                _cetegorySelectedItem = value;

                ProductFiltration();
            }
        }

        /// <summary>
        /// Фильтрация товаров 
        /// </summary>
        public void ProductFiltration()
        {
            ViewProducts.Filter = (item) =>
            {
                Product product = (item as Product)!;

                return _cetegorySelectedItem == DatabaseContext.Entities.Categories.Local.ToObservableCollection().First()
                       || product!.Category == _cetegorySelectedItem;
            };
        }


        /// <summary>
        /// Команда поиска по товарам
        /// </summary>
        [RelayCommand]
        public void ProductSearch()
        {
            if (Search == null)
            {
                ViewProducts.Filter = (obj) => true;
                return;
            }

            ViewProducts.Filter = (obj) =>
            {
                string? search = Search?.ToLower().Trim();

                var product = obj as Product;

                if (product?.Name.ToLower().Contains(search!) == false)
                    return false;

                return true;
            };
        }

        /// <summary>
        /// Открытие корзины
        /// </summary>
        [RelayCommand]
        public void OpenBasket()
        {
            if (DatabaseContext.Entities.Baskets.Local.Any(b => b.Client == App.CurrentUser.Client) != true)
                CreateBasket();

            NavigationWindow.Navigate(typeof(BasketPageVM));
        }

        /// <summary>
        /// Команда добавление товара в корзину
        /// </summary>
        [RelayCommand]
        public void AddProductInBasket(Product SelectedItem)
        {
            ValidateExistenceBasket();

                Basket basket = DatabaseContext.Entities.Baskets.Local.First(b => b.Client == App.CurrentUser!.Client);

            if (basket.ProductLists.Any(p => p.Product == SelectedItem!) == true)
            {
                if (SelectedItem.Count - 1 >= basket.ProductLists.First(p => p.Product == SelectedItem!).Count)
                    basket.ProductLists.First(p => p.Product == SelectedItem!).Count += 1;
                else
                    SelectedItem.IsEnableButtomPlus = true;

                SelectedItem.CountInBasket = basket.ProductLists.First(p => p.Product == SelectedItem!).Count;
            }
            else
            {
                DatabaseContext.Entities.ProductLists.Local.Add(new ProductList()
                {
                    Basket = basket,
                    Product = SelectedItem!,
                    Count = 1
                });

                SelectedItem.CountInBasket = basket.ProductLists.First(p => p.Product == SelectedItem!).Count;
            }

            ValidateCountInProductBasket(SelectedItem);

            ViewProducts.Refresh();
        }

        /// <summary>
        /// Команда удаление товара в корзину
        /// </summary>
        [RelayCommand]
        public void DeleteProductInBasket(Product SelectedItem)
        {
            ValidateExistenceBasket();

            Basket basket = DatabaseContext.Entities.Baskets.Local.First(b => b.Client == App.CurrentUser!.Client);

            if (basket.ProductLists.First(p => p.Product == SelectedItem!).Count - 1 > 0)
            {
                basket.ProductLists.First(p => p.Product == SelectedItem!).Count -= 1;

                if (SelectedItem.Count <= basket.ProductLists.First(p => p.Product == SelectedItem!).Count)
                    SelectedItem.IsEnableButtomPlus = false;

                SelectedItem.CountInBasket = basket.ProductLists.First(p => p.Product == SelectedItem!).Count;
            }
            else
            {
                DatabaseContext.Entities.ProductLists.Local.Remove(basket.ProductLists.First(p => p.Product == SelectedItem!));

                SelectedItem.CountInBasket = 0;
            }

            ValidateCountInProductBasket(SelectedItem);

            ViewProducts.Refresh();
        }

        /// <summary>
        /// Открытие окна подробнее о товаре 
        /// </summary>
        [RelayCommand]
        public void ProductDetails(Product SelectedItem)
        {
            DatabaseContext.Entities.SaveChanges();
            new ProductDetails(SelectedItem).ShowDialog();
        }

        /// <summary>
        /// Проверка количество, для показать или скрить элементы
        /// </summary>
        /// <param name="SelectedItem">Выбранный элемент</param>
        private static void ValidateCountInProductBasket(Product SelectedItem)
        {
            if (SelectedItem.CountInBasket <= 0)
            {
                SelectedItem.VisibilyButtonProductNotInCart = Visibility.Visible;
                SelectedItem.VisibilyButtonProductInCart = Visibility.Collapsed;
            }
            else
            {
                SelectedItem.VisibilyButtonProductNotInCart = Visibility.Collapsed;
                SelectedItem.VisibilyButtonProductInCart = Visibility.Visible;
            }
        }

        /// <summary>
        /// Проверка на существование корзины
        /// </summary>
        private static void ValidateExistenceBasket()
        {
            if (DatabaseContext.Entities.Baskets.Local.Any(b => b.Client == App.CurrentUser!.Client) != true)
                CreateBasket();
        }

        /// <summary>
        /// Создание новой корзины
        /// </summary>
        private static void CreateBasket() =>
            DatabaseContext.Entities.Baskets.Local.Add(new Basket()
            {
                Client = App.CurrentUser!.Client!
            });

        /// <summary>
        /// Открытие окна пользователя 
        /// </summary>
        [RelayCommand]
        private static void OpenPersonalPage() =>
            new PersonalPageWindow().ShowDialog();
    }
}

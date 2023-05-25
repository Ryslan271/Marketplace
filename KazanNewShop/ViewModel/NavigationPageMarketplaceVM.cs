using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Data;

namespace KazanNewShop.ViewModel
{
    public partial class NavigationPageMarketplaceVM : ObservableValidator
    {
        public static Dictionary<string, SortDescription> SortContentTag { get; } = new()
        {
            { "От А до Я", new SortDescription { PropertyName = nameof(Product.Name), Direction = ListSortDirection.Ascending } },
            { "От Я до А", new SortDescription { PropertyName = nameof(Product.Name), Direction = ListSortDirection.Descending } },
            { "Цена по возрастанию", new SortDescription { PropertyName = nameof(Product.Cost), Direction = ListSortDirection.Ascending } },
            { "Цена по убыванию", new SortDescription { PropertyName = nameof(Product.Cost), Direction = ListSortDirection.Descending } }
        };

        public ObservableCollection<Category> Category { get; } = DatabaseContext.Entities.Categories.Local.ToObservableCollection();

        public ICollectionView ViewProducts { get; } = CollectionViewSource.GetDefaultView(DatabaseContext.Entities.Products.Local.ToObservableCollection());

        [ObservableProperty]
        private int _countProdutsInBasket = 0;

        [ObservableProperty]
        private Product? _selectedItem;

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
                       || product!.IdCategoryNavigation == _cetegorySelectedItem;
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

        }

        /// <summary>
        /// Команда добавление товара в корзину
        /// </summary>
        [RelayCommand]
        public void AddProductInBasket()
        {
            if (SelectedItem == null)
                return;

            if (DatabaseContext.Entities.Baskets.Local.Any(b => b.IdClientNavigation == App.CarrentUser.Client) != true)
                CreateBasket();

            Basket basket = DatabaseContext.Entities.Baskets.Local.First(b => b.IdClientNavigation == App.CarrentUser.Client);

            if (basket.ProductLists.Any(p => p.IdProductNavigation == SelectedItem!))
            {
                basket.ProductLists.First(p => p.IdProductNavigation == SelectedItem!).Count += 1;
            }
            else
            {
                DatabaseContext.Entities.ProductLists.Local.Add(new ProductList()
                {
                    IdBasketNavigation = basket,
                    IdProductNavigation = SelectedItem!,
                    Count = 1
                });
            }
        }

        /// <summary>
        /// Создание новой корзины
        /// </summary>
        private static void CreateBasket() =>
            DatabaseContext.Entities.Baskets.Local.Add(new Basket()
            {
                IdClientNavigation = App.CarrentUser.Client!,
                AllCost = 0,
                CountProduct = 0
            });

    }
}

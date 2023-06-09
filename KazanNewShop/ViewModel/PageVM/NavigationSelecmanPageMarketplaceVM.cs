﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.Services;
using KazanNewShop.View.Pages.MainPages;
using KazanNewShop.View.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace KazanNewShop.ViewModel.PageVM
{
    public partial class NavigationSelecmanPageMarketplaceVM : ObservableValidator
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

        public static NavigationSelecmanPageMarketplaceVM Instance { get; private set; } = null!;

        // Кнопка мои заказы
        private bool _isChekedMyProduct = false;
        public bool IsChekedMyProduct
        {
            get => _isChekedMyProduct;
            set
            {
                _isChekedMyProduct = value;
                if (IsChekedMyProduct)
                    FilterShowMyProduct();
                else
                    ViewProducts.Filter = (obj) => true;
            }
        }

        // Иконка продавца
        [ObservableProperty]
        private byte[] _salesmanPhoto
            = App.CurrentUser!.Salesman!.ProfilePhoto! == null ? CommonMethods.MainForProfileClientNullPhoto : App.CurrentUser.Salesman!.ProfilePhoto!;

        // Список категорий
        public ObservableCollection<Category> Category { get; } = DatabaseContext.Entities.Categories.Local.ToObservableCollection();

        // Список всех продуктов
        public ICollectionView ViewProducts { get; set; }
            = CollectionViewSource.GetDefaultView
                (
                    DatabaseContext.Entities.Products.Local.ToObservableCollection()
                    .Where(p => (p.IdStatus == 1 && p.Removed == false) || (p.Removed == false && p.Salesman == App.CurrentUser!.Salesman))
                );

        [ObservableProperty]
        private int _countProdutsInBasket = 0;

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
        public NavigationSelecmanPageMarketplaceVM() =>
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
        /// Филтрация товаров для вывода товара продовца
        /// </summary>
        public void FilterShowMyProduct()
        {
            ViewProducts.Filter = (obj) =>
            {
                Product product = (obj as Product)!;

                if (product.Salesman == App.CurrentUser!.Salesman)
                    return true;

                return false;
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
        /// Открытие списка всех заказов
        /// </summary>
        [RelayCommand]
        public void OpenOrdersList()
        {
            NavigationWindow.Navigate(typeof(ListOrderPageVM));
        }

        /// <summary>
        /// Открытие графика продаж
        /// </summary>
        [RelayCommand]
        public void OpenSchedul()
        {
            NavigationWindow.Navigate(typeof(ProductShoppingSchedulePageVM));
        }

        /// <summary>
        /// Создание заказа
        /// </summary>
        [RelayCommand]
        public void CreatedProduct()
        {
            new EditOrAddProduct(new Product(), "Создание товара").ShowDialog();
        }

        /// <summary>
        /// Открытие окна подробнее о товаре 
        /// </summary>
        [RelayCommand]
        public void ProductDetails(Product SelectedItem)
        {
            if (SelectedItem.Salesman == App.CurrentUser!.Salesman!)
                new EditOrAddProduct(SelectedItem, "Редактирование").Show();
            else
                new ProductDetails(SelectedItem).ShowDialog();
        }

        /// <summary>
        /// Открытие окна пользователя 
        /// </summary>
        [RelayCommand]
        private static void OpenPersonalPage() =>
            new PersonalSalesmanPageWindow().ShowDialog();
    }
}

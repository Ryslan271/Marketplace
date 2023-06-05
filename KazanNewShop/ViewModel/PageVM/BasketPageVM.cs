using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.View.Windows;
using KazanNewShop.ViewModel.PageVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace KazanNewShop.ViewModel
{
    public partial class BasketPageVM : ObservableValidator
    {
        // список продуктов в корзине
        public ICollectionView ViewProductsInBasket { get; }
            = CollectionViewSource.GetDefaultView(DatabaseContext.Entities.Baskets.Local
                .First(b => b.Client == App.CurrentUser!.Client).ProductLists);

        [ObservableProperty]
        private string? _name;

        // Общая стоимость корзины
        public decimal? AllCostBasket => TotalCostCalculation();

        [ObservableProperty]
        private string? _search;

        /// <summary>
        /// Удаление -1 к указанному товару
        /// </summary>
        [RelayCommand]
        public void DeleteOneSelectedProduct(ProductList SelectedItem)
        {
            if (SelectedItem.Count - 1 < 1)
                return;

            if (SelectedItem.Product.Count <= SelectedItem.Count)
                SelectedItem.IsEnableButtomPlus = false;

            SelectedItem.Count -= 1;

            SelectedItem.Product.CountInBasket = SelectedItem.Count;

            ViewProductsInBasket.Refresh();

            DatabaseContext.Entities.SaveChanges();

            OnPropertyChanged(nameof(AllCostBasket));
        }

        /// <summary>
        /// Добавление +1 к указанному товару
        /// </summary>
        [RelayCommand]
        public void AddOneSelectedProduct(ProductList SelectedItem)
        {
            if (SelectedItem.Product.Count - 1 >= SelectedItem.Count)
                SelectedItem.Count += 1;
            else
                SelectedItem.IsEnableButtomPlus = true;

            SelectedItem.Product.CountInBasket = SelectedItem.Count;

            ViewProductsInBasket.Refresh();

            DatabaseContext.Entities.SaveChanges();

            OnPropertyChanged(nameof(AllCostBasket));
        }

        /// <summary>
        /// Удаление товара из корзины
        /// </summary>
        [RelayCommand]
        public void DeleteProduct(ProductList SelectedItem)
        {
            DatabaseContext.Entities.ProductLists.Local.Remove(SelectedItem);

            ViewProductsInBasket.Refresh();

            DatabaseContext.Entities.SaveChanges();

            OnPropertyChanged(nameof(AllCostBasket));
        }

        /// <summary>
        /// Команда поиска по товарам
        /// </summary>
        [RelayCommand]
        public void ProductSearch()
        {
            if (Search == null)
            {
                ViewProductsInBasket.Filter = (obj) => true;
                return;
            }

            ViewProductsInBasket.Filter = (obj) =>
            {
                string? search = Search?.ToLower().Trim();

                var product = obj as Product;

                if (product?.Name.ToLower().Contains(search!) == false)
                    return false;

                return true;
            };
        }

        /// <summary>
        /// Подсчет общей стоимости корзины
        /// </summary>
        public decimal? TotalCostCalculation() =>
             DatabaseContext.Entities.Baskets.Local
            .FirstOrDefault(b => App.CurrentUser!.Client!.Baskets.Contains(b))!
            .ProductLists.Select(p => p.Product)
            .Sum(p => p.CostWithDiscount * Convert.ToDecimal(DatabaseContext.Entities.Baskets.Local.ToObservableCollection()
                                                                                                     .FirstOrDefault(b => b == App.CurrentUser!.Client!.Baskets.First())!
                                                                                                     .ProductLists.First(product => product.Product == p).Count));

        /// <summary>
        /// Команда перехода на страницу продуктов
        /// </summary>
        [RelayCommand]
        public void OpenProductsList() =>
            NavigationWindow.TransitionProductList(typeof(NavigationPageMarketplaceVM));

        /// <summary>
        /// Создание нового заказа
        /// </summary>
        [RelayCommand]
        public void CreatingOrder()
        {
            List<ProductListOrder> localListProductListOrder = new();

            Order order = new()
            {
                Client = App.CurrentUser!.Client!,
            };

            foreach (var item in DatabaseContext.Entities.Baskets.Local.FirstOrDefault(b => b == App.CurrentUser!.Client!.Baskets.First())!.ProductLists)
            {
                localListProductListOrder.Add
                    (
                        new ProductListOrder()
                        {
                            Cost = item.Product.CostWithDiscount,
                            Count = item.Product.Count,
                            Product = item.Product,
                            Order = order
                        }
                    );
            }

            new AddSelectorAddress(order, localListProductListOrder).ShowDialog();
        }
    }
}

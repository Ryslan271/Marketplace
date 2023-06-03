using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.View.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace KazanNewShop.ViewModel
{
    public partial class ListOrderPageVM : ObservableValidator
    {
        // список заказов
        public ICollectionView ViewProductsInOrder { get; }
            = CollectionViewSource.GetDefaultView
            (
                DatabaseContext.Entities.ProductListOrders.Local
                .Where(p => p.Product.Salesman == App.CurrentUser!.Salesman)
                .Select(p => p.Order)
            );

        // список статусов заказа
        public IEnumerable<OrderStatus> OrderStatus { get; } = DatabaseContext.Entities.OrderStatuses.Local;

        [ObservableProperty]
        private OrderStatus _orderStatusSelectedItem = DatabaseContext.Entities.OrderStatuses.Local.First();

        [ObservableProperty]
        private Order _orderSelectedItem;

        [ObservableProperty]
        private string? _search;

        /// <summary>
        /// Команда поиска по товарам
        /// </summary>
        [RelayCommand]
        public void ProductSearch()
        {
            if (Search == null)
            {
                ViewProductsInOrder.Filter = (obj) => true;
                return;
            }

            ViewProductsInOrder.Filter = (obj) =>
            {
                string? search = Search?.ToLower().Trim();

                IEnumerable<ProductListOrder> productListOrders = (obj as Order)!.ProductListOrders;

                foreach (var product in productListOrders)
                    if (product.Product?.Name.ToLower().Contains(search!) == false)
                        return false;

                return true;
            };
        }

        /// <summary>
        /// Создание заказа
        /// </summary>
        [RelayCommand]
        public void CretingOrder()
        {
            OrderSelectedItem.OrderStatus = OrderStatusSelectedItem;

           
            DatabaseContext.Entities.SaveChanges();
        }

        /// <summary>
        /// Оформление заказа
        /// </summary>
        [RelayCommand]
        public void MakeOrder()
        {
        }

        /// <summary>
        /// Открытие списка продуктов в заказе
        /// </summary>
        [RelayCommand]
        public void OrderProductList(Order SelectOrder) =>
            new OrderProductList(SelectOrder).Show();

        /// <summary>
        /// Открытие все товары
        /// </summary>
        [RelayCommand]
        public void OpenProductList() =>
             NavigationWindow.Navigate(typeof(NavigationSelecmanPageMarketplaceVM));
    }
}

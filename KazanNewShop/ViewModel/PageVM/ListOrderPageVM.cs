using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.View.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace KazanNewShop.ViewModel.PageVM
{
    public partial class ListOrderPageVM : ObservableValidator
    {
        // список заказов
        public ICollectionView ViewProductsInOrder { get; }
            = CollectionViewSource.GetDefaultView
            (
                DatabaseContext.Entities.ProductListOrders.Local
                .ToObservableCollection()
                .Where(p => p.Product.Salesman == App.CurrentUser!.Salesman)
                .Select(p => p.Order)
                .Distinct()
            );


        private OrderStatus? _statusSelectedItem;
        public OrderStatus? StatusSelectedItem
        {
            get => _statusSelectedItem;
            set
            {
                _statusSelectedItem = value;
                OrderSorted();
            }
        }
        public List<OrderStatus> Statuses { get; } = DatabaseContext.Entities.OrderStatuses.Local.ToList();

        // список статусов заказа
        public ObservableCollection<OrderStatus> OrderStatus { get; } = DatabaseContext.Entities.OrderStatuses.Local.ToObservableCollection();

        [ObservableProperty]
        private OrderStatus _orderStatusSelectedItem = DatabaseContext.Entities.OrderStatuses.Local.ToObservableCollection().First();

        [ObservableProperty]
        private Order? _orderSelectedItem;

        [ObservableProperty]
        private string? _search;

        public ListOrderPageVM()
        {
            if (Statuses.FirstOrDefault(x => x.Name == "Все") == null)
                Statuses.Insert(0, new OrderStatus() { Name = "Все" });

            StatusSelectedItem = Statuses.First();
        }

        /// <summary>
        /// Команда сортировка заказов
        /// </summary>
        [RelayCommand]
        public void OrderSorted()
        {
            ViewProductsInOrder.Filter = (obj) =>
            {
                Order? order = obj as Order;

                return _statusSelectedItem == Statuses.First()
                      || order!.OrderStatus == _statusSelectedItem;
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
        public void MakeOrder(Order selectOrder)
        {
            DatabaseContext.Entities.SaveChanges();
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

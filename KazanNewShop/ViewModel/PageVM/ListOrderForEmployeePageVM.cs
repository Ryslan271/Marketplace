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
using System.Windows.Data;

namespace KazanNewShop.ViewModel.PageVM
{
    public partial class ListOrderForEmployeePageVM : ObservableValidator
    {
        // список заказов
        public ICollectionView ViewProductsInOrder { get; }
            = CollectionViewSource.GetDefaultView
            (
                DatabaseContext.Entities.ProductListOrders.Local
                .ToObservableCollection()
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

        // список статусов заказа
        public List<OrderStatus> Statuses { get; } = DatabaseContext.Entities.OrderStatuses.Local.ToList();

        // выбранный статус для сортирвки 
        [ObservableProperty]
        private OrderStatus _orderStatusSelectedItem = DatabaseContext.Entities.OrderStatuses.Local.ToObservableCollection().First();

        // Выбранный казан из списка
        [ObservableProperty]
        private Order? _orderSelectedItem;
        
        // строка поиска
        [ObservableProperty]
        private string? _search;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ListOrderForEmployeePageVM()
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
        /// Оформление заказа
        /// </summary>
        [RelayCommand]
        public void MakeOrder(Order selectOrder)
        {
            DatabaseContext.Entities.SaveChanges();


            DatabaseContext.LoadEntitesForMarketplace();
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
             NavigationWindow.Navigate(typeof(NavigationEmployeePageMarketplaceVM));

        /// <summary>
        /// Открытие окна создание нового пункта выдачи 
        /// </summary>
        [RelayCommand]
        public void CreatedPointOfAddress()
        {
            new AddNewAddress().ShowDialog();
        }
    }
}

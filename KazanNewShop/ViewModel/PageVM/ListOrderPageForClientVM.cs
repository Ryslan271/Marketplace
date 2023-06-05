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
    public partial class ListOrderPageForClientVM : ObservableValidator
    {
        // список заказов
        public ICollectionView ViewProductsInOrder { get; }
            = CollectionViewSource.GetDefaultView
            (
                DatabaseContext.Entities.ProductListOrders.Local
                .ToObservableCollection()
                .Where(p => p.Order.Client == App.CurrentUser!.Client)
                .Select(p => p.Order)
                .Distinct()
            );


        private TypeReturn? _statusSelectedItem;
        public TypeReturn? StatusSelectedItem
        {
            get => _statusSelectedItem;
            set
            {
                _statusSelectedItem = value;
                OrderSorted();
            }
        }
        public List<TypeReturn> TypeReturnsForSort { get; } = DatabaseContext.Entities.TypeReturns.ToList();

        // список статусов заказа
        public ObservableCollection<TypeReturn> TypeReturns { get; } = DatabaseContext.Entities.TypeReturns.Local.ToObservableCollection();

        [ObservableProperty]
        private OrderStatus _orderStatusSelectedItem = DatabaseContext.Entities.OrderStatuses.Local.ToObservableCollection().First();

        [ObservableProperty]
        private Order? _orderSelectedItem;

        [ObservableProperty]
        private string? _search;

        public ListOrderPageForClientVM()
        {
            if (TypeReturnsForSort.FirstOrDefault(x => x.Name == "Все") == null)
                TypeReturnsForSort.Insert(0, new TypeReturn() { Name = "Все" });

            StatusSelectedItem = TypeReturnsForSort.First();
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

                return _statusSelectedItem == TypeReturnsForSort.First()
                      || order!.TypeReturn == _statusSelectedItem;
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
        public void ChangeTypeReturnInOrder(Order selectOrder)
        {
            if (selectOrder.IdTypeReturn != null)
                selectOrder.OrderStatus = DatabaseContext.Entities.OrderStatuses.Local.Last();

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
             NavigationWindow.Navigate(typeof(NavigationPageMarketplaceVM));
    }
}

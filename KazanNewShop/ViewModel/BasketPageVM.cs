using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.View.Windows;
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
                .First(b => b.Client == App.CarrentUser.Client).ProductLists);

        // Нужная корзина 
        private Basket _basket
            = DatabaseContext.Entities.Baskets.Local.ToObservableCollection().FirstOrDefault(b => b == App.CarrentUser.Client!.Baskets)!;

        [ObservableProperty]
        private string? _name;

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

            SelectedItem.Count -= 1;

            SelectedItem.Product.CountInBasket = SelectedItem.Count;

            ViewProductsInBasket.Refresh();
        }

        /// <summary>
        /// Добавление +1 к указанному товару
        /// </summary>
        [RelayCommand]
        public void AddOneSelectedProduct(ProductList SelectedItem)
        {
            SelectedItem.Count += 1;

            SelectedItem.Product.CountInBasket = SelectedItem.Count;

            ViewProductsInBasket.Refresh();
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
        /// Команда перехода на страницу продуктов
        /// </summary>
        [RelayCommand]
        public void OpenProductsList() =>
            NavigationWindow.TransitionProductList(typeof(NavigationPageMarketplaceVM));

        /// <summary>
        /// Создание и открытие заказа
        /// </summary>
        [RelayCommand]
        public void OpenCreatingOrder()
        {
            NavigationWindow.TransitionProductList(typeof(OrderPageVM));
            DatabaseContext.Entities.SaveChanges();
        }
    }
}

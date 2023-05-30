using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.View.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace KazanNewShop.ViewModel
{
    public partial class OrderPageVM : ObservableValidator
    {
        // список продуктов в корзине
        public ICollectionView ViewProductsInBasket { get; }
            = CollectionViewSource.GetDefaultView(DatabaseContext.Entities.Baskets.Local
                .First(b => b.Client == App.CurrentUser.Client).ProductLists);

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
        /// Создание заказа
        /// </summary>
        [RelayCommand]
        public void CretingOrder()
        {
            DatabaseContext.Entities.SaveChanges();

            new OrderCreationWindow().ShowDialog();
        }

        /// <summary>
        /// Открытие корзины
        /// </summary>
        [RelayCommand]
        public void OpenBasket() =>
            NavigationWindow.Navigate(typeof(BasketPageVM));
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.View.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace KazanNewShop.ViewModel
{
    public partial class BasketPageVM : ObservableValidator
    {
        // список продуктов в корзине
        public ICollectionView ViewProductsInBasket { get; }
            = CollectionViewSource.GetDefaultView(DatabaseContext.Entities.ProductLists.Local.ToObservableCollection()
                .Where(p => p.IdBasketNavigation == DatabaseContext.Entities.Baskets.Local.ToObservableCollection().FirstOrDefault(b => b == App.CarrentUser.Client!.Baskets))
                .Select(p => p.IdProductNavigation));

        // Нужная корзина 
        private Basket _basket 
            = DatabaseContext.Entities.Baskets.Local.ToObservableCollection().FirstOrDefault(b => b == App.CarrentUser.Client!.Baskets)!;

        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _search;

        [ObservableProperty]
        private Product? _selectedItem;

        /// <summary>
        /// Удаление -1 к указанному товару
        /// </summary>
        [RelayCommand]
        public void DeleteOneSelectedProduct()
        {
            DatabaseContext.Entities.ProductLists.Local
                .FirstOrDefault(p => p.IdProductNavigation == SelectedItem &&
                                     p.IdBasketNavigation == _basket)!
                .Count -= 1;

            ViewProductsInBasket.Refresh();

            DatabaseContext.Entities.SaveChanges();
        }

        /// <summary>
        /// Добавление +1 к указанному товару
        /// </summary>
        [RelayCommand]
        public void AddOneSelectedProduct()
        {
            DatabaseContext.Entities.ProductLists.Local
                .FirstOrDefault(p => p.IdProductNavigation == SelectedItem &&
                                     p.IdBasketNavigation == _basket)!
                .Count += 1;

            ViewProductsInBasket.Refresh();

            DatabaseContext.Entities.SaveChanges();
        }

        /// <summary>
        /// Удаление товара из корзины
        /// </summary>
        [RelayCommand]
        public void DeleteProduct() 
        {
            DatabaseContext.Entities.ProductLists.Local.Remove
                (
                    DatabaseContext.Entities.ProductLists.Local.FirstOrDefault(p => p.IdBasketNavigation == _basket)!
                );

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
            NavigationWindow.TransitionProductList();
    }
}

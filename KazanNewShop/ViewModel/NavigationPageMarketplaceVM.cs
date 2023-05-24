using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.View.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KazanNewShop.ViewModel
{
    public partial class NavigationPageMarketplaceVM : ObservableValidator
    {
        public ObservableCollection<Category> Category { get; } = DatabaseContext.Entities.Categories.Local.ToObservableCollection();

        public ObservableCollection<Product> Products { get; } = DatabaseContext.Entities.Products.Local.ToObservableCollection();

        [ObservableProperty]
        private Product? _selectedItem;

        /// <summary>
        /// Открытие корзины
        /// </summary>
        [RelayCommand]
        public void OpenBasket()
        {

        }

        [RelayCommand]
        public void AddProductInBasket()
        {
            if (SelectedItem == null)
                return;

            if (DatabaseContext.Entities.Baskets.Local.Any(b => b.IdClientNavigation == App.CarrentUser.Client) != true)
                CreateBasket();

            Basket basket = DatabaseContext.Entities.Baskets.Local.First(b => b.IdClientNavigation == App.CarrentUser.Client);

            if (basket.ProductLists.Any(p => p.IdProductNavigation == SelectedItem!))
            {NavigationWindow.Navigate(typeof(NavigationPageMarketplaceVM));
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

        private static void CreateBasket() =>
            DatabaseContext.Entities.Baskets.Local.Add(new Basket()
            {
                IdClientNavigation = App.CarrentUser.Client!,
                AllCost = 0,
                CountProduct = 0
            });

    }
}

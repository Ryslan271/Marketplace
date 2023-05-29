﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.ViewModel.Base;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace KazanNewShop.ViewModel
{
    public partial class ProductDetailsVM : WindowViewModelBase
    {
        // Видимость первой картинки
        [ObservableProperty]
        private Visibility _visiblyFirstImage = Visibility.Collapsed;
        [ObservableProperty]
        private Visibility _visiblyLastImage = Visibility.Visible;

        // Выбранный продукт
        [ObservableProperty]
        private Product _currentProduct;

        // Список всех картинок
        [ObservableProperty]
        private List<byte[]?> _images;

        // 3 картинки для вывода
        [ObservableProperty]
        private byte[]? _prevPhoto, _currentPhoto, _nextPhoto;
        private int _currPhotoIndex;

        /// <summary>
        /// Конструктор VM
        /// </summary>
        /// <param name="product">Выбранный продукт</param>
        public ProductDetailsVM(Product product)
        {
            CurrentProduct = product;

            Images ??= DatabaseContext.Entities.PhotoProducts.Local
                .ToObservableCollection().Where(p => p.Product == CurrentProduct).Select(p => p.Photo).ToList()!;

            CurrentPhoto = Images.FirstOrDefault();
            NextPhoto = Images.Count > 1 ? Images[1] : null; 
        }

        /// <summary>
        /// Команда прокрутки картинки назад
        /// </summary>
        [RelayCommand]
        public void ImageScrollingDown()
        {
            if (_currPhotoIndex <= 0)
            {
                VisiblyFirstImage = Visibility.Collapsed;
                return;
            }

            PrevPhoto = CurrentPhoto;
            _currPhotoIndex -= 1;
            CurrentPhoto = Images[_currPhotoIndex];
            NextPhoto = Images[_currPhotoIndex + 1];
            ImageScrollingDownCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Команда прокрутки картинки вперед
        /// </summary>
        [RelayCommand]
        public void ImageScrollingUp()
        {
            if (_currPhotoIndex >= Images.Count - 1)
            {
                VisiblyLastImage = Visibility.Collapsed;
                VisiblyFirstImage = Visibility.Visible;
                return;
            }
            else
                VisiblyLastImage = Visibility.Visible;

            VisiblyFirstImage = Visibility.Visible;

            PrevPhoto = CurrentPhoto;
            _currPhotoIndex += 1;
            CurrentPhoto = Images[_currPhotoIndex];
            if (_currPhotoIndex + 1 <= Images.Count - 1)
                NextPhoto = Images[_currPhotoIndex + 1];
            ImageScrollingUpCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Команда добавление товара в корзину
        /// </summary>
        [RelayCommand]
        public void AddProductInBasket()
        {
            ValidateExistenceBasket();

            Basket basket = DatabaseContext.Entities.Baskets.Local.First(b => b.Client == App.CarrentUser.Client);

            if (basket.ProductLists.Any(p => p.Product == CurrentProduct!) == true)
            {
                basket.ProductLists.First(p => p.Product == CurrentProduct!).Count += 1;

                CurrentProduct.CountInBasket = basket.ProductLists.First(p => p.Product == CurrentProduct!).Count;
            }
            else
            {
                DatabaseContext.Entities.ProductLists.Local.Add(new ProductList()
                {
                    Basket = basket,
                    Product = CurrentProduct!,
                    Count = 1
                });

                CurrentProduct.CountInBasket = basket.ProductLists.First(p => p.Product == CurrentProduct!).Count;
            }

            ValidateCountInProductBasket(CurrentProduct);

            NavigationPageMarketplaceVM.ViewProducts.Refresh();
        }

        /// <summary>
        /// Команда удаление товара в корзину
        /// </summary>
        [RelayCommand]
        public void DeleteProductInBasket()
        {
            ValidateExistenceBasket();

            Basket basket = DatabaseContext.Entities.Baskets.Local.First(b => b.Client == App.CarrentUser.Client);

            if (basket.ProductLists.First(p => p.Product == CurrentProduct!).Count - 1 > 0)
            {
                basket.ProductLists.First(p => p.Product == CurrentProduct!).Count -= 1;

                CurrentProduct.CountInBasket = basket.ProductLists.First(p => p.Product == CurrentProduct!).Count;
            }
            else
            {
                DatabaseContext.Entities.ProductLists.Local.Remove(basket.ProductLists.First(p => p.Product == CurrentProduct!));

                CurrentProduct.CountInBasket = 0;
            }

            ValidateCountInProductBasket(CurrentProduct );

            NavigationPageMarketplaceVM.ViewProducts.Refresh();
        }

        /// <summary>
        /// Проверка количество, для показать или скрить элементы
        /// </summary>
        /// <param name="SelectedItem">Выбранный элемент</param>
        private static void ValidateCountInProductBasket(Product SelectedItem)
        {
            if (SelectedItem.CountInBasket <= 0)
            {
                SelectedItem.VisibilyButtonProductNotInCart = Visibility.Visible;
                SelectedItem.VisibilyButtonProductInCart = Visibility.Collapsed;
            }
            else
            {
                SelectedItem.VisibilyButtonProductNotInCart = Visibility.Collapsed;
                SelectedItem.VisibilyButtonProductInCart = Visibility.Visible;
            }
        }

        /// <summary>
        /// Проверка на существование корзины
        /// </summary>
        private static void ValidateExistenceBasket()
        {
            if (DatabaseContext.Entities.Baskets.Local.Any(b => b.Client == App.CarrentUser.Client) != true)
                CreateBasket();
        }

        /// <summary>
        /// Создание новой корзины
        /// </summary>
        private static void CreateBasket() =>
            DatabaseContext.Entities.Baskets.Local.Add(new Basket()
            {
                Client = App.CarrentUser.Client!
            });
    }
}

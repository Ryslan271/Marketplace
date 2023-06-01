using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.Services;
using KazanNewShop.View.Windows;
using KazanNewShop.ViewModel.Base;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KazanNewShop.ViewModel
{
    public partial class ProductDetailsVM : WindowViewModelBase
    {
        // Текущий VM
        public static ProductDetailsVM Instance = null!;

        // Видимость первой картинки
        [ObservableProperty]
        private Visibility _visiblyFirstImage = Visibility.Collapsed;
        [ObservableProperty]
        private Visibility _visiblyLastImage = Visibility.Visible;

        // Кружки для списка картинок
        [ObservableProperty]
        private List<Ellipse> _ellipses = new();

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

            if (Images.Count == 0)
                Images.Add(CommonMethods.MainForProductNullPhoto);

            for (int i = 0; i < Images.Count; i++)
                Ellipses.Add
                (
                    new Ellipse()
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.Gray
                    }
                );

            Ellipses[0].Fill = Brushes.Wheat;

            CurrentPhoto = Images.FirstOrDefault();
            NextPhoto = Images.Count > 1 ? Images[1] : null;

            Instance = this;
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

            Ellipses.ForEach(e => e.Fill = Brushes.Gray);

            PrevPhoto = _currPhotoIndex - 2 >= 0 ? Images[_currPhotoIndex - 2] : null; ;
            _currPhotoIndex -= 1;
            Ellipses[_currPhotoIndex].Fill = Brushes.Wheat;
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

            Ellipses.ForEach(e => e.Fill = Brushes.Gray);

            PrevPhoto = CurrentPhoto;
            _currPhotoIndex += 1;
            Ellipses[_currPhotoIndex].Fill = Brushes.Wheat;
            CurrentPhoto = Images[_currPhotoIndex];
            NextPhoto = _currPhotoIndex + 1 <= Images.Count - 1 ? Images[_currPhotoIndex + 1] : null;
            ImageScrollingUpCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Команда добавление товара в корзину
        /// </summary>
        [RelayCommand]
        public void AddProductInBasket()
        {
            ValidateExistenceBasket();

            Basket basket = DatabaseContext.Entities.Baskets.Local.First(b => b.Client == App.CurrentUser.Client);

            if (basket.ProductLists.Any((System.Func<ProductList, bool>)(p => p.Product == this.CurrentProduct!)) == true)
            {
                basket.ProductLists.First((System.Func<ProductList, bool>)(p => p.Product == this.CurrentProduct!)).Count += 1;

                this.CurrentProduct.CountInBasket = basket.ProductLists.First((System.Func<ProductList, bool>)(p => p.Product == this.CurrentProduct!)).Count;
            }
            else
            {
                DatabaseContext.Entities.ProductLists.Local.Add(new ProductList()
                {
                    Basket = basket,
                    Product = this.CurrentProduct!,
                    Count = 1
                });

                this.CurrentProduct.CountInBasket = basket.ProductLists.First((System.Func<ProductList, bool>)(p => p.Product == this.CurrentProduct!)).Count;
            }

            ValidateCountInProductBasket(CurrentProduct);

            NavigationPageMarketplaceVM.Instance.ViewProducts.Refresh();

            ProductDetails.Instance.DataContext = null;
            ProductDetails.Instance.DataContext = Instance;
        }

        /// <summary>
        /// Команда удаление товара в корзину
        /// </summary>
        [RelayCommand]
        public void DeleteProductInBasket()
        {
            ValidateExistenceBasket();

            Basket basket = DatabaseContext.Entities.Baskets.Local.First(b => b.Client == App.CurrentUser.Client);

            if (basket.ProductLists.First((System.Func<ProductList, bool>)(p => p.Product == this.CurrentProduct!)).Count - 1 > 0)
            {
                basket.ProductLists.First((System.Func<ProductList, bool>)(p => p.Product == this.CurrentProduct!)).Count -= 1;

                this.CurrentProduct.CountInBasket = basket.ProductLists.First((System.Func<ProductList, bool>)(p => p.Product == this.CurrentProduct!)).Count;
            }
            else
            {
                DatabaseContext.Entities.ProductLists.Local.Remove(basket.ProductLists.First((System.Func<ProductList, bool>)(p => p.Product == this.CurrentProduct!)));

                this.CurrentProduct.CountInBasket = 0;
            }

            ValidateCountInProductBasket(CurrentProduct );

            NavigationPageMarketplaceVM.Instance.ViewProducts.Refresh();

            ProductDetails.Instance.DataContext = null;
            ProductDetails.Instance.DataContext = Instance;
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
            if (DatabaseContext.Entities.Baskets.Local.Any(b => b.Client == App.CurrentUser.Client) != true)
                CreateBasket();
        }

        /// <summary>
        /// Создание новой корзины
        /// </summary>
        private static void CreateBasket() =>
            DatabaseContext.Entities.Baskets.Local.Add(new Basket()
            {
                Client = App.CurrentUser.Client!
            });
    }
}

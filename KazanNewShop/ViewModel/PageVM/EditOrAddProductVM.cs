using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.Services;
using KazanNewShop.View.Windows;
using KazanNewShop.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KazanNewShop.ViewModel.PageVM
{
    public partial class EditOrAddProductVM : WindowViewModelBase
    {
        #region Свойста окна

        // Текущий VM
        public static EditOrAddProductVM Instance = null!;

        // Title
        [ObservableProperty]
        private string _title;

        // 3 картинки для вывода
        [ObservableProperty]
        private byte[]? _prevPhoto, _currentPhoto, _nextPhoto;
        private int _currPhotoIndex;

        // Видимость первой картинки
        [ObservableProperty]
        private Visibility _visiblyFirstImage = Visibility.Collapsed;
        [ObservableProperty]
        private Visibility _visiblyLastImage = Visibility.Visible;

        // Список всех картинок
        [ObservableProperty]
        private List<byte[]?> _images;

        // Список всех картинок на удаление
        [ObservableProperty]
        private List<byte[]?> _imagesIsRemoved = new();

        #region Выбранный продукт/новый продукт

        [Required(ErrorMessage = "Заполните все поля")]
        private Product _currentProduct;

        [Required(ErrorMessage = "Заполните все поля")]
        public Product CurrentProduct
        {
            get => _currentProduct;
            set
            {
                ValidateProperty(value);
                _currentProduct = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Заполните все поля")]
        public string Name
        {
            get => _currentProduct.Name;
            set
            {
                ValidateProperty(value);
                _currentProduct.Name = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Заполните все поля")]
        public int? Count
        {
            get => _currentProduct.Count;
            set
            {
                ValidateProperty(value);
                _currentProduct.Count = Convert.ToInt32(value);
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Заполните все поля")]
        public decimal? Cost
        {
            get => _currentProduct.Cost;
            set
            {
                ValidateProperty(value);
                _currentProduct.Cost = Convert.ToInt32(value);
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Заполните все поля")]
        public int? Discount
        {
            get => _currentProduct.Discount;
            set
            {
                ValidateProperty(value);
                _currentProduct.Discount = Convert.ToInt32(value);
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Заполните все поля")]
        public string Description
        {
            get => _currentProduct.Description;
            set
            {
                ValidateProperty(value);
                _currentProduct.Description = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Заполните все поля")]
        public string? Characteristics
        {
            get => _currentProduct.Characteristics;
            set
            {
                ValidateProperty(value);
                _currentProduct.Characteristics = value;
                OnPropertyChanged();
            }
        }
        #endregion

        // Выбранный продукт
        [ObservableProperty]
        private Category? _cetegorySelectedItem;

        // Кружки для списка картинок
        [ObservableProperty]
        private List<Ellipse> _ellipses = new();

        // Список категорий
        public IEnumerable<Category> Category { get; } = DatabaseContext.Entities.Categories.Local.ToObservableCollection();

        #endregion

        /// <summary>
        /// Конструктор VM
        /// </summary>
        /// <param name="product">Выбранный продукт</param>
        public EditOrAddProductVM(Product product, string title)
        {
            CurrentProduct = product;

            Images ??= DatabaseContext.Entities.PhotoProducts.Local
                .ToObservableCollection().Where(p => p.Product == CurrentProduct).Select(p => p.Photo).ToList()!;

            WorkWithImages();

            Instance = this;

            Instance.Title = title;
        }

        /// <summary>
        /// Команда добавление новой картинки
        /// </summary>
        [RelayCommand]
        public void AddImage()
        {
            byte[] newImage = CommonMethods.ConvertImage()!;

            Images.Add(newImage);

            _currPhotoIndex = 0;

            WorkWithImages();
        }

        /// <summary>
        /// Команда удаление центральной картинки
        /// </summary>
        [RelayCommand]
        public void RemoveCurrentImage()
        {
            Images.Remove(CurrentPhoto);

            ImagesIsRemoved.Add(CurrentPhoto);

            _currPhotoIndex = 0;
            VisiblyFirstImage = Visibility.Collapsed;

            UpdateEllipseList();
        }

        /// <summary>
        /// Работа с списком картинок и списком кружком
        /// </summary>
        private void WorkWithImages()
        {
            if (Images.Count == 0)
                Images.Add(CommonMethods.MainForProductNullPhoto);
            else
                Images.Remove(CommonMethods.MainForProductNullPhoto);

            UpdateEllipseList();
        }

        /// <summary>
        /// Метод обновление списка кружков
        /// </summary>
        private void UpdateEllipseList()
        {
            Ellipses.Clear();

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

            if (Ellipses.Count <= 0)
                return;

            Ellipses[0].Fill = Brushes.Wheat;

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
        /// Сохранение измененний
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanSaveChanges))]
        public void SaveChanges()
        {
            ValidateAllProperties();

            if (HasErrors)
                return;

            if (CurrentProduct.Category.Id == 0)
                return;

            foreach (var image in Images)
            {
                if (DatabaseContext.Entities.PhotoProducts.Local.Select(p => p.Photo).Contains(image) == true)
                    continue;

                DatabaseContext.Entities.PhotoProducts.Local.Add
                    (
                        new PhotoProduct()
                        {
                            Product = CurrentProduct,
                            Photo = image!
                        }
                    );
            }

            foreach (var image in ImagesIsRemoved)
            {
                if (DatabaseContext.Entities.PhotoProducts.Local.Select(p => p.Photo).Contains(image) == false)
                    continue;

                DatabaseContext.Entities.PhotoProducts.Local.Remove
                   (
                       DatabaseContext.Entities.PhotoProducts.Local.First(p => p.Photo == image)
                   );
            }

            CurrentProduct.Salesman = App.CurrentUser!.Salesman;

            CurrentProduct.IdStatus = 1; // изменить на 2

            DatabaseContext.Entities.SaveChanges();

            NavigationWindow.IssuingImage();

            NavigationEmployeePageMarketplaceVM.Instance.ViewProducts.Refresh();

            CloseWindow();
        }
        private bool CanSaveChanges() => HasErrors == false;
    }
}

﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.Services;
using KazanNewShop.ViewModel.Base;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KazanNewShop.ViewModel
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

        // Выбранный продукт
        [ObservableProperty]
        private Product _currentProduct;

        // Выбранный продукт
        [ObservableProperty]
        private Category? _cetegorySelectedItem;

        // Кружки для списка картинок
        [ObservableProperty]
        private List<Ellipse> _ellipses = new();

        // Список категорий
        public IEnumerable<Category> Category { get; } = DatabaseContext.Entities.Categories.Local.ToObservableCollection().Skip(1);

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

            WorkWithImages();
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

            DatabaseContext.Entities.SaveChanges();

            NavigationPageMarketplaceVM.ViewProducts.Refresh();

            CloseWindow();
        }
        private bool CanSaveChanges() => HasErrors == false;
    }
}

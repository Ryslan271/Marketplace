using KazanNewShop.Services;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace KazanNewShop.Database.Models
{
    public partial class Product
    {
        private byte[]? _mainPhoto;
        [NotMapped]
        public byte[]? MainPhoto
        {
            get => _mainPhoto ??= CommonMethods.MainForProductNullPhoto;
            set
            {
                _mainPhoto = value;
            }
        }

        private int? _countInBasket;
        [NotMapped]
        public int? CountInBasket
        {
            get
            {
                var count = DatabaseContext.Entities.ProductLists.Local.ToObservableCollection().ToList()
                            .FirstOrDefault(p => p.Product == this && p.Basket.Client == App.CurrentUser!.Client);

                _countInBasket = count == null ? 0 : count.Count;

                _countInBasket ??= 0;

                return _countInBasket;
            }
            set
            {
                _countInBasket = value;
            }
        }

        private decimal? _costWithDiscount = 0;
        [NotMapped]
        public decimal? CostWithDiscount
        {
            get
            {
                _costWithDiscount = Cost - (Cost * (Discount * Convert.ToDecimal(0.01)));

                return _costWithDiscount;
            }
            set
            {
                _costWithDiscount = value;
            }
        }

        private Visibility _visibilyStatusProduct;
        [NotMapped]
        public Visibility VisibilyStatusProduct
        {
            get
            {
                _visibilyStatusProduct = App.CurrentUser!.Employee != null ? Visibility.Visible : Visibility.Collapsed;

                return _visibilyStatusProduct;
            }
            set
            {
                _visibilyStatusProduct = value;
            }
        }

        private Visibility _visibilyButtonProductNotInCart;
        [NotMapped]
        public Visibility VisibilyButtonProductNotInCart
        {
            get
            {
                _visibilyButtonProductNotInCart = CountInBasket == 0 ? Visibility.Visible : Visibility.Collapsed;

                return _visibilyButtonProductNotInCart;
            }
            set
            {
                _visibilyButtonProductNotInCart = value;
            }
        }

        private Visibility _visibilyButtonProductInCart;
        [NotMapped]
        public Visibility VisibilyButtonProductInCart
        {
            get
            {
                _visibilyButtonProductInCart = CountInBasket > 0 ? Visibility.Visible : Visibility.Collapsed;

                return _visibilyButtonProductInCart;
            }
            set
            {
                _visibilyButtonProductInCart = value;
            }
        }

        private bool _isEnableButtomPlus;
        [NotMapped]
        public bool IsEnableButtomPlus
        {
            get
            {
                return _isEnableButtomPlus;
            }
            set
            {
                _isEnableButtomPlus = value;
            }
        }

        private Brush _colorForStatus = Brushes.LightGreen;
        [NotMapped]
        public Brush ColorForStatus
        {
            get
            {
                if (Status!.Id == 2)
                    _colorForStatus = Brushes.LightSalmon;
                else if (Status!.Id == 3)
                    _colorForStatus = Brushes.DarkRed;
                else if (Status!.Id == 1)
                    _colorForStatus = Brushes.LightGreen;

                return _colorForStatus;
            }
            set
            {
                _colorForStatus = value;
            }
        }
    }
}

using KazanNewShop.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;

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

        private int _countInBasket = 0;
        [NotMapped]
        public int CountInBasket
        {
            get => _countInBasket;
            set
            {
                _countInBasket = value;
            }
        }

        private Visibility _visibilyButtonProductNotInCart = Visibility.Visible;
        [NotMapped]
        public Visibility VisibilyButtonProductNotInCart
        {
            get
            {
                if (_visibilyButtonProductNotInCart == Visibility.Visible)
                    VisibilyButtonProductInCart = Visibility.Collapsed;

                return _visibilyButtonProductNotInCart;
            }
            set
            {
                _visibilyButtonProductNotInCart = value;
            }
        }

        private Visibility _visibilyButtonProductInCart = Visibility.Collapsed;
        [NotMapped]
        public Visibility VisibilyButtonProductInCart
        {
            get
            {
                if (_visibilyButtonProductInCart == Visibility.Visible)
                    VisibilyButtonProductNotInCart = Visibility.Collapsed;

                return _visibilyButtonProductInCart;
            }
            set
            {
                _visibilyButtonProductInCart = value;
            }
        }
    }
}

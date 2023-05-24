using KazanNewShop.Services;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}

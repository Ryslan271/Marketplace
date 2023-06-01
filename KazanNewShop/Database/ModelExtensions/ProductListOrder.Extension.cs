using System.ComponentModel.DataAnnotations.Schema;

namespace KazanNewShop.Database.Models
{
    public partial class ProductListOrder
    {
        private decimal? _cost;
        [NotMapped]
        public decimal? Cost
        {
            get
            {
                _cost = Models.Product.Cost * Models.Product.Count;

                return _cost;
            }
            set
            {
                _cost = value;
            }
        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace KazanNewShop.Database.Models
{
    partial class ProductList
    {
        private decimal _cost;
        [NotMapped]
        public decimal Cost
        {
            get
            {
                _cost = Product.Cost * Count;

                return _cost;
            }
            set
            {
                _cost = value;
            }
        }
    }
}

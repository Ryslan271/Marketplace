using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace KazanNewShop.Database.Models
{
    public partial class Order
    {
        private int? _count;
        [NotMapped]
        public int? Count
        {
            get
            {
                _count = ProductListOrders.Count();

                return _count;
            }
            set
            {
                _count = value;
            }
        }

        private decimal? _allCost;
        [NotMapped]
        public decimal? AllCost
        {
            get
            {
                _allCost = ProductListOrders.Select(p => p.Product.Cost).Sum();

                return _allCost;
            }
        }

        private decimal? _totalCost;
        [NotMapped]
        public decimal? TotalCost
        {
            get
            {
                _totalCost = ProductListOrders.Select(p => p.Product.Cost - (p.Product.Cost * (p.Product.Discount * Convert.ToDecimal(0.01)))).Sum();

                return _totalCost;
            }
        }

        private decimal? _totalDiscount;
        [NotMapped]
        public decimal? TotalDiscount
        {
            get
            {
                _totalDiscount = ProductListOrders.Select(p => p.Product.Cost * (p.Product.Discount * Convert.ToDecimal(0.01))).Sum();

                return _totalDiscount;
            }
        }
    }
}

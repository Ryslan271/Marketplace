using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            set
            {
                _allCost = value;
            }
        }
    }
}

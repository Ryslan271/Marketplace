using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace KazanNewShop.Database.Models
{
    public partial class Basket
    {
        private decimal _allCost = 0;
        [NotMapped]
        public decimal AllCost
        {
            get
            {
                if (ProductLists != null)
                {
                    _allCost += ProductLists.Select(p => p.Product).Sum(s => s.Cost);
                }

                return _allCost;
            }
            set => _allCost = value;
        }

        private int _countProduct = 0;
        [NotMapped]
        public int CountProduct
        {
            get
            {   
                if (ProductLists != null)
                    _countProduct = ProductLists.Count;

                return _countProduct;
            }
            set => _countProduct = value;
        }
    }
}

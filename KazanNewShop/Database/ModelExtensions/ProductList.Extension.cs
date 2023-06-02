using System.ComponentModel.DataAnnotations.Schema;

namespace KazanNewShop.Database.Models
{
    public partial class ProductList
    {
        private bool _IsEnableButtomPlus;
        [NotMapped]
        public bool IsEnableButtomPlus
        {
            get
            {
                return _IsEnableButtomPlus;
            }
            set
            {
                _IsEnableButtomPlus = value;
            }
        }
    }
}

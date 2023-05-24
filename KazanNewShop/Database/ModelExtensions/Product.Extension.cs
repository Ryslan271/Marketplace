using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazanNewShop.Database.Models
{
    public partial class Product
    {
        private byte[]? _mainPhoto;
        [NotMapped]
        public byte[]? MainPhoto { get; set; }
    }
}

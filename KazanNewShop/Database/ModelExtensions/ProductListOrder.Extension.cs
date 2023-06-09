﻿using System.ComponentModel.DataAnnotations.Schema;

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
                _cost = Product.Cost * Product.Count;

                return _cost;
            }
            set
            {
                _cost = value;
            }
        }
    }
}

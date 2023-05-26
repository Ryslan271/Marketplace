using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class ProductList
{
    public int Id { get; set; }

    public int IdProduct { get; set; }

    public int IdBasket { get; set; }

    public int Count { get; set; }

    public virtual Basket Basket { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}

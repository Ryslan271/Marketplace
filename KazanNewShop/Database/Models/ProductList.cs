using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class ProductList
{
    public int Id { get; set; }

    public int IdProduct { get; set; }

    public int IdBasket { get; set; }

    public int Count { get; set; }

    public virtual Basket IdBasketNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}

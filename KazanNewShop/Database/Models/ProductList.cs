using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class ProductList
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int BasketId { get; set; }

    public virtual Basket Basket { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}

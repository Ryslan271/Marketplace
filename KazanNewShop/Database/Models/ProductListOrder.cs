using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class ProductListOrder
{
    public int IdProduct { get; set; }

    public int IdOrder { get; set; }

    public int Count { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}

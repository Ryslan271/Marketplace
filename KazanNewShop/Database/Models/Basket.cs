using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Basket
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public decimal AllCost { get; set; }

    public int CountProduct { get; set; }

    public int ProductId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<ProductList> ProductLists { get; set; } = new List<ProductList>();
}

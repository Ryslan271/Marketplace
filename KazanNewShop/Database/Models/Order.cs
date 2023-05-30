using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Order
{
    public int Id { get; set; }

    public int IdPointOfIssue { get; set; }

    public int IdClient { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual PointOfIssue PointOfIssue { get; set; } = null!;

    public virtual ICollection<ProductListOrder> ProductListOrders { get; set; } = new List<ProductListOrder>();
}

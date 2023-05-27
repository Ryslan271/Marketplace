using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class PointOfIssue
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal? Lat { get; set; }

    public decimal? Lot { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

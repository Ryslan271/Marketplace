﻿using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Order
{
    public int Id { get; set; }

    public int IdPointOfIssue { get; set; }

    public int IdClient { get; set; }

    public int IdStatus { get; set; }

    public int? IdTypeReturn { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual PointOfIssue PointOfIssue { get; set; } = null!;

    public virtual OrderStatus OrderStatus { get; set; } = null!;

    public virtual TypeReturn? TypeReturn { get; set; }

    public virtual ICollection<ProductListOrder> ProductListOrders { get; set; } = new List<ProductListOrder>();
}

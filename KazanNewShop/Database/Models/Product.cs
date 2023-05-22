﻿using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Characteristics { get; set; }

    public int CatedoryId { get; set; }

    public int? Count { get; set; }

    public decimal Cost { get; set; }

    public int? Discount { get; set; }

    public int SalesmanId { get; set; }

    public bool Removed { get; set; }

    public int? StatusId { get; set; }

    public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();

    public virtual Category Catedory { get; set; } = null!;

    public virtual ICollection<PhotoProduct> PhotoProducts { get; set; } = new List<PhotoProduct>();

    public virtual ICollection<ProductList> ProductLists { get; set; } = new List<ProductList>();

    public virtual Salesman Salesman { get; set; } = null!;

    public virtual Status? Status { get; set; }
}
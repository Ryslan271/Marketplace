using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Characteristics { get; set; }

    public int IdCategory { get; set; }

    public int? Count { get; set; }

    public decimal Cost { get; set; }

    public int? Discount { get; set; }

    public int IdSalesman { get; set; }

    public bool Removed { get; set; }

    public int? IdStatus { get; set; }

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual Salesman IdSalesmanNavigation { get; set; } = null!;

    public virtual Status? IdStatusNavigation { get; set; }

    public virtual ICollection<PhotoProduct> PhotoProducts { get; set; } = new List<PhotoProduct>();

    public virtual ICollection<ProductListOrder> ProductListOrders { get; set; } = new List<ProductListOrder>();

    public virtual ICollection<ProductList> ProductLists { get; set; } = new List<ProductList>();
}

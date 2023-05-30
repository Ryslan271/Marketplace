using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Salesman
{
    public int Id { get; set; }

    public string? NameCompany { get; set; }

    public string? Description { get; set; }

    public DateTime? DateOnMarketplace { get; set; }

    public byte[]? ProfilePhoto { get; set; }

    public virtual User IdNavigation { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

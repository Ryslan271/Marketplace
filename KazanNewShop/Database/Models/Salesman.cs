using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Salesman
{
    public int Id { get; set; }

    public string? NameCompany { get; set; }

    public string? Description { get; set; }

    public DateTime? DateOnMarketplace { get; set; }

    public int? IdUser { get; set; }

    public virtual User? IdUserNavigation { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

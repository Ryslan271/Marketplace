using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Salesman
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Name { get; set; }

    public string? Patronymic { get; set; }

    public string? Description { get; set; }

    public DateTime? DateOnMarketplace { get; set; }

    public bool Removed { get; set; }

    public string? OrganizationName { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual User User { get; set; } = null!;
}

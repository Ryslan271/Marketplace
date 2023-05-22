using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Client
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Name { get; set; }

    public string? Patronymic { get; set; }

    public bool Removed { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();

    public virtual User User { get; set; } = null!;
}

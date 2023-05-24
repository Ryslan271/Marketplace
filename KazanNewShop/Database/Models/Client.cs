using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Client
{
    public int Id { get; set; }

    public string? Surname { get; set; }

    public string? Name { get; set; }

    public string? Patronymic { get; set; }

    public int? IdUser { get; set; }

    public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();

    public virtual User? IdUserNavigation { get; set; }
}

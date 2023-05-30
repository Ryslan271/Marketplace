using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Client
{
    public int Id { get; set; }

    public string? Surname { get; set; }

    public string? Name { get; set; }

    public string? Patronymic { get; set; }

    public byte[]? ProfilePhoto { get; set; }

    public string? NumberOfCreditCard { get; set; }

    public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();

    public virtual User IdNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

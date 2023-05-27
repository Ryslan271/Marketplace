using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string? Surname { get; set; }

    public string? Name { get; set; }

    public string? Patronymic { get; set; }

    public byte[]? ProfilePhoto { get; set; }

    public virtual User IdNavigation { get; set; } = null!;
}

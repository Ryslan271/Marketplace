using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Employee
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public bool? Removed { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Salesman? Salesman { get; set; }
}

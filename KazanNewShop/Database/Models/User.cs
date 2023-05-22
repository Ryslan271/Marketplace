using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Salesman> Salesmen { get; set; } = new List<Salesman>();
}

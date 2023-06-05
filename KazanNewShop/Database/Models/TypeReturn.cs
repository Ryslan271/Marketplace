﻿using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class TypeReturn
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

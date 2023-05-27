using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Basket
{
    public int Id { get; set; }

    public int IdClient { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<ProductList> ProductLists { get; set; } = new List<ProductList>();
}

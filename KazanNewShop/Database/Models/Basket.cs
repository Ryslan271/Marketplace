using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Basket
{
    public int Id { get; set; }

    public int IdClient { get; set; }

    public decimal AllCost { get; set; }

    public int CountProduct { get; set; }

    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductList> ProductLists { get; set; } = new List<ProductList>();
}

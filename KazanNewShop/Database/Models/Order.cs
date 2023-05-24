using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Order
{
    public int Id { get; set; }

    public int IdBasket { get; set; }

    public int IdAddress { get; set; }

    public virtual Address IdAddressNavigation { get; set; } = null!;

    public virtual Basket IdBasketNavigation { get; set; } = null!;
}

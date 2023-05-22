using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class Order
{
    public int Id { get; set; }

    public int BasketId { get; set; }

    public int AddressId { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual Basket Basket { get; set; } = null!;
}

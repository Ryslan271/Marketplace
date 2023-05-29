using System;
using System.Collections.Generic;

namespace KazanNewShop.Database.Models;

public partial class PhotoProduct
{
    public int Id { get; set; }

    public byte[] Photo { get; set; } = null!;

    public int IdProduct { get; set; }

    public virtual Product Product { get; set; } = null!;
}

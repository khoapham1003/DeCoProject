using System;
using System.Collections.Generic;

namespace DeCo.Model;

public partial class Product
{
    public int Id { get; set; }

    public string? DisplayName { get; set; }

    public string? DisplayType { get; set; }

    public string? Company { get; set; }

    public int? Quantity { get; set; }

    public decimal? ImportPrice { get; set; }

    public decimal? SalePrice { get; set; }

    public bool? Deleted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

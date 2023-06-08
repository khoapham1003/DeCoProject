using System;
using System.Collections.Generic;

namespace DeCo.Model;

public partial class Order
{
    public int Id { get; set; }

    public int? BillId { get; set; }

    public int? ProductId { get; set; }

    public int? Amount { get; set; }

    public decimal? TotalPrice { get; set; }

    public bool? Deleted { get; set; }

    public virtual BillDetail? Bill { get; set; }

    public virtual Product? Product { get; set; }
}

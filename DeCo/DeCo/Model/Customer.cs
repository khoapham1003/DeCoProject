using System;
using System.Collections.Generic;

namespace DeCo.Model;

public partial class Customer
{
    public int Id { get; set; }

    public string? DisplayName { get; set; }

    public DateTime? MemberDate { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? MemberRank { get; set; }

    public decimal? Spending { get; set; }

    public string? Nationality { get; set; }

    public bool? Deleted { get; set; }

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();
}

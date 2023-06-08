using System;
using System.Collections.Generic;

namespace DeCo.Model;

public partial class BillDetail
{
    public int Id { get; set; }

    public int? CusId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? BillDate { get; set; }

    public decimal? TotalMoney { get; set; }

    public bool? Deleted { get; set; }

    public virtual Customer? Cus { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

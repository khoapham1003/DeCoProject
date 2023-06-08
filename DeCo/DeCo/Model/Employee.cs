using System;
using System.Collections.Generic;

namespace DeCo.Model;

public partial class Employee
{
    public int Id { get; set; }

    public string? DisplayName { get; set; }

    public string? WorkRole { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public DateTime? ContractDate { get; set; }

    public bool? Deleted { get; set; }

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

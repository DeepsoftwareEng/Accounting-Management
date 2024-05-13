using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class Department
{
    public int IdPhongBan { get; set; }

    public string? TenPhongBan { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

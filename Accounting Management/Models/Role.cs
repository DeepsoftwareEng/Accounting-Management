using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class Role
{
    public int IdChucVu { get; set; }

    public string? TenChucVu { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

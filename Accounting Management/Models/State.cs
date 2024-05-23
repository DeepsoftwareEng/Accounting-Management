using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class State
{
    public int Id { get; set; }

    public string? TenHuyen { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

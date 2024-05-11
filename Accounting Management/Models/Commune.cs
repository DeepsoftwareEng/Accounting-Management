using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class Commune
{
    public int Id { get; set; }

    public string? TenXa { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}

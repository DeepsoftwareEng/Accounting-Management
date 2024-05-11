using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class Bank
{
    public int IdNganHang { get; set; }

    public string? TenNganHang { get; set; }

    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
}

using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class BankLog
{
    public int Id { get; set; }

    public string? NoiDung { get; set; }

    public float? SoTien { get; set; }

    public string? MaTaiKhoan { get; set; }

    public virtual BankAccount? MaTaiKhoanNavigation { get; set; }
}

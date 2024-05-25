using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class BankAccount
{
    public string MaTaiKhoan { get; set; } = null!;

    public string? SoTaiKhoan { get; set; }

    public string? LoaiTaiKhoan { get; set; }

    public int? IdNganHang { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? TenTaiKhoan { get; set; }

    public double? HienCo { get; set; }

    public double? TienNo { get; set; }

    public virtual ICollection<BankLog> BankLogs { get; set; } = new List<BankLog>();

    public virtual Bank? IdNganHangNavigation { get; set; }
}

using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class Employee
{
    public string MaNhanVien { get; set; } = null!;

    public string? TenNhanVien { get; set; }

    public DateTime? NgaySinh { get; set; }

    public int? IdChucVu { get; set; }

    public int? IdPhongBan { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Role? IdChucVuNavigation { get; set; }

    public virtual Department? IdPhongBanNavigation { get; set; }
}

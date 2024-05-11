using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class User
{
    public int MaNguoiDung { get; set; }

    public string? TenNguoiDung { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string? SoDienThoai { get; set; }

    public string? DiaChi { get; set; }

    public int? IdAccount { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }
}

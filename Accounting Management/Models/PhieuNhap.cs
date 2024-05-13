using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class PhieuNhap
{
    public string? MaPhieu { get; set; }

    public string? DonVi { get; set; }

    public string? DiaChi { get; set; }

    public string? NoiDung { get; set; }

    public DateTime? NgayLap { get; set; }

    public string? NguoiLap { get; set; }

    public string? ThuKho { get; set; }

    public string? NguoiNhan { get; set; }

    public string? GiamDoc { get; set; }

    public string? KeToanTruong { get; set; }

    public string? MaHoaDon { get; set; }

    public string? NoiNhap { get; set; }

    public string? NguoiGiao { get; set; }

    public virtual Invoice? MaHoaDonNavigation { get; set; }
}

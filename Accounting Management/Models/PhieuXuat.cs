using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class PhieuXuat
{
    public string MaPhieu { get; set; } = null!;

    public string? DonVi { get; set; }

    public string? DiaChi { get; set; }

    public string? LiDo { get; set; }

    public DateTime? NgayLap { get; set; }

    public string? NguoiLap { get; set; }

    public string? ThuKho { get; set; }

    public string? NguoiNhan { get; set; }

    public string? GiamDoc { get; set; }

    public string? KeToanTruong { get; set; }

    public string? MaHoaDon { get; set; }

    public string? NhanVienGiao { get; set; }

    public int? IsThanhToan { get; set; }

    public virtual Employee? GiamDocNavigation { get; set; }

    public virtual Employee? KeToanTruongNavigation { get; set; }

    public virtual Invoice? MaHoaDonNavigation { get; set; }

    public virtual Employee? NguoiLapNavigation { get; set; }
}

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

    public string? DiaChiCuThe { get; set; }

    public int? IdThanhPho { get; set; }

    public int? IdHuyen { get; set; }

    public int? IdXa { get; set; }

    public string? SoDienThoai { get; set; }

    public string? GioiTinh { get; set; }

    public virtual Role? IdChucVuNavigation { get; set; }

    public virtual State? IdHuyenNavigation { get; set; }

    public virtual Department? IdPhongBanNavigation { get; set; }

    public virtual City? IdThanhPhoNavigation { get; set; }

    public virtual Commune? IdXaNavigation { get; set; }

    public virtual ICollection<PhieuNhap> PhieuNhapGiamDocNavigations { get; set; } = new List<PhieuNhap>();

    public virtual ICollection<PhieuNhap> PhieuNhapKeToanTruongNavigations { get; set; } = new List<PhieuNhap>();

    public virtual ICollection<PhieuNhap> PhieuNhapNguoiLapNavigations { get; set; } = new List<PhieuNhap>();

    public virtual ICollection<PhieuXuat> PhieuXuatGiamDocNavigations { get; set; } = new List<PhieuXuat>();

    public virtual ICollection<PhieuXuat> PhieuXuatKeToanTruongNavigations { get; set; } = new List<PhieuXuat>();

    public virtual ICollection<PhieuXuat> PhieuXuatNguoiLapNavigations { get; set; } = new List<PhieuXuat>();
}

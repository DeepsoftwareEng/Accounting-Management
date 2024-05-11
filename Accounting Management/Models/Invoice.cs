using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class Invoice
{
    public string MaHoaDon { get; set; } = null!;

    public string? DonViBan { get; set; }

    public string? MaSoThueBan { get; set; }

    public string? NguoiMua { get; set; }

    public string? DonViMua { get; set; }

    public string? MaSoThueMua { get; set; }

    public DateTime? NgayLap { get; set; }

    public float? ThanhTien { get; set; }

    public string? NoiDung { get; set; }

    public string? MaKhachHang { get; set; }

    public virtual Customer? MaKhachHangNavigation { get; set; }

    public virtual ICollection<ProductInvoice> ProductInvoices { get; set; } = new List<ProductInvoice>();
}

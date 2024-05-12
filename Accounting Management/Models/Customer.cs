using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class Customer
{
    public string MaKhachHang { get; set; } = null!;

    public string? TenKhachHang { get; set; }

    public string? DonVi { get; set; }

    public string? MaSoThue { get; set; }

    public string? DiaChiCuThe { get; set; }

    public string? SoDienThoai { get; set; }

    public int? IdThanhPho { get; set; }

    public int? IdHuyen { get; set; }

    public int? IdXa { get; set; }
    public DateTime? CreateDate { get; set; }

    public virtual State? IdHuyenNavigation { get; set; }

    public virtual City? IdThanhPhoNavigation { get; set; }

    public virtual Commune? IdXaNavigation { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}

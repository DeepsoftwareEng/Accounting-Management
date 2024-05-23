﻿using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class Invoice
{
    public string MaHoaDon { get; set; } = null!;

    public DateTime? NgayLap { get; set; }

    public float? ThanhTien { get; set; }

    public string? NoiDung { get; set; }

    public string? MaKhachHang { get; set; }

    public virtual Customer? MaKhachHangNavigation { get; set; }

    public virtual ICollection<ProductInvoice> ProductInvoices { get; set; } = new List<ProductInvoice>();
}

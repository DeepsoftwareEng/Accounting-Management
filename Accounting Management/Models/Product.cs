using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class Product
{
    public string MaHangHoa { get; set; } = null!;

    public string? TenHangHoa { get; set; }

    public string? DonViTinh { get; set; }

    public int? SoLuong { get; set; }

    public float? DonGia { get; set; }

    public virtual ICollection<ProductInvoice> ProductInvoices { get; set; } = new List<ProductInvoice>();
}

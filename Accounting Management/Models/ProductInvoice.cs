using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class ProductInvoice
{
    public int Id { get; set; }

    public int? SoLuong { get; set; }

    public string? MaHangHoa { get; set; }

    public string? MaHoaDon { get; set; }

    public virtual Product? MaHangHoaNavigation { get; set; }

    public virtual Invoice? MaHoaDonNavigation { get; set; }
}

using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class Log
{
    public int Id { get; set; }

    public string? Action { get; set; }

    public DateTime? NgayThucHien { get; set; }

    public string? NoiDung { get; set; }

    public int? IdAccount { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }
}

using System;
using System.Collections.Generic;

namespace Accounting_Management.Models;

public partial class Account
{
    public int IdAccount { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? AccountType { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

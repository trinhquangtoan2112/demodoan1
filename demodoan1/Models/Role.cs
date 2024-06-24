using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class Role
{
    public int MaQuyen { get; set; }

    public string? TenQuyen { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class Banthao
{
    public int MaBanThao { get; set; }

    public string? TenBanThao { get; set; }

    public string? Noidung { get; set; }

    public DateTime? Ngaytao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

    public int MaTruyen { get; set; }

    public virtual Truyen MaTruyenNavigation { get; set; } = null!;
}

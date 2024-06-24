using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class Binhluan
{
    public int MabinhLuan { get; set; }

    public string? Noidung { get; set; }

    public DateTime? Ngaytao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

    public int MaTruyen { get; set; }

    public int MaNguoiDung { get; set; }

    public virtual User MaNguoiDungNavigation { get; set; } = null!;

    public virtual Truyen MaTruyenNavigation { get; set; } = null!;

    public virtual ICollection<Phanhoibinhluan> Phanhoibinhluans { get; set; } = new List<Phanhoibinhluan>();
}

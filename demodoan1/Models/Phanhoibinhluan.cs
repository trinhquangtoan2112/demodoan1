using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class Phanhoibinhluan
{
    public int MaPhanHoiBinhLuan { get; set; }

    public string? Noidung { get; set; }

    public DateTime? Ngaytao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

    public int MaBinhLuan { get; set; }

    public int MaNguoiDung { get; set; }

    public virtual Binhluan MaBinhLuanNavigation { get; set; } = null!;

    public virtual User MaNguoiDungNavigation { get; set; } = null!;
}

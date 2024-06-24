using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class Phanhoi
{
    public int MaPhanHoi { get; set; }

    public string? NoiDung { get; set; }

    public int? TrangThai { get; set; }

    public DateTime? Ngaytao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

    public int MaNguoiDung { get; set; }

    public string? Tieude { get; set; }

    public virtual User MaNguoiDungNavigation { get; set; } = null!;
}

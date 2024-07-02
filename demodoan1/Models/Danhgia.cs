using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class Danhgia
{
    public int MaDanhGia { get; set; }

    public string? Noidung { get; set; }

    public int? DiemDanhGia { get; set; }

    public DateTime? Ngaytao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

    public int MaTruyen { get; set; }

    public int MaNguoiDung { get; set; }

    public virtual User MaNguoiDungNavigation { get; set; } = null!;

    public virtual Truyen MaTruyenNavigation { get; set; } = null!;
}

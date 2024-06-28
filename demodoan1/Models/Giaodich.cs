using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class Giaodich
{
    public int MaGiaoDich { get; set; }

    public int MaChuongTruyen { get; set; }

    public int MaNguoiDung { get; set; }

    public int? LoaiGiaoDich { get; set; }

    public int? LoaiTien { get; set; }

    public int? SoTien { get; set; }

    public int? Trangthai { get; set; }

    public DateTime? Ngaytao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

    public virtual Chuongtruyen MaChuongTruyenNavigation { get; set; } = null!;

    public virtual User MaNguoiDungNavigation { get; set; } = null!;
}

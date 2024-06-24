using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class Baocao
{
    public int MaBaoCao { get; set; }

    public int? Loaibaocao { get; set; }

    public string? Noidung { get; set; }

    public int? MaThucThe { get; set; }

    public DateTime? Ngaytao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

    public int MaNguoiDung { get; set; }

    public string? Tieude { get; set; }

    public virtual User MaNguoiDungNavigation { get; set; } = null!;
}

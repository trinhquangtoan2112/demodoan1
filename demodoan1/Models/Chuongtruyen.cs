using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class Chuongtruyen
{
    public int MaChuong { get; set; }

    public string? TenChuong { get; set; }

    public int? TrangThai { get; set; }

    public string? NoiDung { get; set; }

    public int? HienThi { get; set; }

    public int? GiaChuong { get; set; }

    public int? LuotDoc { get; set; }

    public DateTime? Ngaytao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

    public int MaTruyen { get; set; }

    public virtual ICollection<Giaodich> Giaodiches { get; set; } = new List<Giaodich>();

    public virtual Truyen MaTruyenNavigation { get; set; } = null!;
}

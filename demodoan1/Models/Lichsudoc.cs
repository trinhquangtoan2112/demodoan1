using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class Lichsudoc
{
    public int MaChuongTruyen { get; set; }

    public int MaNguoiDung { get; set; }

    public int? TrangthaiXoa { get; set; }

    public ulong? TrangthaiDaDoc { get; set; }

    public string? Audio { get; set; }

    public DateTime? Ngaytao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

    public int? ViTri { get; set; }

    public virtual Chuongtruyen MaChuongTruyenNavigation { get; set; } = null!;

    public virtual User MaNguoiDungNavigation { get; set; } = null!;
}

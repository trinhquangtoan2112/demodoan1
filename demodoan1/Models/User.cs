using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class User
{
    public int MaNguoiDung { get; set; }

    public string? TenNguoiDung { get; set; }

    public string MatKhau { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    public string? AnhDaiDien { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public int? SoDeCu { get; set; }

    public int? SoXu { get; set; }

    public int? SoChiaKhoa { get; set; }

    public bool? Vip { get; set; }

    public DateTime? NgayHetHanVip { get; set; }

    public DateTime? Ngaytao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

    public int MaQuyen { get; set; }

    public virtual ICollection<Baocao> Baocaos { get; set; } = new List<Baocao>();

    public virtual ICollection<Binhluan> Binhluans { get; set; } = new List<Binhluan>();

    public virtual ICollection<Butdanh> Butdanhs { get; set; } = new List<Butdanh>();

    public virtual ICollection<Danhdau> Danhdaus { get; set; } = new List<Danhdau>();

    public virtual ICollection<Danhgium> Danhgia { get; set; } = new List<Danhgium>();

    public virtual ICollection<Giaodich> Giaodiches { get; set; } = new List<Giaodich>();

    public virtual ICollection<Lichsudoc> Lichsudocs { get; set; } = new List<Lichsudoc>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual Role MaQuyenNavigation { get; set; } = null!;

    public virtual ICollection<Phanhoibinhluan> Phanhoibinhluans { get; set; } = new List<Phanhoibinhluan>();

    public virtual ICollection<Phanhoi> Phanhois { get; set; } = new List<Phanhoi>();
}

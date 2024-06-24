using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class Truyen
{
    public int MaTruyen { get; set; }

    public string? TenTruyen { get; set; }

    public string? MoTa { get; set; }

    public string? AnhBia { get; set; }

    public int? CongBo { get; set; }

    public int? TrangThai { get; set; }

    public DateTime? Ngaytao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

    public int MaButDanh { get; set; }

    public int MaTheLoai { get; set; }

    public virtual ICollection<Banthao> Banthaos { get; set; } = new List<Banthao>();

    public virtual ICollection<Binhluan> Binhluans { get; set; } = new List<Binhluan>();

    public virtual ICollection<Chuongtruyen> Chuongtruyens { get; set; } = new List<Chuongtruyen>();

    public virtual ICollection<Danhdau> Danhdaus { get; set; } = new List<Danhdau>();

    public virtual ICollection<Danhgium> Danhgia { get; set; } = new List<Danhgium>();

    public virtual ICollection<Lichsudoc> Lichsudocs { get; set; } = new List<Lichsudoc>();

    public virtual Butdanh MaButDanhNavigation { get; set; } = null!;

    public virtual Theloai MaTheLoaiNavigation { get; set; } = null!;
}

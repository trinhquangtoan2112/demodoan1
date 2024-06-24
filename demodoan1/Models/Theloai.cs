using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class Theloai
{
    public int MaTheLoai { get; set; }

    public string? TenTheLoai { get; set; }

    public string? MoTa { get; set; }

    public int? Trangthai { get; set; }

    public DateTime? Ngaytao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

    public virtual ICollection<Truyen> Truyens { get; set; } = new List<Truyen>();
}

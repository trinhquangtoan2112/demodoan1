using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;

namespace demodoan1.Models;

public partial class Butdanh
{
    public int MaButDanh { get; set; }

    public string? TenButDanh { get; set; }

    public DateTime? Ngaytao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

    public int MaNguoiDung { get; set; }

    public int? Trangthai { get; set; }

    public virtual User MaNguoiDungNavigation { get; set; } = null!;

    public virtual ICollection<Truyen> Truyens { get; set; } = new List<Truyen>();
}


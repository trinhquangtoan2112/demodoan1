using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class Like
{
    public int MaLike { get; set; }

    public int? LoaiThucTheLike { get; set; }

    public int? MaThucThe { get; set; }

    public DateTime? Ngaytao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

    public int MaNguoiDung { get; set; }

    public virtual User MaNguoiDungNavigation { get; set; } = null!;
}

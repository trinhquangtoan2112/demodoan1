using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class Danhdau
{
    public int MaTruyen { get; set; }

    public int MaNguoiDung { get; set; }

    public DateTime? Ngaytao { get; set; }

    public virtual User MaNguoiDungNavigation { get; set; } = null!;

    public virtual Truyen MaTruyenNavigation { get; set; } = null!;
}

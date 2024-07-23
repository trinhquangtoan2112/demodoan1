namespace demodoan1.Models.BinhluanDto
{
    public class BinhluanDto
    {
        public int MabinhLuan { get; set; }

        public string? Noidung { get; set; }

        public int MaTruyen { get; set; }

        public int MaNguoiDung { get; set; }
    }

    public class DSBinhLuanDto
    {
        public int MaBinhLuan { get; set; }
        public string? Noidung { get; set; }
        public DateTime? NgayCapNhap { get; set; }
        public string? TenNguoiDung { get; set; }
        public string? AnhDaiDien { get; set; }
        public int? Solike { get; set; }
        public bool? CheckCuaToi { get; set; }
        public List<DSPhanHoiDto>? DsPhBinhLuan { get; set; }

        public class DSPhanHoiDto
        {
            public int MaPhanHoiBinhLuan { get; set; }
            public string? Noidung { get; set; }
            public int MaBinhLuan { get; set; }
            public DateTime? NgayCapNhap { get; set; }
            public string? TenNguoiDung { get; set; }
            public string? AnhDaiDien { get; set; }
            public bool? CheckCuaToi { get; set; }
            public int? Solike { get; set; }
        }
    }

    public class SuaBinhLuanDto
    {
        public int MaBinhLuan { get; set; }
        public string? Noidung { get; set; }
    }

    public class SuaPhanHoiBinhLuanDto
    {
        public int MaPhanHoiBinhLuan { get; set; }
        public string? Noidung { get; set; }
    }
}

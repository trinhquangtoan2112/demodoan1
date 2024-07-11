namespace demodoan1.Models.ButdanhDto
{
    public class ButdanhDto
    {
        public string? TenButDanh { get; set; }
    }
    public class ButdanhDtoKhoa
    {
        public int MaButDanh { get; set; }
    }
    public class SuaButdanhDto
    {
        public int MaButDanh { get; set; }
        public string? TenButDanh { get; set; }
    }

    public class LayButDanhAdminDto
    {
        public int MaButDanh { get; set; }

        public string? TenButDanh { get; set; }
        public string? EmailNguoiDung { get; set; }

        public DateTime? Ngaytao { get; set; }

        public int MaNguoiDung { get; set; }

        public int? Trangthai { get; set; }
        public int? SoLuongTruyen { get; set; }
    }
}

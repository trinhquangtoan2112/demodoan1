namespace demodoan1.Models.ChuongtruyenDto
{
    public class ChuongtruyenDto
    {
        public string? TenChuong { get; set; }
        public string? NoiDung { get; set; }
        public int? GiaChuong { get; set; }
        public int MaTruyen { get; set; }
    }

    public class ChuongtruyenCapNhapDto
    {
        public string? TenChuong { get; set; }
        public string? NoiDung { get; set; }
        public int? GiaChuong { get; set; }
        public int? HienThi { get; set; }
        public int? TrangThai { get; set; }
        public float? Stt { get; set; }
    }
}

namespace demodoan1.Models.TruyenDto
{
    public class CapNhapTruyenDto
    {
        public string? TenTruyen { get; set; }
        public string? MoTa { get; set; }
        public IFormFile? AnhBia { get; set; }
        public int MaTheLoai { get; set; }
        public int? TrangThai { get; set; }

    }
}

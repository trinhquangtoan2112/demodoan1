namespace demodoan1.Models.TruyenDto
{
    public class TruyenDto
    {

        public string? TenTruyen { get; set; }
        public string? MoTa { get; set; }
        public IFormFile? AnhBia { get; set; }
        public int MaButDanh { get; set; }

        public int MaTheLoai { get; set; }
    }
}

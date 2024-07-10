namespace demodoan1.Models.UserDto
{
    public class AdduserDto
    {
       public int maNguoiDung {get; set;}
        public string? TenNguoiDung { get; set; }

        public DateTime? NgaySinh { get; set; }

        public string? GioiTinh { get; set; }

        public IFormFile? AnhDaiDien { get; set; }

    }
}

namespace demodoan1.Models.DanhgiaDto
{
    public class DanhgiaDto
    {
        public int MaDanhGia { get; set; }

        public string? Noidung { get; set; }

        public int? DiemDanhGia { get; set; }

        public int MaTruyen { get; set; }

        public int MaNguoiDung { get; set; }
    }

    public class SuadanhgiaDto
    {
        public int MaDanhGia { get; set; }
        public string? Noidung { get; set; }

        public int? DiemDanhGia { get; set; }
    }

    public class LaydsDanhGiaDto
    {
        public int MaDanhGia { get; set; }

        public string? Noidung { get; set; }

        public int? DiemDanhGia { get; set; }

        public DateTime? Ngaycapnhat { get; set; }

        public string? TenNguoiDung { get; set; }
        public string? AnhDaiDien { get; set; }

        public bool? CheckCuaToi { get; set; }
    }

}

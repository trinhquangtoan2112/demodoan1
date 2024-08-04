namespace demodoan1.Models.GiaodichDto
{
    public class GiaodichDto
    {

        public int MaChuongTruyen { get; set; }

        public int MaNguoiDung { get; set; }

        public int? LoaiGiaoDich { get; set; }

        public int? LoaiTien { get; set; }

        public int? SoTien { get; set; }

        public int? Trangthai { get; set; }
    }

    public class layLSGD
    {

        public int MaGiaoDich { get; set; }

        public DateTime? Ngaytao { get; set; }

        public string? NoiDung { get; set; }

        public int? LoaiGiaoDich { get; set; }
    }

    public class ErrorViewModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
    public class SuccessViewModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string OrderInfo { get; set; }
    }
}

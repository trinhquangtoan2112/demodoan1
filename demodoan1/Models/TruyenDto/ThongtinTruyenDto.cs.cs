namespace demodoan1.Models.TruyenDto
{
    public class ThongtinTruyenDto
    {
        public int MaTruyen { get; set; }

        public string? TenTruyen { get; set; }

        public string? MoTa { get; set; }

        public string? AnhBia { get; set; }

        public int? CongBo { get; set; }

        public int? TrangThai { get; set; }

        public DateTime? Ngaytao { get; set; }

        public DateTime? NgayCapNhap { get; set; }

        public int MaButDanh { get; set; }

        public int MaTheLoai { get; set; }

        public virtual Butdanh MaButDanhNavigation { get; set; } = null!;

        public virtual Theloai MaTheLoaiNavigation { get; set; } = null!;
    }
}

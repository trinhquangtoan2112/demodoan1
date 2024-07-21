namespace demodoan1.Models.BaocaoDto
{
    public class BaocaoDto
    {
        public string? Tieude { get; set; }

        public int? Loaibaocao { get; set; }

        public string? Noidung { get; set; }

        public int? MaThucThe { get; set; }

        public int MaNguoiDung { get; set; }
        
    }

    public class LayDSBaocao
    {
        
        public int? MaBaoCao { get; set; }
        public int? Loaibaocao { get; set; }
        public int? Trangthai { get; set; }
        public string? Tieude { get; set; }
        public string? Noidung { get; set; }

        public string? NoiDungBibaocao { get; set; }
        public string? NguoiBaoCao { get; set; }
        public DateTime? Ngaytao { get; set; }
    }

    public class SuaBaoBaocao
    {
        public int? MaBaoCao { get; set; }
        public int? Trangthai { get; set; }
    }


}

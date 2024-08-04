namespace demodoan1.Models.ChuongtruyenDto
{
    public class ChuongtruyenDto
    {
        public string? TenChuong { get; set; }
        public int GiaChuong { get; set; }

        public string? NoiDung { get; set; }
       
        public int MaTruyen { get; set; }
    }

    public class ChuongtruyenCapNhapDto
    {
        public int GiaChuong { get; set; }
        public string? TenChuong { get; set; }
        public string? NoiDung { get; set; }
       
      
       
    }
    public class ChuongtruyenNoiDung
    {
       
        public string? NoiDung { get; set; }



    }
}

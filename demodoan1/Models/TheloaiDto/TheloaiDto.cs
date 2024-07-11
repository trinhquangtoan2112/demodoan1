namespace demodoan1.Models.TheloaiDto
{
    public class TheloaiDto
    {
        public int MaTheLoai { get; set; }
        public string? TenTheLoai { get; set; }
        public string? MoTa { get; set; }
    }

    public class GetTheloaiDto
    {
        public int MaTheLoai { get; set; }

        public string? TenTheLoai { get; set; }

        public string? MoTa { get; set; }
        public string? Soluongtruyen { get; set; }

    }



}

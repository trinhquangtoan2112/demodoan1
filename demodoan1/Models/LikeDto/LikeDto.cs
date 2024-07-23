namespace demodoan1.Models.LikeDto
{
    public class LikeDto
    {

        public int LoaiThucTheLike { get; set; }

        public int MaThucThe { get; set; }

        public int MaNguoiDung { get; set; }
    }

    public class LikeCheckRequest
    {
        public int LoaiThucTheLike { get; set; }
        public List<int> MaThucThes { get; set; }

    }
}

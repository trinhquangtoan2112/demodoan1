﻿namespace demodoan1.Models.PhanhoiDto
{
    public class PhanhoiDto
    {
        public int MaPhanHoi { get; set; }

        public string? NoiDung { get; set; }

        public int? TrangThai { get; set; }

        public int MaNguoiDung { get; set; }

        public string? Tieude { get; set; }

        public string? EmailNguoiDung { get; set; } // Thêm trường này

        public DateTime? Ngaytao { get; set; }
    }
}

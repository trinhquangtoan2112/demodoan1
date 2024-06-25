using System;
using System.Collections.Generic;

namespace demodoan1.Models;

public partial class User
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int MaNguoiDung { get; set; }

        public string? TenNguoiDung { get; set; }
        [Required]
        [MaxLength(30)]
        public string MatKhau { get; set; }
        [Required]
        [MaxLength(40)]
        public string Email { get; set; }

    public string MatKhau { get; set; } = null!;

    public string Email { get; set; } = null!;

        public DateTime? NgaySinh { get; set; }

        [MaxLength(10)]
        public string? GioiTinh { get; set; }

        [MaxLength(255)]
        public string? AnhDaiDien { get; set; }

        public bool? TrangThai { get; set; }

        public bool? DaXoa { get; set; }

        public int? SoDeCu { get; set; }

        public int? SoXu { get; set; }

        public int? SoChiaKhoa { get; set; }

        public bool? Vip { get; set; }

        public DateTime? NgayHetHanVip { get; set; }

    public DateTime? Ngaytao { get; set; }

    public DateTime? NgayCapNhap { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime NgayCapNhap { get; set; }
        [ForeignKey("Role")]
        public int MaQuyen { get; set; }

    public virtual ICollection<Baocao> Baocaos { get; set; } = new List<Baocao>();

    public virtual ICollection<Binhluan> Binhluans { get; set; } = new List<Binhluan>();

    public virtual ICollection<Butdanh> Butdanhs { get; set; } = new List<Butdanh>();

    public virtual ICollection<Danhdau> Danhdaus { get; set; } = new List<Danhdau>();

    public virtual ICollection<Danhgium> Danhgia { get; set; } = new List<Danhgium>();

    public virtual ICollection<Giaodich> Giaodiches { get; set; } = new List<Giaodich>();

    public virtual ICollection<Lichsudoc> Lichsudocs { get; set; } = new List<Lichsudoc>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual Role MaQuyenNavigation { get; set; } = null!;

    public virtual ICollection<Phanhoibinhluan> Phanhoibinhluans { get; set; } = new List<Phanhoibinhluan>();
       
    public virtual ICollection<Phanhoi> Phanhois { get; set; } = new List<Phanhoi>();
}

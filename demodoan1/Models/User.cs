using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demodoan1.Models
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

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime NgayTao { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime NgayCapNhap { get; set; }
        [ForeignKey("Role")]
        public int MaQuyen { get; set; }
        public Role Role { get; set; }
       
    }
}

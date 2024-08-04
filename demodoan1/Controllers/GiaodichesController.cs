using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.GiaodichDto;
using demodoan1.Helpers;
using demodoan1.Data;
using demodoan1.Models.BaocaoDto;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiaodichesController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public GiaodichesController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Giaodiches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<layLSGD>>> GetGiaodiches(string token)
        {
            token = token.Trim();
            var data = token.Substring(7); // Bỏ qua phần "Bearer "
            Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
            string iDNguoiDung = claimsData["IdUserName"];

            int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

            try
            {
                var dsgiaodich = _context.Giaodiches
                    .Where(g => g.MaNguoiDung == maNguoiDung && g.Trangthai == 1)
                    .Select(d => new layLSGD
                    {
                        MaGiaoDich = d.MaGiaoDich,
                        Ngaytao = d.Ngaytao,
                        LoaiGiaoDich = d.LoaiGiaoDich,
                        NoiDung = d.LoaiGiaoDich == 1
                            ? "Bạn đã mua chương " +
                              _context.Chuongtruyens
                                  .Where(ct => ct.MaChuong == d.MaChuongTruyen)
                                  .Select(ct => ct.TenChuong)
                                  .FirstOrDefault() +
                              " của truyện " +
                              _context.Chuongtruyens
                                  .Where(ct => ct.MaChuong == d.MaChuongTruyen)
                                  .Select(ct => ct.MaTruyenNavigation.TenTruyen)
                                  .FirstOrDefault() +
                              " với giá " +
                              (d.LoaiTien.HasValue
                                  ? (d.LoaiTien.Value == 1 ? $"{d.SoTien} Xu" :
                                     d.LoaiTien.Value == 2 ? $"{d.SoTien} Chìa khóa" :
                                     $"{d.SoTien} Đơn vị không xác định")
                                  : "Loại tiền không xác định")
                            : d.LoaiGiaoDich == 2
                            ? $"Bạn đã nạp {d.SoTien} nghìn VND để nhận được {d.SoTien / 100} Xu"
                            : d.LoaiGiaoDich == 3
                            ? $"Bạn đã dùng {d.SoTien} Xu để nâng cấp tài khoản Vip 30 ngày"
                            : d.LoaiGiaoDich == 4
                            ? "Bạn đã đề cử truyện " +
                              _context.Truyens
                                  .Where(t => t.MaTruyen == _context.Chuongtruyens
                                      .Where(ct => ct.MaChuong == d.MaChuongTruyen)
                                      .Select(ct => ct.MaTruyen)
                                      .FirstOrDefault())
                                  .Select(t => t.TenTruyen)
                                  .FirstOrDefault() +
                              $" với {d.SoTien} phiếu đề cử"
                            : d.LoaiGiaoDich == 5
                            ? "Bạn đã tăng " +
                              d.SoTien +
                              " Xu cho truyện " +
                              _context.Chuongtruyens
                                  .Where(ct => ct.MaChuong == d.MaChuongTruyen)
                                  .Select(ct => ct.MaTruyenNavigation.TenTruyen)
                                  .FirstOrDefault() +
                              " của người đăng là " +
                              _context.Chuongtruyens
                                  .Where(ct => ct.MaChuong == d.MaChuongTruyen)
                                  .Select(ct => ct.MaTruyenNavigation.MaButDanhNavigation.TenButDanh)
                                  .FirstOrDefault()
                            : d.LoaiGiaoDich == 6
                            ? d.LoaiTien == 5
                                ? "Bạn đã nhận được 1 chìa khóa và 2 phiếu đề cử sau khi điểm danh"
                                : d.LoaiTien == 6
                                ? "Bạn đã nhận được 1 chìa khóa và 1 phiếu đề cử sau khi điểm danh"
                                : "Loại tiền không xác định cho điểm danh"
                            : "Loại giao dịch không xác định"
                    });

                return Ok(new { status = StatusCodes.Status200OK, message = "Thành công", data = dsgiaodich });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }


        // GET: api/Giaodiches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Giaodich>> GetGiaodich(int id)
        {
            var giaodich =  _context.Giaodiches.Where(item => item.MaNguoiDung == id).ToListAsync();

            if (giaodich == null)
            {
                return NotFound();
            }

            return Ok();
        }



        [HttpPost]
        public async Task<ActionResult<Giaodich>> PostGiaodich(GiaodichDto giaodichDto, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Remove "Bearer " prefix
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                var detailNguoiDung = await _context.Users.FirstOrDefaultAsync(item => item.MaNguoiDung == maNguoiDung);
                if (detailNguoiDung == null)
                {
                    return NotFound("Người dùng không tồn tại.");
                }

                // Kiểm tra số dư
                if ((detailNguoiDung.SoXu < giaodichDto.SoTien && giaodichDto.LoaiTien == 1) ||
                    (detailNguoiDung.SoChiaKhoa < 1 && giaodichDto.LoaiTien == 2) ||
                    (detailNguoiDung.SoDeCu < 1 && giaodichDto.LoaiTien == 4))
                {
                    return BadRequest("Không đủ số dư.");
                }

                // Kiểm tra điểm danh
                if (giaodichDto.LoaiGiaoDich == 5)
                {
                    var now = DateTime.UtcNow;
                    var lastAttendance = await _context.Giaodiches
                        .Where(g => g.MaNguoiDung == maNguoiDung && g.LoaiGiaoDich == 5)
                        .OrderByDescending(g => g.Ngaytao)
                        .FirstOrDefaultAsync();

                    if (lastAttendance != null)
                    {
                        var timeSinceLastAttendance = now.Subtract(lastAttendance.Ngaytao.Value);
                        if (timeSinceLastAttendance.TotalHours < 24)
                        {
                            var hoursRemaining = 24 - timeSinceLastAttendance.TotalHours;
                            return Unauthorized(new { status = StatusCodes.Status401Unauthorized, message = ($"Bạn cần đợi thêm {hoursRemaining:0} giờ để điểm danh lần nữa.") });
                        }
                    }

                    // Cập nhật số dư dựa trên loại giao dịch
                    if (detailNguoiDung.Vip == true)
                    {
                        detailNguoiDung.SoChiaKhoa += 1;
                        detailNguoiDung.SoDeCu += 2;
                    }
                    else
                    {
                        detailNguoiDung.SoChiaKhoa += 1;
                        detailNguoiDung.SoDeCu += 1;
                    }
                }
                else
                {
                    // Cập nhật số dư dựa trên loại giao dịch
                    if (giaodichDto.LoaiTien == 1 && giaodichDto.LoaiGiaoDich == 1 || giaodichDto.LoaiTien == 1 && giaodichDto.LoaiGiaoDich == 5)
                    {
                        detailNguoiDung.SoXu -= giaodichDto.SoTien;
                    }
                    if (giaodichDto.LoaiTien == 2 && giaodichDto.LoaiGiaoDich == 1)
                    {
                        detailNguoiDung.SoChiaKhoa -= 1;
                    }
                    if (giaodichDto.LoaiTien == 4 && giaodichDto.LoaiGiaoDich == 4)
                    {
                        detailNguoiDung.SoDeCu -= 1;
                    }
                }

                var giaodich = new Giaodich
                {
                    MaChuongTruyen = giaodichDto.MaChuongTruyen,
                    MaNguoiDung = maNguoiDung,
                    LoaiGiaoDich = giaodichDto.LoaiGiaoDich,
                    LoaiTien = giaodichDto.LoaiTien,
                    SoTien = giaodichDto.SoTien,
                    Trangthai = 1,
                    Ngaytao = DateTime.UtcNow
                };

                _context.Giaodiches.Add(giaodich);
                _context.Users.Update(detailNguoiDung);
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status201Created, data = "Thêm thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }


        // DELETE: api/Giaodiches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGiaodich(int id)
        {
            var giaodich = await _context.Giaodiches.FindAsync(id);
            if (giaodich == null)
            {
                return NotFound();
            }

            _context.Giaodiches.Remove(giaodich);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GiaodichExists(int id)
        {
            return _context.Giaodiches.Any(e => e.MaGiaoDich == id);
        }
    }
}

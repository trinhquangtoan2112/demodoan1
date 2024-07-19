using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.BinhluanDto;
using demodoan1.Models.DanhgiaDto;
using demodoan1.Helpers;
using NuGet.Common;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BinhluansController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public BinhluansController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Binhluans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DSBinhLuanDto>>> GetBinhluans(int MaTruyen)
        {
            try
            {
                var danhSachBinhLuan = await _context.Binhluans
                    .Where(d => d.MaTruyen == MaTruyen)
                    .Select(d => new DSBinhLuanDto
                    {
                        MaBinhLuan = d.MabinhLuan,
                        Noidung = d.Noidung,
                        NgayCapNhap = d.NgayCapNhap,
                        TenNguoiDung = d.MaNguoiDungNavigation.TenNguoiDung,
                        AnhDaiDien = d.MaNguoiDungNavigation.AnhDaiDien,
                        CheckCuaToi = false, // Add logic here if needed to determine if it's the user's comment
                        DsPhBinhLuan = _context.Phanhoibinhluans
                            .Where(t => t.MaBinhLuan == d.MabinhLuan)
                            .Select(p => new DSBinhLuanDto.DSPhanHoiDto
                            {
                                MaPhanHoiBinhLuan = p.MaPhanHoiBinhLuan,
                                Noidung = p.Noidung,
                                MaBinhLuan = p.MaBinhLuan,
                                NgayCapNhap = p.NgayCapNhap,
                                TenNguoiDung = p.MaNguoiDungNavigation.TenNguoiDung,
                                AnhDaiDien = p.MaNguoiDungNavigation.AnhDaiDien,
                                CheckCuaToi = false // Add logic here if needed to determine if it's the user's reply
                            }).ToList()
                    })
                    .ToListAsync();

                return Ok(new { status = StatusCodes.Status200OK, data = danhSachBinhLuan });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }

        // GET: api/Binhluans
        [HttpGet("GetBinhluansNguoiDung")]
        public async Task<ActionResult<IEnumerable<DSBinhLuanDto>>> GetBinhluansNguoiDung(int MaTruyen, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                var danhSachBinhLuan = await _context.Binhluans
                    .Where(d => d.MaTruyen == MaTruyen)
                    .Select(d => new DSBinhLuanDto
                    {
                        MaBinhLuan = d.MabinhLuan,
                        Noidung = d.Noidung,
                        NgayCapNhap = d.NgayCapNhap,
                        TenNguoiDung = d.MaNguoiDungNavigation.TenNguoiDung,
                        AnhDaiDien = d.MaNguoiDungNavigation.AnhDaiDien,
                        CheckCuaToi = d.MaNguoiDung == maNguoiDung, // Add logic here if needed to determine if it's the user's comment
                        DsPhBinhLuan = _context.Phanhoibinhluans
                            .Where(t => t.MaBinhLuan == d.MabinhLuan)
                            .Select(p => new DSBinhLuanDto.DSPhanHoiDto
                            {
                                MaPhanHoiBinhLuan = p.MaPhanHoiBinhLuan,
                                Noidung = p.Noidung,
                                MaBinhLuan = p.MaBinhLuan,
                                NgayCapNhap = p.NgayCapNhap,
                                TenNguoiDung = p.MaNguoiDungNavigation.TenNguoiDung,
                                AnhDaiDien = p.MaNguoiDungNavigation.AnhDaiDien,
                                CheckCuaToi = p.MaNguoiDung == maNguoiDung // Add logic here if needed to determine if it's the user's reply
                            }).ToList()
                    })
                    .ToListAsync();

                return Ok(new { status = StatusCodes.Status200OK, data = danhSachBinhLuan });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }

        // DELETE: api/Binhluans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBinhLuan(int id, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];

                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                // Tìm bình luận dựa trên id
                var binhluan = await _context.Binhluans.FindAsync(id);
                if (binhluan == null)
                {
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Bình luận không tồn tại." });
                }

                // Lấy maQuyen của người dùng từ cơ sở dữ liệu
                var user = await _context.Users.FirstOrDefaultAsync(u => u.MaNguoiDung == maNguoiDung);
                if (user == null)
                {
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Người dùng không tồn tại." });
                }
                int maQuyen = user.MaQuyen;

                // Kiểm tra quyền xóa bình luận
                if (binhluan.MaNguoiDung != maNguoiDung && maQuyen == 2)
                {
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Bạn không có quyền xóa bình luận này." });
                }

                if (binhluan.MaNguoiDung == maNguoiDung || maQuyen == 1)
                {
                    // Xóa các phản hồi bình luận liên quan
                    var phanHoiBinhLuans = _context.Phanhoibinhluans.Where(p => p.MaBinhLuan == id);
                    _context.Phanhoibinhluans.RemoveRange(phanHoiBinhLuans);

                    // Xóa bình luận
                    _context.Binhluans.Remove(binhluan);
                    await _context.SaveChangesAsync();

                    return Ok(new { status = StatusCodes.Status201Created });
                }

                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Không thể xóa bình luận." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }

        // POST: api/Binhluans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Binhluan>> PostBinhluan(BinhluanDto binhluanDto,string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];

                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                var binhluan = new Binhluan
                {
                    Noidung = binhluanDto.Noidung,
                    MaTruyen = binhluanDto.MaTruyen,
                    MaNguoiDung = maNguoiDung
                };

                _context.Binhluans.Add(binhluan);
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status201Created});
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Lỗi xảy ra." });
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutBinhLuan(SuaBinhLuanDto suaBinhLuanDto, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                // Tìm đánh giá dựa trên id
                var binhluan = await _context.Binhluans.FindAsync(suaBinhLuanDto.MaBinhLuan);
                if (binhluan == null)
                {
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Bình luận không tồn tại." });
                }

                // Kiểm tra nếu đánh giá này được tạo bởi người dùng có mã người dùng trong token
                if (binhluan.MaNguoiDung != maNguoiDung)
                {
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Bạn không có quyền sửa bình luận này." });
                }

                // Cập nhật đánh giá
                binhluan.Noidung = suaBinhLuanDto.Noidung;

                _context.Entry(binhluan).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }

        private bool BinhluanExists(int id)
        {
            return _context.Binhluans.Any(e => e.MabinhLuan == id);
        }
    }
}

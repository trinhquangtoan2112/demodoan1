using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.DanhgiaDto;
using demodoan1.Helpers;
using demodoan1.Models.ButdanhDto;
using demodoan1.Models.DanhdauDto;
using NuGet.Common;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhgiasController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public DanhgiasController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // Lay danh gia khi da dang nhap
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LaydsDanhGiaDto>>> GetDanhgia(int MaTruyen, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                var danhSachDanhGia = await _context.Danhgia
                    .Where(d => d.MaTruyen == MaTruyen)
                    .Select(d => new LaydsDanhGiaDto
                    {
                        MaDanhGia = d.MaDanhGia,
                        Noidung = d.Noidung,
                        DiemDanhGia = d.DiemDanhGia,
                        Ngaycapnhat = d.NgayCapNhap,
                        TenNguoiDung = d.MaNguoiDungNavigation.TenNguoiDung,
                        AnhDaiDien = d.MaNguoiDungNavigation.AnhDaiDien,
                        CheckCuaToi = d.MaNguoiDung == maNguoiDung,
                    })
                    .ToListAsync();

                return Ok(new { status = StatusCodes.Status200OK, data = danhSachDanhGia });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }

        // Lay danh gia khi chưadangnhap
        [HttpGet("DsDanhGiaChuaDangNhap")]
        public async Task<ActionResult<IEnumerable<LaydsDanhGiaDto>>> LayDanhGia(int MaTruyen)
        {
            try
            {

                var danhSachDanhGia = await _context.Danhgia
                    .Where(d => d.MaTruyen == MaTruyen)
                    .Select(d => new LaydsDanhGiaDto
                    {
                        MaDanhGia = d.MaDanhGia,
                        Noidung = d.Noidung,
                        DiemDanhGia = d.DiemDanhGia,
                        Ngaycapnhat = d.NgayCapNhap,
                        TenNguoiDung = d.MaNguoiDungNavigation.TenNguoiDung,
                        AnhDaiDien = d.MaNguoiDungNavigation.AnhDaiDien,
                        CheckCuaToi = false,
                    })
                    .ToListAsync();

                return Ok(new { status = StatusCodes.Status200OK, data = danhSachDanhGia });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }


        // PUT: api/Danhgias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutDanhgia(SuadanhgiaDto danhgiaDto, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                // Tìm đánh giá dựa trên id
                var danhgia = await _context.Danhgia.FindAsync(danhgiaDto.MaDanhGia);
                if (danhgia == null)
                {
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Đánh giá không tồn tại." });
                }

                // Kiểm tra nếu đánh giá này được tạo bởi người dùng có mã người dùng trong token
                if (danhgia.MaNguoiDung != maNguoiDung)
                {
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Bạn không có quyền sửa đánh giá này." });
                }

                // Cập nhật đánh giá
                danhgia.Noidung = danhgiaDto.Noidung;
                danhgia.DiemDanhGia = danhgiaDto.DiemDanhGia;

                _context.Entry(danhgia).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status200OK, data = danhgiaDto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }

        // POST: api/Danhgias
        [HttpPost]
        public async Task<ActionResult<Danhgia>> PostDanhgia(DanhgiaDto danhgiaDto, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];

                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                // Kiểm tra xem người dùng đã đánh giá truyện này chưa
                bool alreadyRated = await _context.Danhgia.AnyAsync(d => d.MaNguoiDung == maNguoiDung && d.MaTruyen == danhgiaDto.MaTruyen);

                if (alreadyRated)
                {
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Bạn đã đánh giá truyện này rồi." });
                }

                var danhgia = new Danhgia
                {
                    Noidung = danhgiaDto.Noidung,
                    DiemDanhGia = danhgiaDto.DiemDanhGia,
                    MaTruyen = danhgiaDto.MaTruyen,
                    MaNguoiDung = maNguoiDung
                };

                _context.Danhgia.Add(danhgia);
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status201Created, data = danhgiaDto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Lỗi xảy ra." });
            }
        }

        // DELETE: api/Danhgias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDanhgia(int id, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];

                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                // Tìm đánh giá dựa trên id
                var danhgia = await _context.Danhgia.FindAsync(id);
                if (danhgia == null)
                {
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Đánh giá không tồn tại." });
                }

                // Lấy maQuyen của người dùng từ cơ sở dữ liệu
                var user = await _context.Users.FirstOrDefaultAsync(u => u.MaNguoiDung == maNguoiDung);
                if (user == null)
                {
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Người dùng không tồn tại." });
                }
                int maQuyen = user.MaQuyen;


                // Kiểm tra nếu đánh giá này được tạo bởi người dùng có mã người dùng trong token
                if (danhgia.MaNguoiDung != maNguoiDung && maQuyen == 2)
                {
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Bạn không có quyền xóa đánh giá này." });
                }

                if (danhgia.MaNguoiDung == maNguoiDung || maQuyen == 1)
                {
                    _context.Danhgia.Remove(danhgia);
                    await _context.SaveChangesAsync();
                }

                

                return Ok(new { status = StatusCodes.Status201Created });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpGet("CheckDanhgia")]
        public async Task<ActionResult<bool>> CheckDanhgia(int MaTruyen, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];

                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                var existingDanhgia = await _context.Danhgia
                    .AnyAsync(d => d.MaNguoiDung == maNguoiDung && d.MaTruyen == MaTruyen);

                return Ok(new { status = StatusCodes.Status200OK, data = existingDanhgia });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }


        private bool DanhgiaExists(int id)
        {
            return _context.Danhgia.Any(e => e.MaDanhGia == id);
        }
    }
}

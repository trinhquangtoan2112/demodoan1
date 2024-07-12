using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.DanhdauDto;
using demodoan1.Helpers;
using NuGet.Common;
using MySqlX.XDevAPI.Common;
using demodoan1.Models.ButdanhDto;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhdausController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public DanhdausController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Danhdaus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DanhdauDto>>> GetDanhdaus(string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];

                // Lấy danh sách các Danhdau của người dùng
                var dsDanhDau = _context.Danhdaus.Where(item => item.MaNguoiDung == Int64.Parse(iDNguoiDung)).ToList();

                if (dsDanhDau == null || !dsDanhDau.Any())
                {
                    return NotFound();
                }

                // Tạo danh sách kết quả với thông tin truyện, bút danh và thể loại
                var result = dsDanhDau.Select(d => new DanhdauDto
                {
                    MaTruyen = d.MaTruyen,
                    Ngaytao = d.Ngaytao,
                    TenTruyen = _context.Truyens.FirstOrDefault(t => t.MaTruyen == d.MaTruyen)?.TenTruyen,
                    MoTa = _context.Truyens.FirstOrDefault(t => t.MaTruyen == d.MaTruyen)?.MoTa,
                    AnhBia = _context.Truyens.FirstOrDefault(t => t.MaTruyen == d.MaTruyen)?.AnhBia,
                    TenButDanh = _context.Truyens
                        .Include(t => t.MaButDanhNavigation)
                        .FirstOrDefault(t => t.MaTruyen == d.MaTruyen)?.MaButDanhNavigation.TenButDanh,
                    TenTheLoai = _context.Truyens
                        .Include(t => t.MaTheLoaiNavigation)
                        .FirstOrDefault(t => t.MaTruyen == d.MaTruyen)?.MaTheLoaiNavigation.TenTheLoai
                }).ToList();

                return Ok(new { status = StatusCodes.Status200OK, data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }


    [HttpGet("CheckDanhdau")]
        public async Task<ActionResult<bool>> CheckDanhdau(int maTruyen, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];

                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                var existingDanhdau = await _context.Danhdaus
                    .AnyAsync(d => d.MaTruyen == maTruyen && d.MaNguoiDung == maNguoiDung);

                return Ok(new { status = StatusCodes.Status200OK, data = existingDanhdau });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }


        // POST: api/Danhdaus
        [HttpPost]
        public async Task<ActionResult<Danhdau>> PostDanhdau(DanhdauDto danhdauDto, string token)
        {
            token = token.Trim();
            var data = token.Substring(7);
            Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
            string iDNguoiDung = claimsData["IdUserName"];

            int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

            var existingDanhdau = await _context.Danhdaus
                .FirstOrDefaultAsync(d => d.MaTruyen == danhdauDto.MaTruyen && d.MaNguoiDung == maNguoiDung);

            if (existingDanhdau != null)
            {
                return Conflict(new { message = "Truyện đã được đánh dấu." });
            }

            var danhdau = new Danhdau
            {
                MaTruyen = danhdauDto.MaTruyen,
                MaNguoiDung = maNguoiDung,
            };

            _context.Danhdaus.Add(danhdau);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi lưu dữ liệu.");
            }

            return Ok(new { status = StatusCodes.Status201Created });
        }

        [HttpDelete("XoaDanhDauTruyen")]
        public async Task<IActionResult> XoaDanhDauTruyen([FromBody] DanhdauDto danhdauDto,string token)
        {
            token = token.Trim();
            var data = token.Substring(7);
            Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
            string iDNguoiDung = claimsData["IdUserName"];

            int maNguoiDung = (int)Int64.Parse(iDNguoiDung);
            var danhDau = await _context.Danhdaus
                .FirstOrDefaultAsync(d => d.MaTruyen == danhdauDto.MaTruyen && d.MaNguoiDung == maNguoiDung);

            if (danhDau == null)
            {
                return NotFound();
            }

            _context.Danhdaus.Remove(danhDau);
            await _context.SaveChangesAsync();

            return Accepted(new { status = StatusCodes.Status202Accepted, message = "Thành công" });
        }

        private bool DanhdauExists(int id)
        {
            return _context.Danhdaus.Any(e => e.MaTruyen == id);
        }
    }
}

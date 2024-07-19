using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.PhanhoibinhluanDto;
using demodoan1.Helpers;
using demodoan1.Models.BinhluanDto;
using NuGet.Common;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhanhoibinhluansController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public PhanhoibinhluansController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Phanhoibinhluans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Phanhoibinhluan>>> GetPhanhoibinhluans()
        {
            return await _context.Phanhoibinhluans.ToListAsync();
        }

        // GET: api/Phanhoibinhluans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Phanhoibinhluan>> GetPhanhoibinhluan(int id)
        {
            var phanhoibinhluan = await _context.Phanhoibinhluans.FindAsync(id);

            if (phanhoibinhluan == null)
            {
                return NotFound();
            }

            return phanhoibinhluan;
        }

        // PUT: api/Phanhoibinhluans/5

        [HttpPut]
        public async Task<IActionResult> PutPhanHoiBinhLuan(SuaPhanHoiBinhLuanDto phanHoiBinhLuanDto, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                // Tìm đánh giá dựa trên id
                var binhluan = await _context.Phanhoibinhluans.FindAsync(phanHoiBinhLuanDto.MaPhanHoiBinhLuan);
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
                binhluan.Noidung = phanHoiBinhLuanDto.Noidung;

                _context.Entry(binhluan).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }

        // POST: api/Phanhoibinhluans
        [HttpPost]
        public async Task<ActionResult<Phanhoibinhluan>> PostPhanhoibinhluan(PhanhoibinhluanDto phanhoibinhluanDto, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];

                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                var phanhoibinhluan = new Phanhoibinhluan
                {
                    Noidung = phanhoibinhluanDto.Noidung,
                    MaBinhLuan = phanhoibinhluanDto.MaBinhLuan,
                    MaNguoiDung = maNguoiDung
                };

                _context.Phanhoibinhluans.Add(phanhoibinhluan);
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status201Created });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Lỗi xảy ra." });
            }
        }
        // DELETE: api/Danhgias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhanHoiBinhLuan(int id, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];

                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                // Tìm đánh giá dựa trên id
                var phanhoibinhluan = await _context.Phanhoibinhluans.FindAsync(id);
                if (phanhoibinhluan == null)
                {
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Bình luân không tồn tại." });
                }

                // Lấy maQuyen của người dùng từ cơ sở dữ liệu
                var user = await _context.Users.FirstOrDefaultAsync(u => u.MaNguoiDung == maNguoiDung);
                if (user == null)
                {
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Người dùng không tồn tại." });
                }
                int maQuyen = user.MaQuyen;
                // Kiểm tra nếu đánh giá này được tạo bởi người dùng có mã người dùng trong token
                if (phanhoibinhluan.MaNguoiDung != maNguoiDung && maQuyen == 2)
                {
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Bạn không có quyền xóa bình luận này." });
                }

                if (phanhoibinhluan.MaNguoiDung == maNguoiDung || maQuyen == 1)
                {
                    _context.Phanhoibinhluans.Remove(phanhoibinhluan);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { status = StatusCodes.Status201Created });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }

        private bool PhanhoibinhluanExists(int id)
        {
            return _context.Phanhoibinhluans.Any(e => e.MaPhanHoiBinhLuan == id);
        }
    }
}

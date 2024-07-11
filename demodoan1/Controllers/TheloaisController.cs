using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.TheloaiDto;
using demodoan1.Data;
using demodoan1.Helpers;
using NuGet.Common;
using demodoan1.Models.ButdanhDto;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheloaisController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public TheloaisController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Theloais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTheloaiDto>>> GetTheloais(string token)
        {
            var data = token.Trim().Substring(7);
            var claimsData = TokenClass.DecodeToken(data);
            var iDNguoiDung = claimsData["IdUserName"];

            var user = await _context.Users.FirstOrDefaultAsync(u => u.MaNguoiDung == Int64.Parse(iDNguoiDung));
            if (user == null || user.MaQuyen != 1)
            {
                return Unauthorized(new { status = StatusCodes.Status401Unauthorized, message = "Không có quyền" });
            }

            var theloaiList = await _context.Theloais
                .Select(tl => new GetTheloaiDto
                {
                    MaTheLoai = tl.MaTheLoai,
                    TenTheLoai = tl.TenTheLoai,
                    MoTa = tl.MoTa,
                    Soluongtruyen = _context.Truyens.Count(tr => tr.MaTheLoai == tl.MaTheLoai).ToString()
                })
                .ToListAsync();

            return Ok(new { status = StatusCodes.Status200OK, data = theloaiList });
        }


        // GET: api/Theloais/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Theloai>> GetTheloai(int id)
        {
            var theloai = await _context.Theloais.FindAsync(id);

            if (theloai == null)
            {
                return NotFound();
            }

            return theloai;
        }

        // PUT: api/Theloais/5
        [HttpPut]
        public async Task<IActionResult> PutTheloai(TheloaiDto theloaiDto, [FromQuery] string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7);
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                var user = await _context.Users.FirstOrDefaultAsync(u => u.MaNguoiDung == Int64.Parse(iDNguoiDung));
                if (user == null || user.MaQuyen != 1)
                {
                    return Unauthorized(new { status = StatusCodes.Status401Unauthorized, message = "Không có quyền" });
                }

                var theloai = await _context.Theloais.FindAsync(theloaiDto.MaTheLoai);
                if (theloai == null)
                {
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Thể loại không tồn tại" });
                }

                // Cập nhật các thuộc tính của thể loại bằng dữ liệu từ theloaiDto
                theloai.TenTheLoai = theloaiDto.TenTheLoai;
                theloai.MoTa = theloaiDto.MoTa;

                _context.Entry(theloai).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status200OK, data = theloaiDto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Lỗi khi cập nhật thể loại.", Exception = ex.Message });
            }
        }


        [HttpPost]
        public async Task<ActionResult<Theloai>> PostButdanh([FromBody] TheloaiDto theloaiDto, [FromQuery] string token)
        {
            try
            {
                Boolean daCo = _context.Theloais.Any(item => item.TenTheLoai.Equals(theloaiDto.TenTheLoai));
                if (daCo)
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Đã tồn tại" });
                token = token.Trim();
                var data = token.Substring(7);
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                var user = await _context.Users.FirstOrDefaultAsync(u => u.MaNguoiDung == Int64.Parse(iDNguoiDung));
                if (user == null || user.MaQuyen != 1)
                {
                    return Unauthorized(new { status = StatusCodes.Status401Unauthorized, message = "Không có quyền" });
                }
                var tl = new Theloai
                {
                    TenTheLoai = theloaiDto.TenTheLoai,
                    MoTa = theloaiDto.MoTa,
                };

                _context.Theloais.Add(tl);
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status201Created, data = theloaiDto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "lỗi" });
            }

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheloai(int id, [FromQuery] string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7);
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                var user = await _context.Users.FirstOrDefaultAsync(u => u.MaNguoiDung == Int64.Parse(iDNguoiDung));
                if (user == null || user.MaQuyen != 1)
                {
                    return Unauthorized(new { status = StatusCodes.Status401Unauthorized, message = "Không có quyền" });
                }

                var theloai = await _context.Theloais.FindAsync(id);
                if (theloai == null)
                {
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Thể loại không tồn tại" });
                }

                _context.Theloais.Remove(theloai);
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status204NoContent, message = "Xóa thể loại thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Lỗi khi xóa thể loại.", Exception = ex.Message });
            }
        }


        private bool TheloaiExists(int id)
        {
            return _context.Theloais.Any(e => e.MaTheLoai == id);
        }
    }
}

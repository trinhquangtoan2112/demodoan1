using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.PhanhoiDto;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhanhoisController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public PhanhoisController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Phanhois
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhanhoiDto>>> GetPhanhois()
        {
            var phanhois = await _context.Phanhois
                .Select(p => new PhanhoiDto
                {
                    MaPhanHoi = p.MaPhanHoi,
                    Tieude = p.Tieude,
                    NoiDung = p.NoiDung,
                    TrangThai = p.TrangThai,
                    Ngaytao = p.Ngaytao,
                    EmailNguoiDung = _context.Users
                        .Where(n => n.MaNguoiDung == p.MaNguoiDung)
                        .Select(n => n.Email)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return phanhois;
        }

        // GET: api/Phanhois/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhanhoiDto>> GetPhanhoi(int id)
        {
            var phanhoi = await _context.Phanhois
                .Where(p => p.MaPhanHoi == id)
                .Select(p => new PhanhoiDto
                {
                    MaPhanHoi = p.MaPhanHoi,
                    Tieude = p.Tieude,
                    NoiDung = p.NoiDung,
                    TrangThai = p.TrangThai,
                    Ngaytao = p.Ngaytao,
                    EmailNguoiDung = _context.Users
                        .Where(n => n.MaNguoiDung == p.MaNguoiDung)
                        .Select(n => n.Email)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (phanhoi == null)
            {
                return NotFound();
            }

            return phanhoi;
        }

        // PUT: api/Phanhois/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhanhoi(int id, [FromBody] PhanhoiDto phanhoiDto)
        {
            if (id != phanhoiDto.MaPhanHoi)
            {
                return BadRequest();
            }

            try
            {
                var phanhoi = await _context.Phanhois.FindAsync(id);

                if (phanhoi == null)
                {
                    return NotFound();
                }

                phanhoi.TrangThai = phanhoiDto.TrangThai;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi xảy ra khi cập nhật phản hồi: {ex.Message}");
            }
        }


        // POST: api/Phanhois
        [HttpPost]
        public async Task<ActionResult<Phanhoi>> PostPhanhoi(PhanhoiDto phanhoiDto)
        {
            var phanhoi = new Phanhoi
            {
                NoiDung = phanhoiDto.NoiDung,
                TrangThai = phanhoiDto.TrangThai,
                MaNguoiDung = phanhoiDto.MaNguoiDung,
                Tieude = phanhoiDto.Tieude
            };

            _context.Phanhois.Add(phanhoi);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhanhoi", new { id = phanhoi.MaPhanHoi }, phanhoi);
        }

        // DELETE: api/Phanhois/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhanhoi(int id)
        {
            var phanhoi = await _context.Phanhois.FindAsync(id);
            if (phanhoi == null)
            {
                return NotFound();
            }

            _context.Phanhois.Remove(phanhoi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhanhoiExists(int id)
        {
            return _context.Phanhois.Any(e => e.MaPhanHoi == id);
        }
    }
}

using demodoan1.Models;
using demodoan1.Models.TruyenDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruyensController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public TruyensController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Truyens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Truyen>>> GetTruyens()
        {
            return await _context.Truyens.ToListAsync();
        }

        // GET: api/Truyens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Truyen>> GetTruyen(int id)
        {
            var truyen = await _context.Truyens.FindAsync(id);

            if (truyen == null)
            {
                return NotFound();
            }

            return truyen;
        }

        // PUT: api/Truyens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTruyen(int id, TruyenDto truyenDto)
        {
            if (id != truyenDto.MaTruyen)
            {
                return BadRequest();
            }

            var truyen = new Truyen
            {
                MaTruyen = truyenDto.MaTruyen,
                TenTruyen = truyenDto.TenTruyen,
                MoTa = truyenDto.MoTa,
                AnhBia = truyenDto.AnhBia,
                CongBo = truyenDto.CongBo,
                TrangThai = truyenDto.TrangThai,
                MaButDanh = truyenDto.MaButDanh,
                MaTheLoai = truyenDto.MaTheLoai
            };

            _context.Entry(truyen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TruyenExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Truyens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Truyen>> PostTruyen(TruyenDto truyenDto)
        {
            var truyen = new Truyen
            {
                TenTruyen = truyenDto.TenTruyen,
                MoTa = truyenDto.MoTa,
                AnhBia = truyenDto.AnhBia,
                CongBo = truyenDto.CongBo,
                TrangThai = truyenDto.TrangThai,
                MaButDanh = truyenDto.MaButDanh,
                MaTheLoai = truyenDto.MaTheLoai
            };

            _context.Truyens.Add(truyen);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTruyen", new { id = truyen.MaTruyen }, truyen);
        }

        // DELETE: api/Truyens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTruyen(int id)
        {
            var truyen = await _context.Truyens.FindAsync(id);
            if (truyen == null)
            {
                return NotFound();
            }

            _context.Truyens.Remove(truyen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TruyenExists(int id)
        {
            return _context.Truyens.Any(e => e.MaTruyen == id);
        }
    }
}

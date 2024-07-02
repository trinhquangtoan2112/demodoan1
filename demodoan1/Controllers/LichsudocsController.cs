using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.LichsudocDto;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LichsudocsController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public LichsudocsController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Lichsudocs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lichsudoc>>> GetLichsudocs()
        {
            return await _context.Lichsudocs.ToListAsync();
        }

        // GET: api/Lichsudocs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lichsudoc>> GetLichsudoc(int id)
        {
            var lichsudoc = await _context.Lichsudocs.FindAsync(id);

            if (lichsudoc == null)
            {
                return NotFound();
            }

            return lichsudoc;
        }

        // PUT: api/Lichsudocs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLichsudoc(int id, LichsudocDto lichsudocDto)
        {
            if (id != lichsudocDto.MaTruyen)
            {
                return BadRequest();
            }

            var lichsudoc = new Lichsudoc
            {
                MaTruyen = lichsudocDto.MaTruyen,
                MaNguoiDung = lichsudocDto.MaNguoiDung,
                TrangthaiXoa = lichsudocDto.TrangthaiXoa,
                TrangthaiDaDoc = lichsudocDto.TrangthaiDaDoc,
                Audio = lichsudocDto.Audio
            };

            _context.Entry(lichsudoc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LichsudocExists(id))
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

        // POST: api/Lichsudocs
        [HttpPost]
        public async Task<ActionResult<Lichsudoc>> PostLichsudoc(LichsudocDto lichsudocDto)
        {
            var lichsudoc = new Lichsudoc
            {
                MaTruyen = lichsudocDto.MaTruyen,
                MaNguoiDung = lichsudocDto.MaNguoiDung,
                TrangthaiXoa = lichsudocDto.TrangthaiXoa,
                TrangthaiDaDoc = lichsudocDto.TrangthaiDaDoc,
                Audio = lichsudocDto.Audio
            };

            _context.Lichsudocs.Add(lichsudoc);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LichsudocExists(lichsudoc.MaTruyen))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLichsudoc", new { id = lichsudoc.MaTruyen }, lichsudoc);
        }

        // DELETE: api/Lichsudocs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLichsudoc(int id)
        {
            var lichsudoc = await _context.Lichsudocs.FindAsync(id);
            if (lichsudoc == null)
            {
                return NotFound();
            }

            _context.Lichsudocs.Remove(lichsudoc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LichsudocExists(int id)
        {
            return _context.Lichsudocs.Any(e => e.MaTruyen == id);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.DanhdauDto;

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
        public async Task<ActionResult<IEnumerable<Danhdau>>> GetDanhdaus()
        {
            return await _context.Danhdaus.ToListAsync();
        }

        // GET: api/Danhdaus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Danhdau>> GetDanhdau(int id)
        {
            var danhdau = await _context.Danhdaus.FindAsync(id);

            if (danhdau == null)
            {
                return NotFound();
            }

            return danhdau;
        }

        // PUT: api/Danhdaus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDanhdau(int id, DanhdauDto danhdauDto)
        {
            if (id != danhdauDto.MaTruyen)
            {
                return BadRequest();
            }

            var danhdau = new Danhdau
            {
                MaTruyen = danhdauDto.MaTruyen,
                MaNguoiDung = danhdauDto.MaNguoiDung
            };

            _context.Entry(danhdau).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DanhdauExists(id))
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

        // POST: api/Danhdaus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Danhdau>> PostDanhdau(DanhdauDto danhdauDto)
        {
            var danhdau = new Danhdau
            {
                MaTruyen = danhdauDto.MaTruyen,
                MaNguoiDung = danhdauDto.MaNguoiDung
            };

            _context.Danhdaus.Add(danhdau);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DanhdauExists(danhdau.MaTruyen))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDanhdau", new { id = danhdau.MaTruyen }, danhdau);
        }

        // DELETE: api/Danhdaus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDanhdau(int id)
        {
            var danhdau = await _context.Danhdaus.FindAsync(id);
            if (danhdau == null)
            {
                return NotFound();
            }

            _context.Danhdaus.Remove(danhdau);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DanhdauExists(int id)
        {
            return _context.Danhdaus.Any(e => e.MaTruyen == id);
        }
    }
}

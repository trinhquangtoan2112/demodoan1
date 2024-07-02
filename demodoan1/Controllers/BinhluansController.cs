using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.BinhluanDto;

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
        public async Task<ActionResult<IEnumerable<Binhluan>>> GetBinhluans()
        {
            return await _context.Binhluans.ToListAsync();
        }

        // GET: api/Binhluans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Binhluan>> GetBinhluan(int id)
        {
            var binhluan = await _context.Binhluans.FindAsync(id);

            if (binhluan == null)
            {
                return NotFound();
            }

            return binhluan;
        }

        // PUT: api/Binhluans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBinhluan(int id, BinhluanDto binhluanDto)
        {
            if (id != binhluanDto.MabinhLuan)
            {
                return BadRequest();
            }

            var binhluan = new Binhluan
            {
                MabinhLuan = binhluanDto.MabinhLuan,
                Noidung = binhluanDto.Noidung,
                MaTruyen = binhluanDto.MaTruyen,
                MaNguoiDung = binhluanDto.MaNguoiDung
            };

            _context.Entry(binhluan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BinhluanExists(id))
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

        // POST: api/Binhluans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Binhluan>> PostBinhluan(BinhluanDto binhluanDto)
        {
            var binhluan = new Binhluan
            {
                Noidung = binhluanDto.Noidung,
                MaTruyen = binhluanDto.MaTruyen,
                MaNguoiDung = binhluanDto.MaNguoiDung
            };

            _context.Binhluans.Add(binhluan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBinhluan", new { id = binhluan.MabinhLuan }, binhluan);
        }

        // DELETE: api/Binhluans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBinhluan(int id)
        {
            var binhluan = await _context.Binhluans.FindAsync(id);
            if (binhluan == null)
            {
                return NotFound();
            }

            _context.Binhluans.Remove(binhluan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BinhluanExists(int id)
        {
            return _context.Binhluans.Any(e => e.MabinhLuan == id);
        }
    }
}

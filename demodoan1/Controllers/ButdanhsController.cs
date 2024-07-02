using demodoan1.Models;
using demodoan1.Models.ButdanhDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ButdanhsController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public ButdanhsController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Butdanhs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Butdanh>>> GetButdanhs()
        {
            return await _context.Butdanhs.ToListAsync();
        }

        // GET: api/Butdanhs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Butdanh>> GetButdanh(int id)
        {
            var butdanh = await _context.Butdanhs.FindAsync(id);

            if (butdanh == null)
            {
                return NotFound();
            }

            return butdanh;
        }

        // PUT: api/Butdanhs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutButdanh(int id, ButdanhDto butdanhDto)
        {
            if (id != butdanhDto.MaButDanh)
            {
                return BadRequest();
            }

            var butdanh = new Butdanh
            {
                MaButDanh = butdanhDto.MaButDanh,
                TenButDanh = butdanhDto.TenButDanh,
                MaNguoiDung = butdanhDto.MaNguoiDung,
                Trangthai = butdanhDto.Trangthai
            };

            _context.Entry(butdanh).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ButdanhExists(id))
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

        // POST: api/Butdanhs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Butdanh>> PostButdanh(ButdanhDto butdanhDto)
        {
            var butdanh = new Butdanh
            {
                MaButDanh = butdanhDto.MaButDanh,
                TenButDanh = butdanhDto.TenButDanh,
                MaNguoiDung = butdanhDto.MaNguoiDung,
                Trangthai = butdanhDto.Trangthai
            };

            _context.Butdanhs.Add(butdanh);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetButdanh", new { id = butdanh.MaButDanh }, butdanh);
        }

        // DELETE: api/Butdanhs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteButdanh(int id)
        {
            var butdanh = await _context.Butdanhs.FindAsync(id);
            if (butdanh == null)
            {
                return NotFound();
            }

            _context.Butdanhs.Remove(butdanh);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ButdanhExists(int id)
        {
            return _context.Butdanhs.Any(e => e.MaButDanh == id);
        }
    }
}

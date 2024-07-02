using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.PhanhoibinhluanDto;

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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhanhoibinhluan(int id, PhanhoibinhluanDto phanhoibinhluanDto)
        {
            if (id != phanhoibinhluanDto.MaPhanHoiBinhLuan)
            {
                return BadRequest();
            }

            var phanhoibinhluan = new Phanhoibinhluan
            {
                MaPhanHoiBinhLuan = phanhoibinhluanDto.MaPhanHoiBinhLuan,
                Noidung = phanhoibinhluanDto.Noidung,
                MaBinhLuan = phanhoibinhluanDto.MaBinhLuan,
                MaNguoiDung = phanhoibinhluanDto.MaNguoiDung
            };

            _context.Entry(phanhoibinhluan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhanhoibinhluanExists(id))
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

        // POST: api/Phanhoibinhluans
        [HttpPost]
        public async Task<ActionResult<Phanhoibinhluan>> PostPhanhoibinhluan(PhanhoibinhluanDto phanhoibinhluanDto)
        {
            var phanhoibinhluan = new Phanhoibinhluan
            {
                MaPhanHoiBinhLuan = phanhoibinhluanDto.MaPhanHoiBinhLuan,
                Noidung = phanhoibinhluanDto.Noidung,
                MaBinhLuan = phanhoibinhluanDto.MaBinhLuan,
                MaNguoiDung = phanhoibinhluanDto.MaNguoiDung
            };

            _context.Phanhoibinhluans.Add(phanhoibinhluan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhanhoibinhluan", new { id = phanhoibinhluan.MaPhanHoiBinhLuan }, phanhoibinhluan);
        }

        // DELETE: api/Phanhoibinhluans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhanhoibinhluan(int id)
        {
            var phanhoibinhluan = await _context.Phanhoibinhluans.FindAsync(id);
            if (phanhoibinhluan == null)
            {
                return NotFound();
            }

            _context.Phanhoibinhluans.Remove(phanhoibinhluan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhanhoibinhluanExists(int id)
        {
            return _context.Phanhoibinhluans.Any(e => e.MaPhanHoiBinhLuan == id);
        }
    }
}

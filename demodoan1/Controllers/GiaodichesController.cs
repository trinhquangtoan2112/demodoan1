using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.GiaodichDto;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiaodichesController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public GiaodichesController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Giaodiches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Giaodich>>> GetGiaodiches()
        {
            return await _context.Giaodiches.ToListAsync();
        }

        // GET: api/Giaodiches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Giaodich>> GetGiaodich(int id)
        {
            var giaodich = await _context.Giaodiches.FindAsync(id);

            if (giaodich == null)
            {
                return NotFound();
            }

            return giaodich;
        }

        // PUT: api/Giaodiches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGiaodich(int id, GiaodichDto giaodichDto)
        {
            if (id != giaodichDto.MaGiaoDich)
            {
                return BadRequest();
            }

            var giaodich = new Giaodich
            {
                MaGiaoDich = giaodichDto.MaGiaoDich,
                MaChuongTruyen = giaodichDto.MaChuongTruyen,
                MaNguoiDung = giaodichDto.MaNguoiDung,
                LoaiGiaoDich = giaodichDto.LoaiGiaoDich,
                LoaiTien = giaodichDto.LoaiTien,
                SoTien = giaodichDto.SoTien,
                Trangthai = giaodichDto.Trangthai
            };

            _context.Entry(giaodich).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GiaodichExists(id))
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

        // POST: api/Giaodiches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Giaodich>> PostGiaodich(GiaodichDto giaodichDto)
        {
            var giaodich = new Giaodich
            {
                MaGiaoDich = giaodichDto.MaGiaoDich,
                MaChuongTruyen = giaodichDto.MaChuongTruyen,
                MaNguoiDung = giaodichDto.MaNguoiDung,
                LoaiGiaoDich = giaodichDto.LoaiGiaoDich,
                LoaiTien = giaodichDto.LoaiTien,
                SoTien = giaodichDto.SoTien,
                Trangthai = giaodichDto.Trangthai
            };

            _context.Giaodiches.Add(giaodich);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGiaodich", new { id = giaodich.MaGiaoDich }, giaodich);
        }

        // DELETE: api/Giaodiches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGiaodich(int id)
        {
            var giaodich = await _context.Giaodiches.FindAsync(id);
            if (giaodich == null)
            {
                return NotFound();
            }

            _context.Giaodiches.Remove(giaodich);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GiaodichExists(int id)
        {
            return _context.Giaodiches.Any(e => e.MaGiaoDich == id);
        }
    }
}

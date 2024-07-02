using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.DanhgiaDto;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhgiasController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public DanhgiasController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Danhgias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Danhgia>>> GetDanhgia()
        {
            return await _context.Danhgia.ToListAsync();
        }

        // GET: api/Danhgias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Danhgia>> GetDanhgia(int id)
        {
            var danhgia = await _context.Danhgia.FindAsync(id);

            if (danhgia == null)
            {
                return NotFound();
            }

            return danhgia;
        }

        // PUT: api/Danhgias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDanhgia(int id, DanhgiaDto danhgiaDto)
        {
            if (id != danhgiaDto.MaDanhGia)
            {
                return BadRequest();
            }

            var danhgia = new Danhgia
            {
                MaDanhGia = danhgiaDto.MaDanhGia,
                Noidung = danhgiaDto.Noidung,
                DiemDanhGia = danhgiaDto.DiemDanhGia,
                MaTruyen = danhgiaDto.MaTruyen,
                MaNguoiDung = danhgiaDto.MaNguoiDung
            };

            _context.Entry(danhgia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DanhgiaExists(id))
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

        // POST: api/Danhgias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Danhgia>> PostDanhgia(DanhgiaDto danhgiaDto)
        {
            var danhgia = new Danhgia
            {
                MaDanhGia = danhgiaDto.MaDanhGia,
                Noidung = danhgiaDto.Noidung,
                DiemDanhGia = danhgiaDto.DiemDanhGia,
                MaTruyen = danhgiaDto.MaTruyen,
                MaNguoiDung = danhgiaDto.MaNguoiDung
            };

            _context.Danhgia.Add(danhgia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDanhgia", new { id = danhgia.MaDanhGia }, danhgia);
        }

        // DELETE: api/Danhgias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDanhgia(int id)
        {
            var danhgia = await _context.Danhgia.FindAsync(id);
            if (danhgia == null)
            {
                return NotFound();
            }

            _context.Danhgia.Remove(danhgia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DanhgiaExists(int id)
        {
            return _context.Danhgia.Any(e => e.MaDanhGia == id);
        }
    }
}

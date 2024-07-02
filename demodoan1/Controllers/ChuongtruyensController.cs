using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.ChuongtruyenDto;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChuongtruyensController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public ChuongtruyensController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Chuongtruyens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chuongtruyen>>> GetChuongtruyens()
        {
            return await _context.Chuongtruyens.ToListAsync();
        }

        // GET: api/Chuongtruyens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Chuongtruyen>> GetChuongtruyen(int id)
        {
            var chuongtruyen = await _context.Chuongtruyens.FindAsync(id);

            if (chuongtruyen == null)
            {
                return NotFound();
            }

            return chuongtruyen;
        }

        // PUT: api/Chuongtruyens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChuongtruyen(int id, ChuongtruyenDto chuongtruyenDto)
        {
            if (id != chuongtruyenDto.MaChuong)
            {
                return BadRequest();
            }

            var chuongtruyen = new Chuongtruyen
            {
                MaChuong = chuongtruyenDto.MaChuong,
                TenChuong = chuongtruyenDto.TenChuong,
                TrangThai = chuongtruyenDto.TrangThai,
                NoiDung = chuongtruyenDto.NoiDung,
                HienThi = chuongtruyenDto.HienThi,
                GiaChuong = chuongtruyenDto.GiaChuong,
                LuotDoc = chuongtruyenDto.LuotDoc,
                MaTruyen = chuongtruyenDto.MaTruyen
            };

            _context.Entry(chuongtruyen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChuongtruyenExists(id))
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

        // POST: api/Chuongtruyens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Chuongtruyen>> PostChuongtruyen(ChuongtruyenDto chuongtruyenDto)
        {
            var chuongtruyen = new Chuongtruyen
            {
                TenChuong = chuongtruyenDto.TenChuong,
                TrangThai = chuongtruyenDto.TrangThai,
                NoiDung = chuongtruyenDto.NoiDung,
                HienThi = chuongtruyenDto.HienThi,
                GiaChuong = chuongtruyenDto.GiaChuong,
                LuotDoc = chuongtruyenDto.LuotDoc,
                MaTruyen = chuongtruyenDto.MaTruyen
            };

            _context.Chuongtruyens.Add(chuongtruyen);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChuongtruyen", new { id = chuongtruyen.MaChuong }, chuongtruyen);
        }

        // DELETE: api/Chuongtruyens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChuongtruyen(int id)
        {
            var chuongtruyen = await _context.Chuongtruyens.FindAsync(id);
            if (chuongtruyen == null)
            {
                return NotFound();
            }

            _context.Chuongtruyens.Remove(chuongtruyen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChuongtruyenExists(int id)
        {
            return _context.Chuongtruyens.Any(e => e.MaChuong == id);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.BaocaoDto;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaocaosController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public BaocaosController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Baocaos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Baocao>>> GetBaocaos()
        {
            return await _context.Baocaos.ToListAsync();
        }

        // GET: api/Baocaos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Baocao>> GetBaocao(int id)
        {
            var baocao = await _context.Baocaos.FindAsync(id);

            if (baocao == null)
            {
                return NotFound();
            }

            return baocao;
        }

        // PUT: api/Baocaos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBaocao(int id, BaocaoDto baocaoDto)
        {
            if (id != baocaoDto.MaBaoCao)
            {
                return BadRequest();
            }

            var baocao = new Baocao
            {
                MaBaoCao = baocaoDto.MaBaoCao,
                Loaibaocao = baocaoDto.Loaibaocao,
                Noidung = baocaoDto.Noidung,
                MaThucThe = baocaoDto.MaThucThe,
                MaNguoiDung = baocaoDto.MaNguoiDung,
                Tieude = baocaoDto.Tieude
            };

            _context.Entry(baocao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BaocaoExists(id))
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

        // POST: api/Baocaos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Baocao>> PostBaocao(BaocaoDto baocaoDto)
        {
            var baocao = new Baocao
            {
                Loaibaocao = baocaoDto.Loaibaocao,
                Noidung = baocaoDto.Noidung,
                MaThucThe = baocaoDto.MaThucThe,
                MaNguoiDung = baocaoDto.MaNguoiDung,
                Tieude = baocaoDto.Tieude
            };

            _context.Baocaos.Add(baocao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBaocao", new { id = baocao.MaBaoCao }, baocao);
        }

        // DELETE: api/Baocaos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBaocao(int id)
        {
            var baocao = await _context.Baocaos.FindAsync(id);
            if (baocao == null)
            {
                return NotFound();
            }

            _context.Baocaos.Remove(baocao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BaocaoExists(int id)
        {
            return _context.Baocaos.Any(e => e.MaBaoCao == id);
        }
    }
}

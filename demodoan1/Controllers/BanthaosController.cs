using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.BanthaoDto;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanthaosController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public BanthaosController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Banthaos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Banthao>>> GetBanthaos()
        {
            return await _context.Banthaos.ToListAsync();
        }

        // GET: api/Banthaos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Banthao>> GetBanthao(int id)
        {
            var banthao = await _context.Banthaos.FindAsync(id);

            if (banthao == null)
            {
                return NotFound();
            }

            return banthao;
        }

        // PUT: api/Banthaos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBanthao(int id, BanthaoDto banthaoDto)
        {
            if (id != banthaoDto.MaBanThao)
            {
                return BadRequest();
            }

            var banthao = new Banthao
            {
                MaBanThao = banthaoDto.MaBanThao,
                TenBanThao = banthaoDto.TenBanThao,
                Noidung = banthaoDto.Noidung,
                MaTruyen = banthaoDto.MaTruyen
            };

            _context.Entry(banthao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BanthaoExists(id))
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

        // POST: api/Banthaos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Banthao>> PostBanthao(BanthaoDto banthaoDto)
        {
            var banthao = new Banthao
            {
                TenBanThao = banthaoDto.TenBanThao,
                Noidung = banthaoDto.Noidung,
                MaTruyen = banthaoDto.MaTruyen
            };

            _context.Banthaos.Add(banthao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBanthao", new { id = banthao.MaBanThao }, banthao);
        }

        // DELETE: api/Banthaos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanthao(int id)
        {
            var banthao = await _context.Banthaos.FindAsync(id);
            if (banthao == null)
            {
                return NotFound();
            }

            _context.Banthaos.Remove(banthao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BanthaoExists(int id)
        {
            return _context.Banthaos.Any(e => e.MaBanThao == id);
        }
    }
}

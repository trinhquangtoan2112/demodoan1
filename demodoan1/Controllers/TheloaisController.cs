using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheloaisController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public TheloaisController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Theloais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Theloai>>> GetTheloais()
        {
            return await _context.Theloais.ToListAsync();
        }

        // GET: api/Theloais/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Theloai>> GetTheloai(int id)
        {
            var theloai = await _context.Theloais.FindAsync(id);

            if (theloai == null)
            {
                return NotFound();
            }

            return theloai;
        }

        // PUT: api/Theloais/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTheloai(int id, Theloai theloai)
        {
            if (id != theloai.MaTheLoai)
            {
                return BadRequest();
            }

            _context.Entry(theloai).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TheloaiExists(id))
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

        // POST: api/Theloais
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Theloai>> PostTheloai(Theloai theloai)
        {
            _context.Theloais.Add(theloai);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTheloai", new { id = theloai.MaTheLoai }, theloai);
        }

        // DELETE: api/Theloais/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheloai(int id)
        {
            var theloai = await _context.Theloais.FindAsync(id);
            if (theloai == null)
            {
                return NotFound();
            }

            _context.Theloais.Remove(theloai);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TheloaiExists(int id)
        {
            return _context.Theloais.Any(e => e.MaTheLoai == id);
        }
    }
}

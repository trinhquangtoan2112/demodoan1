using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.TheloaiDto;
using demodoan1.Data;

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
        public async Task<IActionResult> PutTheloai(int id, TheloaiDto theloaiDto)
        {
            try
            {
                var theloai = await _context.Theloais.FindAsync(id);

                if (theloai == null)
                {
                    return NotFound();
                }

                theloai.TenTheLoai = theloaiDto.TenTheLoai;
                theloai.MoTa = theloaiDto.MoTa;

                _context.Entry(theloai).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return NoContent();
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
            catch (Exception ex)
            {
                // Xử lý lỗi ở đây, ví dụ như ghi log lỗi, trả về lỗi cho client, vv.
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Lỗi khi cập nhật thể loại.", Exception = ex.Message });
            }
        }

        // POST: api/Theloais
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostTheloai([FromBody] TheloaiDto theloaiDto)
        {
            if (theloaiDto == null)
            {
                return BadRequest(new { Success = 400, Message = "Invalid data." });
            }

            var theloai = new Theloai
            {
                TenTheLoai = theloaiDto.TenTheLoai,
                MoTa = theloaiDto.MoTa,
                Trangthai = 0,
            };

            try
            {
                _context.Theloais.Add(theloai);
                await _context.SaveChangesAsync();
                return Ok(new { Success = 200, Data = theloaiDto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = 500, Message = "An error occurred while saving the data.", Error = ex.Message });
            }
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

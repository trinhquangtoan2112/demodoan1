﻿using System;
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
    public class PhanhoisController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public PhanhoisController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Phanhois
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Phanhoi>>> GetPhanhois()
        {
            return await _context.Phanhois.ToListAsync();
        }

        // GET: api/Phanhois/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Phanhoi>> GetPhanhoi(int id)
        {
            var phanhoi = await _context.Phanhois.FindAsync(id);

            if (phanhoi == null)
            {
                return NotFound();
            }

            return phanhoi;
        }

        // PUT: api/Phanhois/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhanhoi(int id, Phanhoi phanhoi)
        {
            if (id != phanhoi.MaPhanHoi)
            {
                return BadRequest();
            }

            _context.Entry(phanhoi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhanhoiExists(id))
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

        // POST: api/Phanhois
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Phanhoi>> PostPhanhoi(Phanhoi phanhoi)
        {
            _context.Phanhois.Add(phanhoi);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhanhoi", new { id = phanhoi.MaPhanHoi }, phanhoi);
        }

        // DELETE: api/Phanhois/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhanhoi(int id)
        {
            var phanhoi = await _context.Phanhois.FindAsync(id);
            if (phanhoi == null)
            {
                return NotFound();
            }

            _context.Phanhois.Remove(phanhoi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhanhoiExists(int id)
        {
            return _context.Phanhois.Any(e => e.MaPhanHoi == id);
        }
    }
}

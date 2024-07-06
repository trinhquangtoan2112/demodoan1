using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using demodoan1.Models;
using demodoan1.Models.TruyenDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruyensController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly DbDoAnTotNghiepContext _context;

        public TruyensController(DbDoAnTotNghiepContext context)
        {
            _context = context;
            Account account = new Account(
           "dzayfqach", // Replace with your Cloudinary cloud name
           "652647132213558",    // Replace with your Cloudinary API key
           "B1RZWapNbinaEjPUvg3K52VWiHo"  // Replace with your Cloudinary API secret
       );
            _cloudinary = new Cloudinary(account);
        }

        // GET: api/Truyens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Truyen>>> GetTruyens()
        {
            return await _context.Truyens.ToListAsync();
        }

        // GET: api/Truyens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Truyen>> GetTruyen(int id)
        {
            var truyen = await _context.Truyens.FindAsync(id);

            if (truyen == null)
            {
                return NotFound();
            }

            return truyen;
        }

        // PUT: api/Truyens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTruyen(int id, Truyen truyenDto)
        {
            if (id != truyenDto.MaTruyen)
            {
                return BadRequest();
            }

            var truyen = new Truyen
            {
                MaTruyen = truyenDto.MaTruyen,
                TenTruyen = truyenDto.TenTruyen,
                MoTa = truyenDto.MoTa,
                AnhBia = truyenDto.AnhBia,
                CongBo = truyenDto.CongBo,
                TrangThai = truyenDto.TrangThai,
                MaButDanh = truyenDto.MaButDanh,
                MaTheLoai = truyenDto.MaTheLoai
            };

            _context.Entry(truyen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TruyenExists(id))
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

        // POST: api/Truyens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Truyen>> PostTruyen([FromForm] TruyenDto truyenDto)
        {
            try
            {

                var dataTruyen = _context.Truyens.FirstOrDefault(item => item.TenTruyen == truyenDto.TenTruyen);
                if (dataTruyen == null)
                {
                    string linkTruyen;
                    using (var stream = truyenDto.AnhBia.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(truyenDto.TenTruyen, stream),
                            UseFilename = true,
                            UniqueFilename = true,
                            Overwrite = true
                        };
                        var uploadResult = _cloudinary.Upload(uploadParams);
                        linkTruyen = uploadResult.Url.ToString();
                    }
                    var truyen = new Truyen
                    {
                        TenTruyen = truyenDto.TenTruyen,
                        MoTa = truyenDto.MoTa,
                        AnhBia = linkTruyen,
                        CongBo =0,
                        TrangThai = 0,
                        MaButDanh = truyenDto.MaButDanh,
                        MaTheLoai = truyenDto.MaTheLoai
                    };

                    _context.Truyens.Add(truyen);
                    await _context.SaveChangesAsync();

                    return Created("TaoTruyen", new { status = StatusCodes.Status201Created, data = truyen });
                }
                return BadRequest(new { status = StatusCodes.Status400BadRequest, data = truyenDto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, data = truyenDto });
            }
            
        }

        // DELETE: api/Truyens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTruyen(int id)
        {
            var truyen = await _context.Truyens.FindAsync(id);
            if (truyen == null)
            {
                return NotFound();
            }

            _context.Truyens.Remove(truyen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TruyenExists(int id)
        {
            return _context.Truyens.Any(e => e.MaTruyen == id);
        }
    }
}

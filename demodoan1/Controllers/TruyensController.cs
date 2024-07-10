using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using demodoan1.Models;
using demodoan1.Models.TruyenDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using demodoan1.Data;

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
        public async Task<ActionResult> GetTruyens()
        {
          
            var taiKhoan = await _context.Truyens.Include(u => u.MaButDanhNavigation).Include(u => u.MaTheLoaiNavigation).Include(u => u.Chuongtruyens).ToListAsync();
            if(taiKhoan.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }
            var responseData = taiKhoan.Select(u => new
            {
                MaTruyen = u.MaTruyen,
                TenTruyen = u.TenTruyen,
                MoTa = u.MoTa,
                AnhBia = u.AnhBia,
                CongBo = u.CongBo,
                TrangThai = u.TrangThai,
                NgayTao = u.Ngaytao,
                coPhi = u.Chuongtruyens.Any(u => u.GiaChuong > 0)?true:false,
                NgayCapNhat = u.NgayCapNhap,
                TenButDanh = u.MaButDanh != null ? u.MaButDanhNavigation.TenButDanh : null,
                TenTheLoai = u.MaTheLoai != null ? u.MaTheLoaiNavigation.TenTheLoai : null,
            }).ToList();

            return Ok(new
            {
                status =StatusCodes.Status200OK,data =responseData
            });
        }

        [HttpGet("timkiem",Name ="Timkiem")]
        public async Task<ActionResult> GetTruyen(string? tenTruyen, int? matheLoa)
        {
            IQueryable<Truyen> query = _context.Truyens
                                               .Include(u => u.MaButDanhNavigation)
                                               .Include(u => u.MaTheLoaiNavigation).Where(u=> u.TrangThai !=3 || u.TrangThai !=4);

            if (!string.IsNullOrEmpty(tenTruyen))
            {
                query = query.Where(u => u.TenTruyen.Contains(tenTruyen));
            }

            if (matheLoa.HasValue)
            {
                query = query.Where(u => u.MaTheLoai == matheLoa);
            }

            var taiKhoan = await query.ToListAsync();

            if (taiKhoan.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }

            var responseData = taiKhoan.Select(u => new
            {
                MaTruyen = u.MaTruyen,
                TenTruyen = u.TenTruyen,
                MoTa = u.MoTa,
                AnhBia = u.AnhBia,
                CongBo = u.CongBo,
                TrangThai = u.TrangThai,
                NgayTao = u.Ngaytao,
                NgayCapNhat = u.NgayCapNhap,
                TenButDanh = u.MaButDanhNavigation != null ? u.MaButDanhNavigation.TenButDanh : null,
                TenTheLoai = u.MaTheLoaiNavigation != null ? u.MaTheLoaiNavigation.TenTheLoai : null,
            }).ToList();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = responseData
            });
        }


        // PUT: api/Truyens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTruyen(int id, [FromForm] CapNhapTruyenDto truyenDto)
        {
            try
            {
                var datauserr = _context.Truyens.FirstOrDefault(item => item.MaTruyen == id);
                if (datauserr == null)
                {
                    return NotFound(new
                    {
                        message = "Không tìm thấy truyện"
                    });
                }
                string linkTruyen = null;
                if (truyenDto.AnhBia != null)
                {
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
                }

                datauserr.TenTruyen = truyenDto.TenTruyen;
                datauserr.MoTa = truyenDto.MoTa;
                datauserr.AnhBia = linkTruyen != null ? linkTruyen : datauserr.AnhBia;
                datauserr.MaTheLoai = truyenDto.MaTheLoai;
                datauserr.TrangThai = truyenDto.TrangThai;

                _context.Truyens.Update(datauserr);

                try
                {
                    await _context.SaveChangesAsync();
                    return Ok(new { Message = "Cập nhập thông tin thành công" });
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
            catch (Exception ex)
            {
                return BadRequest();

            }

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
                    string linkTruyen = null;
                    if (truyenDto.AnhBia != null)
                    {
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
                    }

                    var truyen = new Truyen
                    {
                        TenTruyen = truyenDto.TenTruyen,
                        MoTa = truyenDto.MoTa,
                        AnhBia = linkTruyen != null ? linkTruyen : null,
                        CongBo = 0,
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
            truyen.TrangThai = 3;
            _context.Truyens.Update(truyen);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = StatusCodes.Status202Accepted,
                message = string.Format("Đã xóa thành công {0}", id)
            }
            );
        }

        private bool TruyenExists(int id)
        {
            return _context.Truyens.Any(e => e.MaTruyen == id);
        }
    }
}

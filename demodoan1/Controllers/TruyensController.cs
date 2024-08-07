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
using demodoan1.Helpers;
using NuGet.Common;

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

            var taiKhoan = await _context.Truyens.Include(u => u.MaButDanhNavigation).Include(u => u.MaTheLoaiNavigation).Include(u => u.Chuongtruyens).Where(item => item.CongBo != 0 && item.TrangThai != 4 && item.TrangThai!=0).ToListAsync();
            if (taiKhoan.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }
            var responseData = taiKhoan.Select(u => new
            {
                MaTruyen = u.MaTruyen,
                TenTruyen = u.TenTruyen,
                AnhBia = u.AnhBia,
                moTa = u.MoTa,
                CongBo = u.CongBo,
                TrangThai = u.TrangThai,
                NgayTao = u.Ngaytao,
                coPhi = u.Chuongtruyens.Any(u => u.GiaChuong > 0) ? true : false,
                NgayCapNhat = u.NgayCapNhap,
                TenButDanh = u.MaButDanh != null ? u.MaButDanhNavigation.TenButDanh : null,
                TenTheLoai = u.MaTheLoai != null ? u.MaTheLoaiNavigation.TenTheLoai : null,
                Luotdoc = u.Chuongtruyens.Sum(c => c.LuotDoc)
            }).ToList().OrderByDescending(item => item.Luotdoc);

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = responseData
            });
        }

            [HttpGet("GetTruyenTheoButDanh")]
            public async Task<ActionResult> GetTruyenTheoButDanh(int id)
            {

                var taiKhoan = await _context.Truyens.Include(u => u.MaButDanhNavigation).Include(u => u.MaTheLoaiNavigation).Include(u => u.Chuongtruyens).Where(item => item.MaButDanh ==id).ToListAsync();
                if (taiKhoan.Count == 0)
                {
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
                }
                var responseData = taiKhoan.Select(u => new
                {
                    MaTruyen = u.MaTruyen,
                    TenTruyen = u.TenTruyen,
                    AnhBia = u.AnhBia,
                    moTa = u.MoTa,
                    CongBo = u.CongBo,
                    TrangThai = u.TrangThai,
                    NgayTao = u.Ngaytao,
                    coPhi = u.Chuongtruyens.Any(u => u.GiaChuong > 0) ? true : false,
                    NgayCapNhat = u.NgayCapNhap,
                    TenButDanh = u.MaButDanh != null ? u.MaButDanhNavigation.TenButDanh : null,
                    TenTheLoai = u.MaTheLoai != null ? u.MaTheLoaiNavigation.TenTheLoai : null,
                    Luotdoc = u.Chuongtruyens.Sum(c => c.LuotDoc)
                }).ToList();

                return Ok(new
                {
                    status = StatusCodes.Status200OK,
                    data = responseData
                });
            }
            [HttpGet("TrangChu")]
        public async Task<ActionResult> GetTruyensTrangChu()
        {

            var taiKhoan = await _context.Truyens.Include(u => u.MaButDanhNavigation).Include(u => u.MaTheLoaiNavigation).Include(u => u.Chuongtruyens).Where(item => item.CongBo != 0 && item.TrangThai != 0 && item.TrangThai != 4).ToListAsync();
            if (taiKhoan.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }
            var responseData = taiKhoan.Select(u => new
            {
                MaTruyen = u.MaTruyen,
                TenTruyen = u.TenTruyen,
                TacGia = u.TacGia,
                moTa = u.MoTa,
                AnhBia = u.AnhBia,
                CongBo = u.CongBo,
                TrangThai = u.TrangThai,
                NgayTao = u.Ngaytao,
                DiemDanhGia = _context.Danhgia.Any(dg => dg.MaTruyen == u.MaTruyen) ? Math.Round((double)_context.Danhgia.Where(dg => dg.MaTruyen == u.MaTruyen).Average(dg => dg.DiemDanhGia), 1) : 0,
                coPhi = u.Chuongtruyens.Any(u => u.GiaChuong > 0) ? true : false,
                NgayCapNhat = u.NgayCapNhap,
                TenButDanh = u.MaButDanh != null ? u.MaButDanhNavigation.TenButDanh : null,
                TenTheLoai = u.MaTheLoai != null ? u.MaTheLoaiNavigation.TenTheLoai : null,
                Luotdoc = u.Chuongtruyens.Sum(c => c.LuotDoc)
            }).ToList().OrderByDescending(item => item.Luotdoc).Take(3);
            var responseData1 = taiKhoan.Select(u => new
            {
                MaTruyen = u.MaTruyen,
                TenTruyen = u.TenTruyen,
                TacGia = u.TacGia,
                AnhBia = u.AnhBia,
                CongBo = u.CongBo,
                TrangThai = u.TrangThai,
                NgayTao = u.Ngaytao,
                DiemDanhGia = _context.Danhgia.Any(dg => dg.MaTruyen == u.MaTruyen) ? Math.Round((double)_context.Danhgia.Where(dg => dg.MaTruyen == u.MaTruyen).Average(dg => dg.DiemDanhGia), 1) : 0,
                coPhi = u.Chuongtruyens.Any(u => u.GiaChuong > 0) ? true : false,
                NgayCapNhat = u.NgayCapNhap,
                TenButDanh = u.MaButDanh != null ? u.MaButDanhNavigation.TenButDanh : null,
                TenTheLoai = u.MaTheLoai != null ? u.MaTheLoaiNavigation.TenTheLoai : null,
                Luotdoc = u.Chuongtruyens.Sum(c => c.LuotDoc)
            }).ToList().OrderByDescending(item => item.Luotdoc).Skip(3).Take(30);
            return Ok(new
            {
                status = StatusCodes.Status200OK,
                Dexuat = responseData,
                conlai = responseData1
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
        [HttpGet("timkiemButDanh", Name = "TimkiemButDanh")]
        public async Task<ActionResult> TimkiemButDanh(string? tenTruyen, string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                return NotFound(new { status = StatusCodes.Status400BadRequest, message = "Không tìm thấy" });
            }if(String.IsNullOrEmpty(tenTruyen))
            {
                return NotFound(new { status = StatusCodes.Status400BadRequest, message = "Nhập tên truyện" });
            }
            token = token.Trim();
            var data = token.Substring(7);
            Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
            string iDNguoiDung = claimsData["IdUserName"];
            var taiKhoan = await _context.Butdanhs.Include(u => u.Truyens).ThenInclude(u => u.MaTheLoaiNavigation).Where(item => item.MaNguoiDung == Int64.Parse(iDNguoiDung)).ToListAsync();
            if (taiKhoan.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }

            var maButDanhList = taiKhoan.Select(t => t.MaButDanh).ToList();

            var dsTruyen = await _context.Truyens
                .Include(u => u.MaTheLoaiNavigation).Include(u => u.MaButDanhNavigation)
                .Where(item => maButDanhList.Contains(item.MaButDanh) && item.TenTruyen.Contains(tenTruyen))
                .Select(u => new
                {
                    MaTruyen = u.MaTruyen,
                    TenTruyen = u.TenTruyen,
                    MoTa = u.MoTa,
                    AnhBia = u.AnhBia,
                    CongBo = u.CongBo,
                    TrangThai = u.TrangThai,
                    NgayTao = u.Ngaytao,
                    NgayCapNhat = u.NgayCapNhap,
                    MaButDanh = u.MaButDanh,
                    TenButDanh = u.MaButDanhNavigation.TenButDanh,
                    TrangThaiButDanh = u.MaButDanhNavigation.Trangthai,
                    TenTheLoai = u.MaTheLoaiNavigation != null ? u.MaTheLoaiNavigation.TenTheLoai : null
                })
                .ToListAsync();
            var responseData = taiKhoan.Select(t => new
            {
                MaButDanh = t.MaButDanh,
                TenButDanh = t.TenButDanh,
                TrangThai = t.Trangthai,
                Truyen = t.Truyens.Select(u => new
                {
                    MaTruyen = u.MaTruyen,
                    TenTruyen = u.TenTruyen,
                    MoTa = u.MoTa,
                    AnhBia = u.AnhBia,
                    CongBo = u.CongBo,
                    TrangThai = u.TrangThai,
                    NgayTao = u.Ngaytao,
                    NgayCapNhat = u.NgayCapNhap,

                    TenTheLoai = u.MaTheLoaiNavigation != null ? u.MaTheLoaiNavigation.TenTheLoai : null,
                }).ToList()
            }).ToList();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = dsTruyen
            });
        }

        [HttpGet("GetTruyenID", Name = "GetTruyenID")]
        public async Task<ActionResult> GetTruyenID(int id, int page = 1, int pageSize = 20)
        {
            var truyen = await _context.Truyens
                .Include(t => t.MaButDanhNavigation)
                .Include(t => t.MaTheLoaiNavigation)
                .Include(t => t.Chuongtruyens)
                .FirstOrDefaultAsync(t => t.MaTruyen == id);

            if (truyen == null)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }

          
            int skip = (page - 1) * pageSize;
            int take = pageSize;

            var totalCount = truyen.Chuongtruyens.Count(); 
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);


            var responseData = new
            {
                MaTruyen = truyen.MaTruyen,
                TenTruyen = truyen.TenTruyen,
                TacGia = truyen.TacGia,
                MoTa = truyen.MoTa,
                AnhBia = truyen.AnhBia,
                CongBo = truyen.CongBo,
                TrangThai = truyen.TrangThai,
                DiemDanhGia = _context.Danhgia.Any(dg => dg.MaTruyen == truyen.MaTruyen) ? Math.Round((double)_context.Danhgia.Where(dg => dg.MaTruyen == truyen.MaTruyen).Average(dg => dg.DiemDanhGia), 1) : 0,
                NgayTao = truyen.Ngaytao,
                NgayCapNhat = truyen.NgayCapNhap,
                coPhi = truyen.Chuongtruyens.Any(u => u.GiaChuong > 0) ? true : false,
                TenButDanh = truyen.MaButDanhNavigation?.TenButDanh,
                TenTheLoai = truyen.MaTheLoaiNavigation?.TenTheLoai,
                MaTheLoai = truyen.MaTheLoai,
                MaButDanh = truyen.MaButDanh,
                Solike = _context.Likes.Count(l => l.MaThucThe == truyen.MaTruyen && l.LoaiThucTheLike == 1),
                TongLuotDoc = truyen.Chuongtruyens.Sum(c => c.LuotDoc),
                TotalCount = totalPages, // Tổng số chương
                Page = page,
                PageSize = pageSize,
                Data = truyen.Chuongtruyens
                    .Where(ch => ch.TrangThai != 0 && ch.HienThi != 0 &&ch.TrangThai !=4)
                    .OrderBy(ch => ch.Stt)
                    .Skip(skip)
                    .Take(take)
                    .Select(ch => new
                    {
                        MaChuong = ch.MaChuong,
                        TenChuong = ch.TenChuong,
                        GiaChuong = ch.GiaChuong,
                        Solike = _context.Likes.Count(l => l.MaThucThe == ch.MaChuong && l.LoaiThucTheLike == 5),
                        NgayTao = ch.Ngaytao,
                        NgayCapNhat = ch.NgayCapNhap,
                        Stt = ch.Stt
                    })
                    .ToList()
            };

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = responseData
            });
        }

        [HttpGet("PhanTrang")]
        public async Task<ActionResult> PhanTrang(int id, int page)
        {
            int pageSize = 20;
            var truyen = await _context.Truyens
                .Include(t => t.MaButDanhNavigation)
                .Include(t => t.MaTheLoaiNavigation)
                .Include(t => t.Chuongtruyens)
                .FirstOrDefaultAsync(t => t.MaTruyen == id);

            if (truyen == null)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }

            // Tính toán chỉ số bắt đầu và số lượng chương cần lấy
            int skip = (page - 1) * pageSize;
            int take = pageSize;

            var totalCount = truyen.Chuongtruyens.Count(); // Tổng số chương trong truyện
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var responseData = new
            {
                MaTruyen = truyen.MaTruyen,
                TenTruyen = truyen.TenTruyen,
                MoTa = truyen.MoTa,
                AnhBia = truyen.AnhBia,
                CongBo = truyen.CongBo,
                TrangThai = truyen.TrangThai,
                NgayTao = truyen.Ngaytao,
                NgayCapNhat = truyen.NgayCapNhap,
                TenButDanh = truyen.MaButDanhNavigation?.TenButDanh,
                TenTheLoai = truyen.MaTheLoaiNavigation?.TenTheLoai,
                MaTheLoai = truyen.MaTheLoai,
                MaButDanh = truyen.MaButDanh,
                TongLuotDoc = truyen.Chuongtruyens.Sum(c => c.LuotDoc),
                TotalCount = totalPages, 
                Page = page,
                PageSize = pageSize,
                Data = truyen.Chuongtruyens
                    .Where(ch => ch.TrangThai != 4 && ch.HienThi != 0 &&ch.TrangThai !=4)
                    .OrderBy(ch => ch.Stt)
                    .Skip(skip)
                    .Take(take)
                    .Select(ch => new
                    {
                        MaChuong = ch.MaChuong,
                        TenChuong = ch.TenChuong,
                        GiaChuong = ch.GiaChuong,
                        NgayTao = ch.Ngaytao,
                        NgayCapNhat = ch.NgayCapNhap,
                        Stt = ch.Stt
                    })
                    .ToList()
            };

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = responseData
            });
        }
        // PUT: api/Truyens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("SuaTruyen")]
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
                datauserr.TacGia = truyenDto.TacGia;
                datauserr.MoTa = truyenDto.MoTa;
                datauserr.AnhBia = linkTruyen != null ? linkTruyen : datauserr.AnhBia;
                datauserr.MaTheLoai = truyenDto.MaTheLoai;
                datauserr.TrangThai =0;

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
                        return NoContent();
                    }
                }

              
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
        [HttpPut("AnTruyen")]
        public async Task<IActionResult> AnTruyen(int id)
        {
            var truyen = await _context.Truyens.FindAsync(id);
            if (truyen == null)
            {
                return NotFound();
            }
            truyen.CongBo = 0;
            _context.Truyens.Update(truyen);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = StatusCodes.Status202Accepted,
                message = string.Format("Đã ẩn thành công {0}", id)
            }
            );
        }
        [HttpPut("HienTruyen")]
        public async Task<IActionResult> HienTruyen(int id)
        {
            var truyen = await _context.Truyens.FindAsync(id);
            if (truyen == null)
            {
                return NotFound();
            }
            truyen.CongBo = 1;
            _context.Truyens.Update(truyen);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = StatusCodes.Status202Accepted,
                message = string.Format("Đã hiện thành công {0}", id)
            }
            );
        }
        private bool TruyenExists(int id)
        {
            return _context.Truyens.Any(e => e.MaTruyen == id);
        }
        [HttpGet("GetDSTruyenAdmin")]
        public async Task<ActionResult> GetTruyensAdmin()
        {

            var taiKhoan = await _context.Truyens.Include(u => u.MaButDanhNavigation).Include(u => u.MaTheLoaiNavigation).Include(u => u.Chuongtruyens).ToListAsync();
            if (taiKhoan.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }
            var responseData = taiKhoan.Select(u => new
            {
                MaTruyen = u.MaTruyen,
                TenTruyen = u.TenTruyen,
                AnhBia = u.AnhBia,
                moTa = u.MoTa,
                CongBo = u.CongBo,
                TrangThai = u.TrangThai,
                NgayTao = u.Ngaytao,
                coPhi = u.Chuongtruyens.Any(u => u.GiaChuong > 0) ? true : false,
                NgayCapNhat = u.NgayCapNhap,
                TenButDanh = u.MaButDanh != null ? u.MaButDanhNavigation.TenButDanh : null,
                TenTheLoai = u.MaTheLoai != null ? u.MaTheLoaiNavigation.TenTheLoai : null,
                Luotdoc = u.Chuongtruyens.Sum(c => c.LuotDoc)
            }).ToList().OrderByDescending(item => item.Luotdoc);

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = responseData
            });
        }
        [HttpPut("KhoaTruyen")]
        public async Task<ActionResult> KhoaTruyen(int maTruyen)
        {

            var taiKhoan =  _context.Truyens.FirstOrDefault(item => item.MaTruyen == maTruyen);
            taiKhoan.TrangThai = 4;
            _context.Update(taiKhoan);
            _context.SaveChanges();
         
            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Khóa thành công"

            });
        }
        [HttpPut("MoKhoaTruyen")]
        public async Task<ActionResult> MoKhoaTruyen(int maTruyen)
        {
            var taiKhoan = _context.Truyens.FirstOrDefault(item => item.MaTruyen == maTruyen);
            taiKhoan.TrangThai = 1;
            _context.Update(taiKhoan);
            _context.SaveChanges();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Mở khóa thành công"
            });
        }
        [HttpGet("DanhsachTruyenCanDuyet")]
        public async Task<ActionResult> DanhsachTruyenCanDuyet()
        {
            var taiKhoan = _context.Truyens.Include(u => u.MaButDanhNavigation).Include(u => u.MaTheLoaiNavigation).Include(u => u.Chuongtruyens).Where(item => item.TrangThai == 0 ).ToList();
            if (taiKhoan.Count == 0)
            {
                return NotFound(new
                {
                    status = StatusCodes.Status404NotFound,
                    message = "Không có truyện",
                });
            }
            var responseData = taiKhoan.Select(u => new
            {
                MaTruyen = u.MaTruyen,
                TenTruyen = u.TenTruyen,
                AnhBia = u.AnhBia,
                moTa = u.MoTa,
                CongBo = u.CongBo,
                TrangThai = u.TrangThai,
                NgayTao = u.Ngaytao,
                coPhi = u.Chuongtruyens.Any(u => u.GiaChuong > 0) ? true : false,
                NgayCapNhat = u.NgayCapNhap,
                TenButDanh = u.MaButDanh != null ? u.MaButDanhNavigation.TenButDanh : null,
                TenTheLoai = u.MaTheLoai != null ? u.MaTheLoaiNavigation.TenTheLoai : null,
                Luotdoc = u.Chuongtruyens.Sum(c => c.LuotDoc)
            }).ToList().OrderByDescending(item => item.Luotdoc);
            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Danh sách truyện",
                data = responseData
            });
        }
        [HttpPut("DuyetTruyen")]
        public async Task<ActionResult> DuyetTruyen(int maTruyen)
        {
            var taiKhoan = _context.Truyens.FirstOrDefault(item => item.MaTruyen == maTruyen);
            if(taiKhoan == null)
            {
                return NotFound(new
                {
                    status = StatusCodes.Status404NotFound,
                    message = "Không có truyện",
                });
            }
            taiKhoan.TrangThai = 1;
            _context.Truyens.Update(taiKhoan);
            _context.SaveChanges();
            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Thành công",
            });
        }
        [HttpGet("GetTruyenTheoTenTruyen")]
        public async Task<ActionResult> GetTruyenTheoTenTruyen(String tenTruyen)
        {
            var searchString = tenTruyen?.ToLower() ?? "";

            var taiKhoan = await _context.Truyens.Include(u => u.MaButDanhNavigation).Include(u => u.MaTheLoaiNavigation).Include(u => u.Chuongtruyens).Where(item => item.TenTruyen.Contains(searchString) && item.TrangThai!=0 && item.TrangThai !=4).ToListAsync();
            if (taiKhoan.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }
            var responseData = taiKhoan.Select(u => new
            {
                MaTruyen = u.MaTruyen,
                TenTruyen = u.TenTruyen,
                AnhBia = u.AnhBia,
                moTa = u.MoTa,
                CongBo = u.CongBo,
                TrangThai = u.TrangThai,
                DiemDanhGia = _context.Danhgia.Any(dg => dg.MaTruyen == u.MaTruyen) ? Math.Round((double)_context.Danhgia.Where(dg => dg.MaTruyen == u.MaTruyen).Average(dg => dg.DiemDanhGia), 1) : 0,
                NgayTao = u.Ngaytao,
                coPhi = u.Chuongtruyens.Any(u => u.GiaChuong > 0) ? true : false,
                NgayCapNhat = u.NgayCapNhap,
                TenButDanh = u.MaButDanh != null ? u.MaButDanhNavigation.TenButDanh : null,
                TenTheLoai = u.MaTheLoai != null ? u.MaTheLoaiNavigation.TenTheLoai : null,
                Luotdoc = u.Chuongtruyens.Sum(c => c.LuotDoc)
            }).ToList();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = responseData
            });
        }
        
        [HttpGet("GetTruyenTheoIDNguoiDung")]
        public async Task<ActionResult> GetTruyenTheoIDNguoiDung(String token)
        {

            if (String.IsNullOrEmpty(token))
            {
                return NotFound(new { status = StatusCodes.Status400BadRequest, message = "Không tìm thấy" });
            }
            token = token.Trim();
            var data = token.Substring(7);
            Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
            string iDNguoiDung = claimsData["IdUserName"];
            var taiKhoan = await _context.Butdanhs.Include(u => u.Truyens).ThenInclude(u => u.MaTheLoaiNavigation).Where(item => item.MaNguoiDung ==Int64.Parse(iDNguoiDung) ).ToListAsync();
            if (taiKhoan.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }

            var maButDanhList = taiKhoan.Select(t => t.MaButDanh).ToList();

            var dsTruyen = await _context.Truyens
                .Include(u => u.MaTheLoaiNavigation).Include(u => u.MaButDanhNavigation)
                .Where(item => maButDanhList.Contains(item.MaButDanh))
                .Select(u => new
                {
                    MaTruyen = u.MaTruyen,
                    TenTruyen = u.TenTruyen,
                    MoTa = u.MoTa,
                    AnhBia = u.AnhBia,
                    CongBo = u.CongBo,
                    TrangThai = u.TrangThai,
                    NgayTao = u.Ngaytao,
                    NgayCapNhat = u.NgayCapNhap,
                    MaButDanh = u.MaButDanh,
                    TenButDanh = u.MaButDanhNavigation.TenButDanh,
                    TrangThaiButDanh = u.MaButDanhNavigation.Trangthai,
                    TenTheLoai = u.MaTheLoaiNavigation != null ? u.MaTheLoaiNavigation.TenTheLoai : null
                })
                .ToListAsync();
            var responseData = taiKhoan.Select(t => new
            {
                MaButDanh = t.MaButDanh,
                TenButDanh = t.TenButDanh,
               TrangThai = t.Trangthai,
                Truyen = t.Truyens.Select(u => new
                {
                    MaTruyen = u.MaTruyen,
                    TenTruyen = u.TenTruyen,
                    MoTa = u.MoTa,
                    AnhBia = u.AnhBia,
                    CongBo = u.CongBo,
                    TrangThai = u.TrangThai,
                    NgayTao = u.Ngaytao,
                    NgayCapNhat = u.NgayCapNhap,
                  
                    TenTheLoai = u.MaTheLoaiNavigation != null ? u.MaTheLoaiNavigation.TenTheLoai : null,
                }).ToList()
            }).ToList();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = dsTruyen
            });
        }

        /*
.Sum(t => t.LoaiTien == 1 ? t.SoTien : t.MaChuongTruyenNavigation.GiaChuong* t.SoTien) :
Sum tính tổng doanh thu từ các giao dịch đã lọc ở bước trước.
t.LoaiTien == 1 ? t.SoTien : t.MaChuongTruyenNavigation.GiaChuong* t.SoTien là biểu thức điều kiện để tính doanh thu từ hai loại tiền khác nhau:
Nếu LoaiTien == 1 (tức là xu), thì giá trị doanh thu chính là t.SoTien.
Nếu LoaiTien == 2 (tức là chìa khóa), thì giá trị doanh thu là giá chương(GiaChuong) nhân với số tiền(SoTien). Điều này vì mỗi chìa khóa sẽ có giá trị bằng giá chương(GiaChuong).*/
        [HttpGet("Xemdoanhthutruyen")]
        public async Task<ActionResult> GetXemdoanhthutruyen(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return NotFound(new { status = StatusCodes.Status400BadRequest, message = "Không tìm thấy" });
            }

            token = token.Trim();
            var data = token.Substring(7);
            Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);

            if (!claimsData.ContainsKey("IdUserName") || !long.TryParse(claimsData["IdUserName"], out long iDNguoiDung))
            {
                return NotFound(new { status = StatusCodes.Status400BadRequest, message = "Không tìm thấy" });
            }

            var taiKhoan = await _context.Butdanhs.Include(u => u.Truyens)
                .ThenInclude(u => u.MaTheLoaiNavigation)
                .Where(item => item.MaNguoiDung == iDNguoiDung)
                .ToListAsync();

            if (taiKhoan.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }

            var maButDanhList = taiKhoan.Select(t => t.MaButDanh).ToList();

            try
            {
                var dsTruyen = await _context.Truyens
                    .Include(u => u.MaTheLoaiNavigation)
                    .Include(u => u.MaButDanhNavigation)
                    .Where(item => maButDanhList.Contains(item.MaButDanh))
                    .Select(u => new
                    {
                        MaTruyen = u.MaTruyen,
                        TenTruyen = u.TenTruyen,
                        TenButDanh = u.MaButDanhNavigation.TenButDanh,
                        TenTheLoai = u.MaTheLoaiNavigation != null ? u.MaTheLoaiNavigation.TenTheLoai : null,
                        Luotdoc = u.Chuongtruyens.Sum(c => c.LuotDoc),
                        LuotmuabangXu = u.Chuongtruyens.SelectMany(c => c.Giaodiches)
                            .Count(t => t.LoaiGiaoDich == 1 && t.LoaiTien == 1),
                        Luotmuabangchiakhoa = u.Chuongtruyens.SelectMany(c => c.Giaodiches)
                            .Count(t => t.LoaiGiaoDich == 1 && t.LoaiTien == 2),
                        Tongdoanhthuthuve = u.Chuongtruyens.SelectMany(c => c.Giaodiches)
                            .Where(t => t.LoaiGiaoDich == 1)
                            .Sum(t => t.LoaiTien == 1 ? t.SoTien : 10 * t.SoTien),
                        Sodecu = u.Chuongtruyens.SelectMany(c => c.Giaodiches)
                    .Where(t => t.LoaiGiaoDich == 4 && t.LoaiTien == 4)
                    .Sum(t => t.SoTien)
                    }).ToListAsync();

                return Ok(new { status = StatusCodes.Status200OK, data = dsTruyen });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }


        [HttpGet("XemdoanhthutruyenAdmin")]
        public async Task<ActionResult> GetXemdoanhthutruyenAdmin(string token)
        {
            token = token.Trim();
            var data = token.Substring(7); // Remove "Bearer " prefix
            Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
            string iDNguoiDung = claimsData["IdUserName"];
            int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

            if (string.IsNullOrEmpty(token))
            {
                return NotFound(new { status = StatusCodes.Status400BadRequest, message = "Không tìm thấy" });
            }


            var user = await _context.Users.FindAsync(maNguoiDung);
            if (user.MaQuyen != 1) // Not admin
            {
                return Unauthorized(new { status = StatusCodes.Status401Unauthorized, message = "Bạn không có quyền xem thông tin." });
            }

            var taiKhoan = await _context.Butdanhs.Include(u => u.Truyens)
                .ThenInclude(u => u.MaTheLoaiNavigation).ToListAsync();

            if (taiKhoan.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }

            var maButDanhList = taiKhoan.Select(t => t.MaButDanh).ToList();

            try
            {
                var dsTruyen = await _context.Truyens
                    .Include(u => u.MaTheLoaiNavigation)
                    .Include(u => u.MaButDanhNavigation)
                    .Where(item => maButDanhList.Contains(item.MaButDanh))
                    .Select(u => new
                    {
                        MaTruyen = u.MaTruyen,
                        TenTruyen = u.TenTruyen,
                        TenButDanh = u.MaButDanhNavigation.TenButDanh,
                        TenTheLoai = u.MaTheLoaiNavigation != null ? u.MaTheLoaiNavigation.TenTheLoai : null,
                        Luotdoc = u.Chuongtruyens.Sum(c => c.LuotDoc),
                        LuotmuabangXu = u.Chuongtruyens.SelectMany(c => c.Giaodiches)
                            .Count(t => t.LoaiGiaoDich == 1 && t.LoaiTien == 1),
                        Luotmuabangchiakhoa = u.Chuongtruyens.SelectMany(c => c.Giaodiches)
                            .Count(t => t.LoaiGiaoDich == 1 && t.LoaiTien == 2),
                        Tongdoanhthuthuve = u.Chuongtruyens.SelectMany(c => c.Giaodiches)
                            .Where(t => t.LoaiGiaoDich == 1)
                            .Sum(t => t.LoaiTien == 1 ? t.SoTien : 10 * t.SoTien),
                        Sodecu = u.Chuongtruyens.SelectMany(c => c.Giaodiches)
                    .Where(t => t.LoaiGiaoDich == 4 && t.LoaiTien == 4)
                    .Sum(t => t.SoTien)
                    }).ToListAsync();

                return Ok(new { status = StatusCodes.Status200OK, data = dsTruyen });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }

    }
}

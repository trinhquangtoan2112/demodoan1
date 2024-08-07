using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.ChuongtruyenDto;
using demodoan1.Models.TruyenDto;
using demodoan1.Helpers;
using Org.BouncyCastle.Asn1.Ocsp;
using static System.Net.Mime.MediaTypeNames;

namespace demodoan1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChuongtruyensController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;
        private readonly TextToSpeechService _ttsService;
        public ChuongtruyensController(DbDoAnTotNghiepContext context, TextToSpeechService ttsService)
        {
            _context = context;
            _ttsService = ttsService;
        }

        // GET: api/Chuongtruyens

        [HttpGet("Danhsachchuong")]
        public async Task<ActionResult> GetChuongtruyens([FromQuery] int maTruyen,[FromQuery] string? token)
        {
            string tokenData =null;
            if (!string.IsNullOrEmpty(token))
            {
                 tokenData = TokenClass.Decodejwt(token);
            }
          
            var taiKhoan = await _context.Chuongtruyens.Where(item =>item.MaTruyen ==maTruyen && item.TrangThai!=0 && item.HienThi != 0 && item.TrangThai != 4).ToListAsync();
            if (taiKhoan.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }
           
            var responseData = taiKhoan.Select(u => new
            {
                Machuongtruyen = u.MaChuong,
                TenChuong = u.TenChuong,
                TrangThai = u.TrangThai,
                Luotdoc =u.LuotDoc,
                HienThi = u.HienThi,
                GiaChuong = u.GiaChuong,
                Stt =u.Stt,
                NgayTao = u.Ngaytao,
                NgayCapNhat = u.NgayCapNhap,
                Damua = u.GiaChuong > 0
        ? tokenData!=null?  _context.Giaodiches.Any(g => g.MaChuongTruyen == u.MaChuong && g.MaNguoiDung == Int64.Parse(tokenData)):false
        : (bool?)true // Return null if GiaChuong is not greater than 0
            }).ToList();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = responseData
            });
           
        }
        [HttpGet("DanhsachchuongTacGia")]
        public async Task<ActionResult> DanhsachchuongTacGia([FromQuery] int maTruyen, [FromQuery] string? token)
        {
            string tokenData = null;
            if (!string.IsNullOrEmpty(token))
            {
                tokenData = TokenClass.Decodejwt(token);
            }

            var taiKhoan = await _context.Chuongtruyens.Where(item => item.MaTruyen == maTruyen && item.TrangThai != 0 ).ToListAsync();
            if (taiKhoan.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }

            var responseData = taiKhoan.Select(u => new
            {
                Machuongtruyen = u.MaChuong,
                TenChuong = u.TenChuong,
                TrangThai = u.TrangThai,
                Luotdoc = u.LuotDoc,
                HienThi = u.HienThi,
                GiaChuong = u.GiaChuong,
                Stt = u.Stt,
                NgayTao = u.Ngaytao,
                NgayCapNhat = u.NgayCapNhap,
                Damua = u.GiaChuong > 0
        ? tokenData != null ? _context.Giaodiches.Any(g => g.MaChuongTruyen == u.MaChuong && g.MaNguoiDung == Int64.Parse(tokenData)) : false
        : (bool?)true // Return null if GiaChuong is not greater than 0
            }).ToList();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = responseData
            });

        }
        [HttpGet("DanhsachchuongID")]
        public async Task<ActionResult> DanhsachchuongID([FromQuery] int id )
        {


            var taiKhoan = await _context.Chuongtruyens.Where(item => item.MaTruyen == id && item.MaChuong !=22).ToListAsync();
                var taiKhoan1 =  _context.Truyens.FirstOrDefault(item => item.MaTruyen == id);
            if (taiKhoan.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }

            var responseData = taiKhoan.Select(u => new
            {
                Machuongtruyen = u.MaChuong,
                TenChuong = u.TenChuong,
                TrangThai = u.TrangThai,
                Luotdoc = u.LuotDoc,
                HienThi = u.HienThi,
                GiaChuong = u.GiaChuong,
                Stt = u.Stt,
                NgayTao = u.Ngaytao,
                NgayCapNhat = u.NgayCapNhap,
                Damua = u.GiaChuong > 0,
                
            }).ToList();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = responseData,
                tenTruyen = taiKhoan1.TenTruyen
            });

        }
        // GET: api/Chuongtruyens/5
        [HttpGet("GetChiTietChuong")]
        public async Task<ActionResult<Chuongtruyen>> GetChuongtruyen([FromQuery] int maChuong, [FromQuery] string? token)
        {
            try
            {
                var currentChapter = _context.Chuongtruyens.FirstOrDefault(item => item.MaChuong == maChuong &&item.TrangThai !=0);
                string tokenData = null;
                Lichsudoc lichSuDoc =null;
                tokenData = TokenClass.Decodejwt(token);
                if (!string.IsNullOrEmpty(token))
                {
                    lichSuDoc = _context.Lichsudocs.FirstOrDefault(item => item.MaChuongTruyen == maChuong && item.MaNguoiDung == Int64.Parse(tokenData));
                    
                }
                    if (currentChapter == null)
                {
                    return NotFound();

                }
                else
                {
                    var hasNextChapter = await _context.Chuongtruyens
        .AnyAsync(c => c.MaTruyen == currentChapter.MaTruyen && c.Stt > currentChapter.Stt);
                 

                    var previousChapter = await _context.Chuongtruyens
                        .Where(c => c.MaTruyen == currentChapter.MaTruyen && c.Stt < currentChapter.Stt)
                        .OrderByDescending(c => c.Stt)
                        .FirstOrDefaultAsync();

                    var nextChapter = await _context.Chuongtruyens
                        .Where(c => c.MaTruyen == currentChapter.MaTruyen && c.Stt > currentChapter.Stt)
                        .OrderBy(c => c.Stt)
                        .FirstOrDefaultAsync();
                    // Get the first 10 chapters
                    var first10Chapters = await _context.Chuongtruyens
                        .Where(c => c.MaTruyen == currentChapter.MaTruyen)
                        .OrderBy(c => c.Stt)
                        .Take(10)
                        .Select(c => c.MaChuong)
                        .ToListAsync();

                    if (currentChapter.GiaChuong == 0 || first10Chapters.Contains(currentChapter.MaChuong))
                    {
                        var responseData = new
                        {
                            maTruyen = currentChapter.MaTruyen,
                            Machuongtruyen = currentChapter.MaChuong,
                            TenChuong = currentChapter.TenChuong,
                            TrangThai = currentChapter.TrangThai,
                            NoiDung = currentChapter.NoiDung,
                            HienThi = currentChapter.HienThi,
                            GiaChuong = currentChapter.GiaChuong,
                            LuotDoc = currentChapter.LuotDoc,
                            Stt = currentChapter.Stt,
                            Solike = _context.Likes.Count(l => l.MaThucThe == currentChapter.MaChuong),
                            NgayTao = currentChapter.Ngaytao,
                            NgayCapNhat = currentChapter.NgayCapNhap,
                            trangTruoc = previousChapter?.MaChuong,
                            trangTiep = nextChapter?.MaChuong,
                            viTri = lichSuDoc!=null? lichSuDoc.ViTri:0
                        };
                        return Ok(new { StatusCode = StatusCodes.Status200OK, data = responseData });
                    }
                    else
                    {
                       
                        var responseData1 = new
                        {
                            Machuongtruyen = currentChapter.MaChuong,
                            Stt = currentChapter.Stt,
                            TenChuong = currentChapter.TenChuong,
                            maTruyen = currentChapter.MaTruyen,
                            Solike = _context.Likes.Count(l => l.MaThucThe == currentChapter.MaChuong),
                            GiaChuong = currentChapter.GiaChuong,
                            trangTruoc = previousChapter?.MaChuong,
                            trangTiep = nextChapter?.MaChuong,
                            viTri = lichSuDoc != null ? lichSuDoc.ViTri : 0
                        };
                        if (!string.IsNullOrEmpty(token))
                        {
                          
                            Boolean daMua = tokenData != null ? _context.Giaodiches.Any(g => g.MaChuongTruyen == maChuong && g.MaNguoiDung == Int64.Parse(tokenData) && g.LoaiGiaoDich == 1) : false;
                          
                            if (!daMua)
                            {
                            
                                return Unauthorized(new
                                {
                                    Status = StatusCodes.Status401Unauthorized,
                                    data = responseData1
                                });
                            }
                            var responseData = new
                            {
                                Machuongtruyen = currentChapter.MaChuong,
                                TenChuong = currentChapter.TenChuong,
                                TrangThai = currentChapter.TrangThai,
                                NoiDung = currentChapter.NoiDung,
                                HienThi = currentChapter.HienThi,
                                GiaChuong = currentChapter.GiaChuong,
                                LuotDoc = currentChapter.LuotDoc,
                                Stt = currentChapter.Stt,
                                Solike = _context.Likes.Count(l => l.MaThucThe == currentChapter.MaChuong),
                                NgayTao = currentChapter.Ngaytao,
                                NgayCapNhat = currentChapter.NgayCapNhap,
                                trangTruoc = previousChapter?.MaChuong,
                                trangTiep = nextChapter?.MaChuong,
                                viTri = lichSuDoc != null ? lichSuDoc.ViTri : 0
                            };
                            return Ok(new
                            {
                                Status =StatusCodes.Status200OK,
                               data = responseData
                            });
                        }
                     
                        return Unauthorized(new
                        {
                            Status = StatusCodes.Status401Unauthorized,
                            data = responseData1
                        });

                    }
                }
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        
        [HttpGet("GetChiTietChuongAdmin")]
        public async Task<ActionResult<Chuongtruyen>> GetChiTietChuongAdmin([FromQuery] int maChuong, [FromQuery] string? token)
        {
            try
            {
                var currentChapter = _context.Chuongtruyens.FirstOrDefault(item => item.MaChuong == maChuong );

                if (currentChapter == null)
                {
                    return NotFound();

                }
                else
                {
                    var hasNextChapter = await _context.Chuongtruyens
        .AnyAsync(c => c.MaTruyen == currentChapter.MaTruyen && c.Stt > currentChapter.Stt);


                    var previousChapter = await _context.Chuongtruyens
                        .Where(c => c.MaTruyen == currentChapter.MaTruyen && c.Stt < currentChapter.Stt)
                        .OrderByDescending(c => c.Stt)
                        .FirstOrDefaultAsync();

                    var nextChapter = await _context.Chuongtruyens
                        .Where(c => c.MaTruyen == currentChapter.MaTruyen && c.Stt > currentChapter.Stt)
                        .OrderBy(c => c.Stt)
                        .FirstOrDefaultAsync();

                        var responseData = new
                        {
                            maTruyen = currentChapter.MaTruyen,
                            Machuongtruyen = currentChapter.MaChuong,
                            TenChuong = currentChapter.TenChuong,
                            TrangThai = currentChapter.TrangThai,
                            NoiDung = currentChapter.NoiDung,
                            HienThi = currentChapter.HienThi,
                            GiaChuong = currentChapter.GiaChuong,
                            LuotDoc = currentChapter.LuotDoc,
                            Stt = currentChapter.Stt,
                            Solike = _context.Likes.Count(l => l.MaThucThe == currentChapter.MaChuong),
                            NgayTao = currentChapter.Ngaytao,
                            NgayCapNhat = currentChapter.NgayCapNhap,
                            trangTruoc = previousChapter?.MaChuong,
                            trangTiep = nextChapter?.MaChuong
                        };
                        return Ok(new { StatusCode = StatusCodes.Status200OK, data = responseData });

                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        // PUT: api/Chuongtruyens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("CapNhapChuongTruyen")]
        public async Task<ActionResult> PutChuongtruyen(int id,[FromBody] ChuongtruyenCapNhapDto chuongtruyen)
        {
            var thongTin = _context.Chuongtruyens.FirstOrDefault(item => item.MaChuong == id);
            if(thongTin == null)
            {
                return NotFound();
            }

            else
            {
                try
                {
                 
                
                    thongTin.TenChuong = chuongtruyen.TenChuong;
                    thongTin.GiaChuong = chuongtruyen.GiaChuong;
                        thongTin.NoiDung = chuongtruyen.NoiDung;
                    thongTin.TrangThai = 0;
                       
                        _context.Update(thongTin);
                        await _context.SaveChangesAsync();
                        return Ok(new { Status = StatusCodes.Status200OK, data = chuongtruyen });
                    
                   
                }
                catch (DbUpdateConcurrencyException)
                {

                    return BadRequest();
                }

             
            }


           
        }

        // POST: api/Chuongtruyens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Chuongtruyen>> PostChuongtruyen([FromBody]ChuongtruyenDto chuongtruyenDto)
        {

            try
            {
                var maxStt = await _context.Chuongtruyens
                                              .Where(d => d.MaTruyen == chuongtruyenDto.MaTruyen)
                                              .MaxAsync(d => (int?)d.Stt) ?? 0;
                var chuongtruyen = new Chuongtruyen
                {
                    TenChuong = chuongtruyenDto.TenChuong,
                    TrangThai = 0,
                    NoiDung = chuongtruyenDto.NoiDung,
                    HienThi = 1,
                    GiaChuong = chuongtruyenDto.GiaChuong,
                    LuotDoc = 0,
                    MaTruyen = chuongtruyenDto.MaTruyen,
                    Stt = maxStt + 1
                };

                _context.Chuongtruyens.Add(chuongtruyen);
                await _context.SaveChangesAsync();
                var response = new
                {
                    Status = StatusCodes.Status201Created,
                    Message = "Thêm chương truyện mới",
                    Data = chuongtruyenDto
                };
                return Created("Themtruyen", response);
            }
            catch(Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, data = chuongtruyenDto });

            }

        }

        // DELETE: api/Chuongtruyens/5
        [HttpDelete("XoaChuongTruyen")]
        public async Task<IActionResult> DeleteChuongtruyen(int id)
        {
            var chuongtruyen = await _context.Chuongtruyens.FindAsync(id);
            if (chuongtruyen == null)
            {
                return NotFound();
            }
            chuongtruyen.TrangThai = 3;
            _context.Chuongtruyens.Update(chuongtruyen);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = StatusCodes.Status204NoContent,
                message = "Xoa thanh cong"
            });
        }
        [HttpPut("AnChuongTruyen")]
        public async Task<IActionResult> AnChuongTruyen(int id)
        {
            var chuongtruyen = await _context.Chuongtruyens.FindAsync(id);
            if (chuongtruyen == null)
            {
                return NotFound();
            }
            chuongtruyen.HienThi = 0;
            _context.Chuongtruyens.Update(chuongtruyen);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message ="An thanh cong"
            });
        }
        [HttpPut("HienChuong")]
        public async Task<IActionResult> HienChuong(int id)
        {
            var chuongtruyen = await _context.Chuongtruyens.FindAsync(id);
            if (chuongtruyen == null)
            {
                return NotFound();
            }
            chuongtruyen.HienThi = 1;
            _context.Chuongtruyens.Update(chuongtruyen);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "An thanh cong"
            });
        }
        [HttpPut("KhoaChuongTruyen")]
        public async Task<IActionResult> KhoaChuongTruyen(int id)
        {
            var chuongtruyen = await _context.Chuongtruyens.FindAsync(id);
            if (chuongtruyen == null)
            {
                return NotFound();
            }
            chuongtruyen.TrangThai = 4;
            _context.Chuongtruyens.Update(chuongtruyen);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "An thanh cong"
            });
        }
        [HttpPut("BoKhoaChuong")]
        public async Task<IActionResult> BoKhoaChuong(int id)
        {
            var chuongtruyen = await _context.Chuongtruyens.FindAsync(id);
            if (chuongtruyen == null)
            {
                return NotFound();
            }
            chuongtruyen.TrangThai = 1;
            _context.Chuongtruyens.Update(chuongtruyen);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "An thanh cong"
            });
        }
        [HttpGet("timkiemchuong", Name = "timkiemchuong")]
        public async Task<ActionResult> GetTruyen(string? tenChuong,int? maTruyen)
        {
            IQueryable<Chuongtruyen> query = _context.Chuongtruyens
                                              .Where(u => u.TrangThai != 0  && u.MaTruyen == maTruyen);

            if (!string.IsNullOrEmpty(tenChuong))
            {
                query = query.Where(u => u.TenChuong.Contains(tenChuong));
            }

            if (maTruyen.HasValue)
            {
                query = query.Where(u => u.MaTruyen == maTruyen);
            }

            var taiKhoan = await query.ToListAsync();

            if (taiKhoan.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }

            var responseData = taiKhoan.Select(u => new
            {
                MaChuongTruyen = u.MaChuong,
                TenChuongTruyen = u.TenChuong,
                TrangThai = u.TrangThai,
                HienThi = u.HienThi,
                GiaChuong = u.GiaChuong,
                LuotDoc = u.LuotDoc,
                NgayTao = u.Ngaytao,
                NgayCapNhat = u.NgayCapNhap,
                stt = u.Stt
            }).ToList();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = responseData
            });
        }
        [HttpPost("Themluotdoc")]
        public async Task<IActionResult> ThemLuotDoc(int id)
        {
            var chuongtruyen = await _context.Chuongtruyens.FindAsync(id);
            if (chuongtruyen == null)
            {
                return NotFound();
            }
            chuongtruyen.LuotDoc += 1;
            _context.Chuongtruyens.Update(chuongtruyen);
            await _context.SaveChangesAsync();

            return Ok();
        }
        private bool ChuongtruyenExists(int id)
        {
            return _context.Chuongtruyens.Any(e => e.MaChuong == id);
        }

        [HttpPost("DocChuongTruyen")]
        public async Task<IActionResult> DocChuongTruyen([FromBody] ChuongtruyenNoiDung chuongtruyenNoiDung)
        {
            try
            {
                var audioBytes = await _ttsService.SynthesizeTextToSpeechAsync(chuongtruyenNoiDung.NoiDung);
                return File(audioBytes, "audio/mpeg", "output.mp3");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("DanhSachChuongTruyenCanDuyet")]

        public async Task<IActionResult> DanhSachChuongTruyenCanDuyet()
        {
           
            var dsChuong =_context.Chuongtruyens.Include(u => u.MaTruyenNavigation).ThenInclude(u => u.MaButDanhNavigation).Where(item=>  item.TrangThai ==0 && item.MaChuong !=22).ToList();
            if(dsChuong.Count == 0)
            {
                return NotFound(new
                {
                    status = StatusCodes.Status404NotFound,
                    message = "Không có chương",
                });
            }
            var responseData = dsChuong.Select(u => new
            {
                Machuongtruyen = u.MaChuong,
                TenChuong = u.TenChuong,
                TrangThai = u.TrangThai,
                Luotdoc = u.LuotDoc,
                HienThi = u.HienThi,
                GiaChuong = u.GiaChuong,
                Stt = u.Stt,
                NgayTao = u.Ngaytao,
                NgayCapNhat = u.NgayCapNhap,
                 TenTruyen =u.MaTruyenNavigation.TenTruyen,
                 tenButdanh = u.MaTruyenNavigation.MaButDanhNavigation.TenButDanh
            }).ToList();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Danh sách chương",
                data = responseData 
            });
        }
        [HttpPut("DuyetChuong")]
        public async Task<ActionResult> DuyetChuong(int maChuong)
        {
            var taiKhoan = _context.Chuongtruyens.FirstOrDefault(item => item.MaChuong == maChuong);
            if (taiKhoan == null)
            {
                return NotFound(new
                {
                    status = StatusCodes.Status404NotFound,
                    message = "Không có truyện",
                });
            }
            taiKhoan.TrangThai = 1;
            _context.Chuongtruyens.Update(taiKhoan);
            _context.SaveChanges();
            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Thành công",
            });
        }

        [HttpGet("timkiemChuongTheoTenTruyen", Name = "timkiemChuongTheoTenTruyen")]
        public async Task<ActionResult> timkiemChuongTheoTenTruyen(string? tenTruyen)
        {
            IQueryable<Truyen> query = _context.Truyens
                                               .Include(u => u.MaButDanhNavigation)
                                               .Include(u => u.MaTheLoaiNavigation);

            if (!string.IsNullOrEmpty(tenTruyen))
            {
                query = query.Where(u => u.TenTruyen.Contains(tenTruyen));
            }


            var maTruyens = await query.Select(q => q.MaTruyen).ToListAsync();

            var chuongTruyenCanDuyet = await _context.Chuongtruyens.Include(u => u.MaTruyenNavigation).ThenInclude(u => u.MaButDanhNavigation)
                                                     .Where(item => maTruyens.Contains(item.MaTruyen) && item.TrangThai == 0 && item.MaChuong != 22)
                                                     .ToListAsync();

            if (chuongTruyenCanDuyet.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }

            var responseData = chuongTruyenCanDuyet.Select(u => new
            {
                Machuongtruyen = u.MaChuong,
                TenChuong = u.TenChuong,
                TrangThai = u.TrangThai,
                Luotdoc = u.LuotDoc,
                HienThi = u.HienThi,
                GiaChuong = u.GiaChuong,
                Stt = u.Stt,
                NgayTao = u.Ngaytao,
                NgayCapNhat = u.NgayCapNhap,
                TenTruyen = u.MaTruyenNavigation.TenTruyen,
                tenButdanh = u.MaTruyenNavigation.MaButDanhNavigation.TenButDanh
            }).ToList();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = responseData
            });
        }

    }
}

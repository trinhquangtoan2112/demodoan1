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
          
            var taiKhoan = await _context.Chuongtruyens.Where(item =>item.MaTruyen ==maTruyen && item.TrangThai!=0).ToListAsync();
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

        // GET: api/Chuongtruyens/5
        [HttpGet("GetChiTietChuong")]
        public async Task<ActionResult<Chuongtruyen>> GetChuongtruyen([FromQuery] int maChuong, [FromQuery] string? token)
        {
            try
            {
                var currentChapter = _context.Chuongtruyens.FirstOrDefault(item => item.MaChuong == maChuong &&item.TrangThai !=0);

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
                    if (currentChapter.GiaChuong == 0)
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
                            trangTiep = nextChapter?.MaChuong
                        };
                        return Ok(new { StatusCode = StatusCodes.Status200OK, data = responseData });
                    }
                    else
                    {
                        string tokenData = null;
                        var responseData1 = new
                        {


                            trangTruoc = previousChapter?.MaChuong,
                            trangTiep = nextChapter?.MaChuong
                        };
                        if (!string.IsNullOrEmpty(token))
                        {
                            tokenData = TokenClass.Decodejwt(token);
                            Boolean daMua = tokenData != null ? _context.Giaodiches.Any(g => g.MaChuongTruyen == maChuong && g.MaNguoiDung == Int64.Parse(tokenData)) : false;
                          
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
                                NgayTao = currentChapter.Ngaytao,
                                NgayCapNhat = currentChapter.NgayCapNhap,
                              
                                  trangTruoc = previousChapter?.MaChuong,
                                trangTiep = nextChapter?.MaChuong
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
                    GiaChuong = 0,
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
    }
}

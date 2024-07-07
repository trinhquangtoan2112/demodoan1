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

namespace demodoan1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChuongtruyensController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public ChuongtruyensController(DbDoAnTotNghiepContext context)
        {
            _context = context;
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
          
            var taiKhoan = await _context.Chuongtruyens.Where(item =>item.MaTruyen ==maTruyen).ToListAsync();
            if (taiKhoan.Count == 0)
            {
                return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy" });
            }
           
            var responseData = taiKhoan.Select(u => new
            {
                Machuongtruyen = u.MaChuong,
                TenChuong = u.TenChuong,
                TrangThai = u.TrangThai,
                NoiDung = u.NoiDung,
                HienThi = u.HienThi,
                GiaChuong = u.GiaChuong,
                LuotDoc =u.LuotDoc,
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
                    if (currentChapter.GiaChuong == 0)
                    {
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
                            trangTiep = hasNextChapter
                        };
                        return Ok(new { StatusCode = StatusCodes.Status200OK, data = responseData });
                    }
                    else
                    {
                        string tokenData = null;
                        if (!string.IsNullOrEmpty(token))
                        {
                            tokenData = TokenClass.Decodejwt(token);
                            Boolean daMua = tokenData != null ? _context.Giaodiches.Any(g => g.MaChuongTruyen == maChuong && g.MaNguoiDung == Int64.Parse(tokenData)) : false;
                            if (!daMua)
                            {
                                return Unauthorized();
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
                                trangTiep = hasNextChapter
                            };
                            return Ok(new
                            {
                                StatusCodes.Status200OK,
                                Ddata = responseData
                            });
                        }
                        return Unauthorized();
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
        [HttpPut("{id}")]
        public async Task<ActionResult> PutChuongtruyen(int id, ChuongtruyenDto chuongtruyenDto)
        {
          

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChuongtruyenExists(id))
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
                    TrangThai = 1,
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
                return Created("Them truyen thành công", response);
            }
            catch(Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, data = chuongtruyenDto });

            }

        }

        // DELETE: api/Chuongtruyens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChuongtruyen(int id)
        {
            var chuongtruyen = await _context.Chuongtruyens.FindAsync(id);
            if (chuongtruyen == null)
            {
                return NotFound();
            }

            _context.Chuongtruyens.Remove(chuongtruyen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChuongtruyenExists(int id)
        {
            return _context.Chuongtruyens.Any(e => e.MaChuong == id);
        }
    }
}

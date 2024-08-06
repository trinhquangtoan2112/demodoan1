using demodoan1.Helpers;
using demodoan1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ZstdSharp.Unsafe;

namespace demodoan1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LichSuDocController : Controller
    {
        private readonly DbDoAnTotNghiepContext _context;

        public LichSuDocController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }
        [HttpGet("LichSuDocTheoTruyen")]
        public async Task<ActionResult> GetLichSuDocTruyen(String? token, int maTruyen)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { Status = StatusCodes.Status400BadRequest, message = "Token không hợp lệ" });
            }
            var truyen = _context.Truyens.FirstOrDefault(item => item.MaTruyen == maTruyen);
            if (truyen == null)
            {
                return NotFound();
            }


            string tokenData = TokenClass.Decodejwt(token);
            var dsChuong = _context.Chuongtruyens.Where(item => item.MaTruyen == maTruyen).Select(c => c.MaChuong).ToList();
            var lichSuDoc = _context.Lichsudocs.FirstOrDefault(item => item.MaNguoiDung == Int64.Parse(tokenData) && dsChuong.Contains(item.MaChuongTruyen));
            if (lichSuDoc == null)
            {
                return NotFound();
            }
            return Ok(new { Status = StatusCodes.Status200OK, data = lichSuDoc }); ;
        }
        [HttpPost("CapNhapLichSuDoc")]
        public async Task<ActionResult> CapNhapLichSuDoc(String? token, int maChuong)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { Status = StatusCodes.Status400BadRequest, message = "Token không hợp lệ" });
            }

            string tokenData = TokenClass.Decodejwt(token);
            var data = _context.Lichsudocs.Where(item => item.MaNguoiDung == Int64.Parse(tokenData));
            var checkMaChuongTonTai = _context.Chuongtruyens.FirstOrDefault(item => item.MaChuong == maChuong);

            if (data == null || checkMaChuongTonTai == null)
            {
                return BadRequest();
            }
            var lichSuDoc = _context.Lichsudocs.FirstOrDefault(item => item.MaNguoiDung == Int64.Parse(tokenData) && item.MaChuongTruyen == maChuong);
            if (lichSuDoc != null)
            {
                return Ok();
            }
            else
            {

                var layDanhSachMaChuong = _context.Chuongtruyens.Where(item => item.MaTruyen == checkMaChuongTonTai.MaTruyen).Select(item => item.MaChuong).ToList();
                var checkChuongTruyen = _context.Lichsudocs.FirstOrDefault(item => layDanhSachMaChuong.Contains(item.MaChuongTruyen) && item.MaNguoiDung == Int64.Parse(tokenData));

                if (checkChuongTruyen != null)
                {

                    _context.Lichsudocs.Remove(checkChuongTruyen);
                    _context.SaveChanges();
                    var newHistory1 = new Lichsudoc
                    {
                        MaNguoiDung = (int)Int64.Parse(tokenData),
                        MaChuongTruyen = maChuong,
                       
                    };
                    _context.Lichsudocs.Add(newHistory1);
                    _context.SaveChanges();
                    return Ok();

                }


                var newHistory = new Lichsudoc
                {
                    MaNguoiDung = (int)Int64.Parse(tokenData),
                    MaChuongTruyen = maChuong,
                  
                };
                _context.Lichsudocs.Add(newHistory);
                _context.SaveChanges();
                return Ok();
            }

        }
        [HttpGet("DanhSachLichSuDoc")]
        public async Task<ActionResult> DanhSachLichSuDoc(String? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { Status = StatusCodes.Status400BadRequest, message = "Token không hợp lệ" });
            }
            string tokenData = TokenClass.Decodejwt(token);
            var lichSuDoc = _context.Lichsudocs.Include(u => u.MaChuongTruyenNavigation).ThenInclude(u => u.MaTruyenNavigation).Where(item => item.MaNguoiDung == Int64.Parse(tokenData)).ToList();

            if (lichSuDoc.Count == 0)
            {

                return NotFound();
            }

            var danhSach = lichSuDoc.Select(item => new
            {
                idMaChuong = item.MaChuongTruyen,
                tenTruyen = item.MaChuongTruyenNavigation.MaTruyenNavigation.TenTruyen,
                anhBia = item.MaChuongTruyenNavigation.MaTruyenNavigation.AnhBia,
                tenChuong = item.MaChuongTruyenNavigation.TenChuong,

            }).ToList();
            return Ok(new { Status = StatusCodes.Status200OK, data = danhSach });
        }
        [HttpDelete("XoaLichSuDoc")]
        public async Task<IActionResult> XoaLichSuDoc(String token, int maChuong)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { Status = StatusCodes.Status400BadRequest, message = "Token không hợp lệ" });
            }

            string tokenData = TokenClass.Decodejwt(token);
            var data = _context.Lichsudocs.FirstOrDefault(item => item.MaNguoiDung == Int64.Parse(tokenData) && item.MaChuongTruyen == maChuong);


            if (data == null)
            {
                return BadRequest();
            }
            else
            {
                _context.Lichsudocs.Remove(data);
                _context.SaveChanges();
                return Ok(new { status = StatusCodes.Status204NoContent, message = "Xoa lich su thanh cong" });
            }
        }



        [HttpPost("CapNhapLichSuDocViTri")]
        public async Task<ActionResult> CapNhapLichSuDocViTri(String? token, int maChuong,int viTri)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { Status = StatusCodes.Status400BadRequest, message = "Token không hợp lệ" });
            }

            string tokenData = TokenClass.Decodejwt(token);
            var data = _context.Lichsudocs.Where(item => item.MaNguoiDung == Int64.Parse(tokenData));
            var checkMaChuongTonTai = _context.Chuongtruyens.FirstOrDefault(item => item.MaChuong == maChuong);

             if(checkMaChuongTonTai == null)
            {
                return BadRequest();
            }

            var lichsuDocCuaChuong = _context.Lichsudocs.FirstOrDefault(item => item.MaChuongTruyen == maChuong && item.MaNguoiDung == Int64.Parse(tokenData));
            if(lichsuDocCuaChuong == null)
            {
                return BadRequest();
            }
            else
            {
                lichsuDocCuaChuong.ViTri = viTri;
                _context.Lichsudocs.Update(lichsuDocCuaChuong);
                _context.SaveChanges();
                return Ok();
            }


        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.BaocaoDto;
using demodoan1.Helpers;
using demodoan1.Models.DanhgiaDto;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaocaosController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public BaocaosController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        [HttpGet("LayDSBaocao")]
        public async Task<ActionResult<IEnumerable<LayDSBaocao>>> GetBaocaos()
        {
            try
            {
                var dsbaocao = _context.Baocaos.Select(d => new LayDSBaocao
                {
                    MaBaoCao = d.MaBaoCao,
                    Loaibaocao = d.Loaibaocao,
                    Trangthai = d.Trangthai,
                    Tieude = d.Tieude,
                    Noidung = d.Noidung,
                    Ngaytao = d.Ngaytao,
                    NguoiBaoCao = d.MaNguoiDungNavigation.Email,
                    NoiDungBibaocao = d.Loaibaocao == 1
                        ? _context.Danhgia
                            .Where(dg => dg.MaDanhGia == d.MaThucThe)
                            .Select(dg => dg.Noidung)
                            .FirstOrDefault() ?? "Đối tượng bị báo cáo không tồn tại"
                        : d.Loaibaocao == 2
                            ? _context.Binhluans
                                .Where(bl => bl.MabinhLuan == d.MaThucThe)
                                .Select(bl => bl.Noidung)
                                .FirstOrDefault() ?? "Đối tượng bị báo cáo không tồn tại"
                            : d.Loaibaocao == 3
                                ? _context.Phanhoibinhluans
                                    .Where(ph => ph.MaPhanHoiBinhLuan == d.MaThucThe)
                                    .Select(ph => ph.Noidung)
                                    .FirstOrDefault() ?? "Đối tượng bị báo cáo không tồn tại"
                                : d.Loaibaocao == 4
                                    ? _context.Truyens
                                        .Where(t => t.MaTruyen == d.MaThucThe)
                                        .Select(t => t.TenTruyen)
                                        .FirstOrDefault() ?? "Đối tượng bị báo cáo không tồn tại"
                                    : d.Loaibaocao == 5
                                        ? _context.Chuongtruyens
                                            .Where(ct => ct.MaChuong == d.MaThucThe)
                                            .Select(ct => new
                                            {
                                                ct.TenChuong,
                                                ct.MaTruyenNavigation.TenTruyen,
                                                ct.Stt
                                            })
                                            .FirstOrDefault() == null
                                            ? "Đối tượng bị báo cáo không tồn tại"
                                            : _context.Chuongtruyens
                                                .Where(ct => ct.MaChuong == d.MaThucThe)
                                                .Select(ct => new
                                                {
                                                    ChuongInfo = $"Tên chương: {ct.TenChuong} - Tên truyện {ct.MaTruyenNavigation.TenTruyen} - Số chương: {ct.Stt}"
                                                })
                                                .FirstOrDefault().ChuongInfo
                                        : "Không tìm thấy loại báo cáo phù hợp"
                }).ToList();

                return Ok(new { status = StatusCodes.Status200OK, message = "Thành công", data = dsbaocao });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }
        
        // POST: api/Baocaos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Baocao>> PostBaocao(BaocaoDto baocaoDto,string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];

                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);
                var baocao = new Baocao
                {
                    Tieude = baocaoDto.Tieude,
                    Loaibaocao = baocaoDto.Loaibaocao,
                    Noidung = baocaoDto.Noidung,
                    MaThucThe = baocaoDto.MaThucThe,
                    MaNguoiDung = maNguoiDung,
                    Trangthai = 0,
                };

                _context.Baocaos.Add(baocao);
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status201Created, data ="Thêm thành công" });
            }   
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Lỗi xảy ra." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBaocao(int id,string token)
        {
            // Validate token and extract user information
            try
            {
                token = token.Trim();
                var data = token.Substring(7); // Remove "Bearer " prefix
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                // Check if user has admin privileges
                var user = await _context.Users.FindAsync(maNguoiDung);
                if (user == null || user.MaQuyen != 1) // Not admin
                {
                    return Unauthorized(new { status = StatusCodes.Status401Unauthorized, message = "Bạn không có quyền xóa báo cáo." });
                }

                // Find the report to delete
                var baocao = await _context.Baocaos.FindAsync(id);
                if (baocao == null)
                {
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Báo cáo không tồn tại." });
                }

                // Remove the report from the database
                _context.Baocaos.Remove(baocao);
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status200OK, message = "Xóa thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }


        [HttpPut("SuaBaoCao")]
        public async Task<IActionResult> UpdateBaoCao(SuaBaoBaocao suaBaoBaocaoDto, string token)
        {
            try
            {
                // Xác thực và lấy thông tin người dùng từ token
                token = token.Trim();
                var data = token.Substring(7); // Bỏ qua phần "Bearer "
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];

                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                // Lấy thông tin người dùng từ cơ sở dữ liệu
                var user = await _context.Users.FindAsync(maNguoiDung);
                if (user == null || user.MaQuyen != 1) // Không có quyền hoặc không phải admin
                {
                    return Unauthorized(new { status = StatusCodes.Status401Unauthorized, message = "Không có quyền" });
                }

                // Tìm báo cáo cần sửa
                var baocao = await _context.Baocaos.FindAsync(suaBaoBaocaoDto.MaBaoCao);
                if (baocao == null)
                {
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Báo cáo không tồn tại" });
                }

                if (baocao.Trangthai == 1)
                {
                    return Ok(new { status = StatusCodes.Status200OK, message = "Báo cáo đã được xử lý" });
                }

                // Cập nhật trạng thái báo cáo
                baocao.Trangthai = suaBaoBaocaoDto.Trangthai ?? baocao.Trangthai;



                _context.Entry(baocao).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                

                return Ok(new { status = StatusCodes.Status200OK, message = "Cập nhật thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = $"Lỗi: {ex.Message}" });
            }
        }


        private bool BaocaoExists(int id)
        {
            return _context.Baocaos.Any(e => e.MaBaoCao == id);
        }
    }
}

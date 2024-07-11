using demodoan1.Helpers;
using demodoan1.Models;
using demodoan1.Models.ButdanhDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ButdanhsController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public ButdanhsController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Butdanhs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Butdanh>>> GetButdanhs()
        {
            return await _context.Butdanhs.ToListAsync();
        }

        // GET: api/Butdanhs/5
        [HttpGet("DanhSachButDanhTheoNguoiDung")]
        public async Task<ActionResult<Butdanh>> GetButdanh(String token )
        {
            token = token.Trim();
            var data = token.Substring(7);
            Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
            string iDNguoiDung = claimsData["IdUserName"];
            var danhSachButDanh = _context.Butdanhs.Where(item =>item.MaNguoiDung ==Int64.Parse(iDNguoiDung)).ToList();

            if (danhSachButDanh == null)
            {
                return NotFound();
            }

            return Ok(new {status = StatusCodes.Status200OK, data= danhSachButDanh });
        }

        // PUT: api/Butdanhs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("SuaButDanh")]
        public async Task<IActionResult> PutButdanh([FromBody] SuaButdanhDto butdanhDto, [FromQuery] string token)
        {
            try
            {
               
                token = token.Trim();
                var data = token.Substring(7);
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                var checkTonTai = _context.Butdanhs.FirstOrDefault(item => item.MaNguoiDung == Int64.Parse(iDNguoiDung) && item.MaButDanh == butdanhDto.MaButDanh);
                if (checkTonTai == null)
                {
                    return Unauthorized(new {status =StatusCodes.Status401Unauthorized, message = "Không Có quyền" });
                }
                checkTonTai.TenButDanh = butdanhDto.TenButDanh;

                _context.Butdanhs.Update(checkTonTai);
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status201Created, data = butdanhDto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "lỗi" });
            }
        }

        // POST: api/Butdanhs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Thembutdanh")]
        public async Task<ActionResult<Butdanh>> PostButdanh([FromBody] ButdanhDto butdanhDto, [FromQuery] string token)
        {
            try
            {
                Boolean daCo = _context.Butdanhs.Any(item => item.TenButDanh.Equals(butdanhDto.TenButDanh));
                if(daCo)
                     return BadRequest(new { status = StatusCodes.Status400BadRequest,message ="Đã tồn tại" });
                token = token.Trim();
                var data = token.Substring(7);
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                var butdanh = new Butdanh
                {

                    TenButDanh = butdanhDto.TenButDanh,
                    MaNguoiDung = (int)Int64.Parse(iDNguoiDung),
                    Trangthai = 0
                };

                _context.Butdanhs.Add(butdanh);
                await _context.SaveChangesAsync();

                return Ok( new { status = StatusCodes.Status201Created,data = butdanhDto } );
            }
            catch (Exception ex)
            {
                return BadRequest( new { status = StatusCodes.Status400BadRequest, message ="lỗi" });
            }
           
        }

        // DELETE: api/Butdanhs/5
        [HttpDelete("XoaButDanh")]
        public async Task<IActionResult> DeleteButdanh([FromBody] ButdanhDtoKhoa butdanhdto, string token)
        {
            try
            {

                token = token.Trim();
                var data = token.Substring(7);
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                var checkTonTai = _context.Butdanhs.FirstOrDefault(item => item.MaNguoiDung == Int64.Parse(iDNguoiDung) && item.MaButDanh == butdanhdto.MaButDanh);
                if (checkTonTai == null)
                {
                    return Unauthorized(new { status = StatusCodes.Status401Unauthorized, message = "Không Có quyền" });
                }
                checkTonTai.Trangthai = 1;

                _context.Butdanhs.Update(checkTonTai);
                await _context.SaveChangesAsync();

                return Accepted(new { status = StatusCodes.Status202Accepted, message = "Thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "lỗi" });
            }
        }

        // GET: api/Butdanhs/laybutdanhcuaadmin
        [HttpGet("laybutdanhcuaadmin")]
        public async Task<ActionResult<IEnumerable<LayButDanhAdminDto>>> GetButDanhCuaAdmin()
        {
            try
            {
                var danhSachButDanh = await _context.Butdanhs
                    .Select(bd => new LayButDanhAdminDto
                    {
                        MaButDanh = bd.MaButDanh,
                        TenButDanh = bd.TenButDanh,
                        EmailNguoiDung = _context.Users
                        .Where(n => n.MaNguoiDung == bd.MaNguoiDung)
                        .Select(n => n.Email)
                        .FirstOrDefault(),
                        Trangthai = bd.Trangthai,
                        Ngaytao = bd.Ngaytao,
                        SoLuongTruyen = _context.Truyens.Count(t => t.MaButDanh == bd.MaButDanh)
                    })
                    .ToListAsync();

                if (danhSachButDanh == null || !danhSachButDanh.Any())
                {
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy bút danh nào" });
                }

                return Ok(new { status = StatusCodes.Status200OK, data = danhSachButDanh });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = ex.Message });
            }
        }

        [HttpPut("KhoaButDanh")]
        public async Task<IActionResult> KhoaButDanh([FromBody] ButdanhDtoKhoa butdanhDtoKhoa, [FromQuery] string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7);
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];

                // Kiểm tra quyền của người dùng
                var user = await _context.Users.FirstOrDefaultAsync(u => u.MaNguoiDung == Int64.Parse(iDNguoiDung));
                if (user == null || user.MaQuyen != 1) // Không có quyền hoặc không phải admin
                {
                    return Unauthorized(new { status = StatusCodes.Status401Unauthorized, message = "Không có quyền" });
                }

                // Tìm bút danh để khóa
                var butDanh = await _context.Butdanhs.FirstOrDefaultAsync(bd => bd.MaButDanh == butdanhDtoKhoa.MaButDanh);
                if (butDanh == null)
                {
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy bút danh" });
                }

                // Cập nhật trạng thái bút danh
                butDanh.Trangthai = 1; // Khóa

                _context.Butdanhs.Update(butDanh);
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status200OK, message = "Khóa bút danh thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = ex.Message });
            }
        }

        // DELETE: api/Butdanhs/XoaButDanhCuaAdmin
        [HttpDelete("XoaButDanhCuaAdmin")]
        public async Task<IActionResult> XoaButDanhCuaAdmin([FromBody] ButdanhDtoKhoa butdanhDtoKhoa, [FromQuery] string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7);
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];

                // Kiểm tra quyền của người dùng
                var user = await _context.Users.FirstOrDefaultAsync(u => u.MaNguoiDung == Int64.Parse(iDNguoiDung));
                if (user == null || user.MaQuyen != 1) // Không có quyền hoặc không phải admin
                {
                    return Unauthorized(new { status = StatusCodes.Status401Unauthorized, message = "Không có quyền" });
                }

                // Tìm bút danh để xóa
                var butDanh = await _context.Butdanhs.FirstOrDefaultAsync(bd => bd.MaButDanh == butdanhDtoKhoa.MaButDanh);
                if (butDanh == null)
                {
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy bút danh" });
                }

                // Xóa bút danh khỏi danh sách
                _context.Butdanhs.Remove(butDanh);
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status200OK, message = "Xóa bút danh thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = ex.Message });
            }
        }


        private bool ButdanhExists(int id)
        {
            return _context.Butdanhs.Any(e => e.MaButDanh == id);
        }
    }
}

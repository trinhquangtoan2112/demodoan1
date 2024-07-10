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

        private bool ButdanhExists(int id)
        {
            return _context.Butdanhs.Any(e => e.MaButDanh == id);
        }
    }
}

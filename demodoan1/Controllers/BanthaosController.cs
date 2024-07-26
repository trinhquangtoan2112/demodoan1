using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.BanthaoDto;
using demodoan1.Helpers;
using demodoan1.Models.ButdanhDto;
using NuGet.Common;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanthaosController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public BanthaosController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Banthaos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Banthao>>> GetBanthaos(String token)
        {
            try
            {
                if (String.IsNullOrEmpty(token))
                {
                    return NotFound(new { status = StatusCodes.Status400BadRequest, message = "Không tìm thấy" });
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
                    .Where(item => maButDanhList.Contains(item.MaButDanh))
                    .Select(u => new
                    {
                        MaTruyen = u.MaTruyen,
                        TenTruyen = u.TenTruyen,
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
                return Ok(new {StatusCodes.Status200OK, data = dsTruyen });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

      
        [HttpGet("LayChiTietBanThao")]
        public async Task<ActionResult<Banthao>> GetBanthao(int id)
        {
            var banthao = await _context.Banthaos.FindAsync(id);

            if (banthao == null)
            {
                return NotFound();
            }

            return Ok(new { status = StatusCodes.Status200OK, Data = banthao });
        }
        [HttpGet("DanhSachBanThaoTheoTruyen")]
        public async Task<ActionResult<Banthao>> DanhSachBanThaoTheoTruyen(int id)
        {
            var banthao =  _context.Banthaos.Where(item => item.MaTruyen ==id).ToList();

            if (banthao == null)
            {
                return NotFound();
            }

            return Ok(new {Status =StatusCodes.Status200OK, data = banthao });
        }
        // PUT: api/Banthaos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> PutBanthao(int id, CapNhapBanthaoDto banthaoDto)
        {
            try
            {
                var banThao = _context.Banthaos.FirstOrDefault(item => item.MaBanThao == id);
                if(banThao == null)
                {
                    return NotFound();
                }

                banThao.Noidung = banthaoDto.Noidung;
                banThao.TenBanThao = banthaoDto.TenBanThao;
                 
               

                _context.Banthaos.Update(banThao);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: api/Banthaos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Banthao>> PostBanthao(BanthaoDto banthaoDto)
        {
            try
            {
                var truyen = _context.Truyens.FirstOrDefault(item => item.MaTruyen == banthaoDto.MaTruyen);
                if(truyen == null)
                {
                    return NotFound();
                }
                var banThao = new Banthao
                {
                    Noidung = banthaoDto.Noidung,
                    TenBanThao = banthaoDto.TenBanThao,
                    MaTruyen = banthaoDto.MaTruyen

                };

                _context.Banthaos.Add(banThao);
                await _context.SaveChangesAsync();

                return Ok(new {data=banthaoDto,status =StatusCodes.Status200OK});
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        // DELETE: api/Banthaos/5
        [HttpDelete()]
        public async Task<IActionResult> DeleteBanthao(int id)
        {
            var banthao =  _context.Banthaos.FirstOrDefault( item => item.MaBanThao ==id);
            if (banthao == null)
            {
                return NotFound();
            }

            _context.Banthaos.Remove(banthao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BanthaoExists(int id)
        {
            return _context.Banthaos.Any(e => e.MaBanThao == id);
        }
    }
}

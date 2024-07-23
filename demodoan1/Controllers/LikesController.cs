using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demodoan1.Models;
using demodoan1.Models.LikeDto;
using demodoan1.Helpers;

namespace demodoan1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly DbDoAnTotNghiepContext _context;

        public LikesController(DbDoAnTotNghiepContext context)
        {
            _context = context;
        }

        // GET: api/Likes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikes()
        {
            return await _context.Likes.ToListAsync();
        }

        // POST: api/Likes
        [HttpPost]
        public async Task<ActionResult<Like>> PostLike(LikeDto likeDto, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7);
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                var like = new Like
                {
                    LoaiThucTheLike = likeDto.LoaiThucTheLike,
                    MaThucThe = likeDto.MaThucThe,
                    MaNguoiDung = maNguoiDung,
                };

                _context.Likes.Add(like);
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status201Created, data = "Thêm thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Lỗi xảy ra." });
            }
        }

        // DELETE: api/Likes/5
        [HttpDelete]
        public async Task<IActionResult> DeleteLike(LikeDto likeDto, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7);
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                int maNguoiDung = (int)Int64.Parse(iDNguoiDung);

                var likes = await _context.Likes
                    .Where(l => l.LoaiThucTheLike == likeDto.LoaiThucTheLike && l.MaThucThe == likeDto.MaThucThe && l.MaNguoiDung == maNguoiDung)
                    .ToListAsync();

                if (likes.Count == 0)
                {
                    return NotFound(new { status = StatusCodes.Status404NotFound, message = "Không tìm thấy bản ghi nào phù hợp." });
                }

                _context.Likes.RemoveRange(likes);
                await _context.SaveChangesAsync();

                return Ok(new { status = StatusCodes.Status200OK, message = "Xóa thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "Lỗi xảy ra." });
            }
        }

        [HttpPost("CheckMultipleLikes")]
        public async Task<ActionResult<Dictionary<int, bool>>> CheckMultipleLikes(LikeCheckRequest request, string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7);
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                int maNguoiDungFromToken = (int)Int64.Parse(iDNguoiDung);

                var likes = await _context.Likes
                    .Where(l => l.LoaiThucTheLike == request.LoaiThucTheLike
                                && l.MaNguoiDung == maNguoiDungFromToken
                                && request.MaThucThes.Contains(l.MaThucThe.Value))
                    .ToListAsync();

                var result = request.MaThucThes.ToDictionary(id => id, id => likes.Any(l => l.MaThucThe == id));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = StatusCodes.Status400BadRequest, message = "An error occurred while processing your request." });
            }
        }
    }
}

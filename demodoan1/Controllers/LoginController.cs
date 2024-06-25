﻿using demodoan1.Data;
using demodoan1.Helpers;
using demodoan1.Models;
using demodoan1.Models.UserDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace demodoan1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {

        public readonly DbDoAnTotNghiepContext _appDbContext;
        private readonly AppSetting _appSetting;
        public LoginController(DbDoAnTotNghiepContext appDbContext, IOptions<AppSetting> appSetting)
        {
            _appDbContext = appDbContext;
            _appSetting = appSetting.Value;
        }
        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{

        //    var html11 =await  _appDbContext.User.Include(u => u.Role).ToListAsync();
        //    var responseData = html11.Select(u => new
        //    {
        //        MaNguoiDung = u.MaNguoiDung,
        //        TenNguoiDung = u.TenNguoiDung,
        //        MatKhau = PasswordEncryptDecord.DecodeFrom64(u.MatKhau),
        //        Email = u.Email,
        //        NgaySinh = u.NgaySinh,
        //        GioiTinh = u.GioiTinh,
        //        AnhDaiDien = u.AnhDaiDien,
        //        TrangThai = u.TrangThai,
        //        DaXoa = u.DaXoa,
        //        SoDeCu = u.SoDeCu,
        //        SoXu = u.SoXu,
        //        SoChiaKhoa = u.SoChiaKhoa,
        //        Vip = u.Vip,
        //        NgayHetHanVip = u.NgayHetHanVip,
        //        NgayTao = u.NgayTao,
        //        NgayCapNhap = u.NgayCapNhap,
        //        TenQuyen = u.Role != null ? u.Role.tenQuyen : null

        //    }).ToList();
        //    return Ok(new { Success = 200, data = responseData });
        //}
        //[HttpPost]
        //public async Task<IActionResult> ThemTaiKhoan([FromBody] UserDto user)
        //{
        //    try
        //    {
        //        user.MatKhau = PasswordEncryptDecord.EncodePasswordToBase64(user.MatKhau);
        //        var user1 = new User
        //        {
                   
        //            MatKhau = user.MatKhau,
        //            Email = user.Email,
        //            MaQuyen = 2,
                   
        //        };
        //        _appDbContext.User.Add(user1);
        //        await  _appDbContext.SaveChangesAsync();
        //        return Ok(new { Success = 200, data = user });
        //    }catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpPost("SignUp",Name = "SignUp")]
        public async Task<IActionResult> SignUp([FromBody] UserDto user)
        {
            try
            {
                user.MatKhau = PasswordEncryptDecord.EncodePasswordToBase64(user.MatKhau);
                var user1 = new User
                {

                    MatKhau = user.MatKhau,
                    Email = user.Email,
                    MaQuyen = 1,
                    TrangThai=false,
                    DaXoa=false
                     
                };
                _appDbContext.Users.Add(user1);
                await _appDbContext.SaveChangesAsync();
                return Ok(new { Success = 200, data = user });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Login",Name ="Login")]
        public async Task<IActionResult> Login([FromBody] UserDto user)
        {
            try
            {
                user.MatKhau  = PasswordEncryptDecord.EncodePasswordToBase64(user.MatKhau);
             
                var taiKhoan = await _appDbContext.Users.Include(u => u.MaQuyenNavigation).SingleOrDefaultAsync(u => u.Email == user.Email && u.MatKhau == user.MatKhau);
                var responseData = new
                {
                    MaNguoiDung = taiKhoan.MaNguoiDung,
                    TenNguoiDung = taiKhoan.TenNguoiDung,
                    MatKhau = PasswordEncryptDecord.DecodeFrom64(taiKhoan.MatKhau),
                    Email = taiKhoan.Email,
                    NgaySinh = taiKhoan.NgaySinh,
                    GioiTinh = taiKhoan.GioiTinh,
                    AnhDaiDien = taiKhoan.AnhDaiDien,
                    TrangThai = taiKhoan.TrangThai,
                    DaXoa = taiKhoan.DaXoa,
                    SoDeCu = taiKhoan.SoDeCu,
                    SoXu = taiKhoan.SoXu,
                    SoChiaKhoa = taiKhoan.SoChiaKhoa,
                    Vip = taiKhoan.Vip,
                    NgayHetHanVip = taiKhoan.NgayHetHanVip,
                    NgayTao = taiKhoan.Ngaytao,
                    NgayCapNhap = taiKhoan.NgayCapNhap,
                    TenQuyen = taiKhoan.MaQuyenNavigation != null ? taiKhoan.MaQuyenNavigation.TenQuyen : null

                };
                if (taiKhoan != null)
                {
                    return Ok(new { Success = 200, data = responseData, token = GenerateJwtToken(taiKhoan) });
                }

                return NotFound(new
                {
                    Success = 404,
                    message = "Not found"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private string GenerateJwtToken(User user)
        {
            if (user.MaQuyenNavigation == null)
            {
                throw new ArgumentException("User role is null");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.MaQuyenNavigation.TenQuyen) ,
                    new Claim("IdUserName", user.MaNguoiDung.ToString())// assuming user.Role is not null
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
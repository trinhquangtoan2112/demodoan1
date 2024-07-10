using demodoan1.Data;
using demodoan1.Helpers;
using demodoan1.Models;
using demodoan1.Models.UserDto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection.Metadata.Ecma335;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using demodoan1.Models.TruyenDto;

namespace demodoan1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {

        public readonly DbDoAnTotNghiepContext _appDbContext;
        private readonly AppSetting _appSetting;
        private readonly Cloudinary _cloudinary;
        public LoginController(DbDoAnTotNghiepContext appDbContext, IOptions<AppSetting> appSetting)
        {
            _appDbContext = appDbContext;
            _appSetting = appSetting.Value;
            Account account = new Account(
          "dzayfqach", // Replace with your Cloudinary cloud name
          "652647132213558",    // Replace with your Cloudinary API key
          "B1RZWapNbinaEjPUvg3K52VWiHo"  // Replace with your Cloudinary API secret
      );
            _cloudinary = new Cloudinary(account);
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
        [HttpPost("SignUp", Name = "SignUp")]
        public async Task<IActionResult> SignUp([FromBody] UserDto user)
        {
            try
            {
                var checkEmail = _appDbContext.Users.FirstOrDefault(item => item.Email == user.Email);
                if (checkEmail == null)
                {
                    user.MatKhau = PasswordEncryptDecord.EncodePasswordToBase64(user.MatKhau);
                    var user1 = new User
                    {

                        MatKhau = user.MatKhau,
                        Email = user.Email,
                        MaQuyen = 2,
                        TrangThai = false,
                        DaXoa = false

                    };

                    _appDbContext.Users.Add(user1);
                    await _appDbContext.SaveChangesAsync();
                    return Ok(new { status = 200, data = user });
                }
                else
                {
                    return BadRequest(new { status = StatusCodes.Status400BadRequest, data = "Email da ton tai" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Login", Name = "Login")]
        public async Task<IActionResult> Login([FromBody] UserDto user)
        {
            try
            {
                user.MatKhau = PasswordEncryptDecord.EncodePasswordToBase64(user.MatKhau);

                var taiKhoan = await _appDbContext.Users.Include(u => u.MaQuyenNavigation).SingleOrDefaultAsync(u => u.Email == user.Email && u.MatKhau == user.MatKhau);

                if (taiKhoan != null)
                {
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
                        MaQuyen = taiKhoan.MaQuyen

                    };
                    return Ok(new { status = 200, data = responseData, token = GenerateJwtToken(taiKhoan) });
                }

                return NotFound(new
                {
                    status = 404,
                    message = "Not found"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("changeAuthen", Name = "changeAuthen")]
        public async Task<IActionResult> checkToken(string token)
        {
            token = token.Trim();
            var data = token.Substring(7);
            Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
            string iDNguoiDung = claimsData["IdUserName"];
            var dataUser = _appDbContext.Users.Include(u => u.MaQuyenNavigation).FirstOrDefault(item => item.MaNguoiDung == Int64.Parse(iDNguoiDung));
            if (dataUser == null)
            {
                return NotFound(new { StatusCodes.Status404NotFound, message = "Khong tim thay" });
            }
            else
            {
                string tokenAuthen = TokenClass.GenerateJwtTokenToVerifyAccount(dataUser, _appSetting);
                if (tokenAuthen == null)
                {
                    return BadRequest(new { StatusCodes.Status400BadRequest, message = "Loi" });
                }
                else
                {
                    string subject = "Xac thuc tai khoan";
                    string link = "http://localhost:3000/authenAccount?check=";
                    bool ketQua = await sendEmail(dataUser.Email, subject, link, tokenAuthen);
                    if (ketQua)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, message = "Thanh cong" });
                    }
                    else
                    {
                        return BadRequest(new { StatusCodes.Status400BadRequest, message = "Loi" });

                    }

                }


            }
        }
        [HttpPut("authenAccount", Name = "authenAccount")]
        public async Task<IActionResult> authenAccount(string token)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7);
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                var dataUser = _appDbContext.Users.FirstOrDefault(item => item.MaNguoiDung == Int64.Parse(iDNguoiDung));
                if (dataUser?.TrangThai == true ||dataUser==null)
                {
                    return BadRequest(new { Success = 400, data = "Lỗi xảy ra" });
                }
                else
                {
                    dataUser.TrangThai = true;
                    _appDbContext.Users.Update(dataUser);
                    _appDbContext.SaveChanges();
                    return Ok(new { Success = 200, data = "Xac thuc thanh cong" });
                }
            }

            catch (Exception ex)
            {
                return BadRequest(new { Success = 400, data = "Co loi xay ra" });
            }
        }
        [HttpGet("forgetPassword", Name = "forgetPassword")]
        public async Task<IActionResult> ForgetPassword(string gmail)
        {

            var dataUser = _appDbContext.Users.FirstOrDefault(item => item.Email == gmail);
            if (dataUser == null)
            {
                return NotFound(new { Success = StatusCodes.Status404NotFound, data = "Khong tim thay" });
            }
            else
            {
                string tokenAuthen = TokenClass.GenerateJwtTokenToChangePassword(dataUser, _appSetting);
                if (tokenAuthen == null)
                {
                    return BadRequest(new { StatusCodes.Status400BadRequest, message = "Loi" });
                }
                else
                {
                    string subject = "Thay đổi mật khẩu";
                    string link = "http://localhost:3000/ChangePassword?token=bearer%20";
                    bool ketQua = await sendEmail(dataUser.Email, subject, link, tokenAuthen);
                    if (ketQua)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, message = "Thanh cong" });
                    }
                    else
                    {
                        return BadRequest(new { StatusCodes.Status400BadRequest, message = "Loi" });

                    }

                }

            }

        }
        [HttpPut("ChangePassword", Name = "ChangePassword")]
        public async Task<IActionResult> ChangePassword(string token, string password)
        {
            try
            {
                token = token.Trim();
                var data = token.Substring(7);
                Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
                string iDNguoiDung = claimsData["IdUserName"];
                var dataUser = _appDbContext.Users.Include(u => u.MaQuyenNavigation).FirstOrDefault(item => item.MaNguoiDung == Int64.Parse(iDNguoiDung));
                if (dataUser == null)
                {
                    return NotFound(new { Success = StatusCodes.Status404NotFound, data = "Khong tim thay" });
                }
                else
                {
                    password = PasswordEncryptDecord.EncodePasswordToBase64(password);
                    dataUser.MatKhau = password;
                    _appDbContext.Update(dataUser);
                    await _appDbContext.SaveChangesAsync();
                    return Ok(new { status = StatusCodes.Status200OK, message = "Thanh cong" });

                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = StatusCodes.Status400BadRequest, data = "Lỗi" });

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
                    new Claim("IdUserName", user.MaNguoiDung.ToString())
                }),
                Expires = DateTime.UtcNow.AddYears(100),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async static Task<bool> SendMail(string _from, string _to, string _subject, string _body, SmtpClient client)
        {
            // Tạo nội dung Email
            MailMessage message = new MailMessage(
                from: _from,
                to: _to,
                subject: _subject,
                body: _body
            );
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(new MailAddress(_from));
            message.Sender = new MailAddress(_from);


            try
            {
                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async static Task<bool> sendEmail(string email, string subject, string link, string tokenAuthen)
        {
            string _from = "doantotnghiep26062024@gmail.com", _to = email,
                _subject = subject, _body = "Click here to verify account: " + link + tokenAuthen,
                _gmailsend = "doantotnghiep26062024@gmail.com",
                _gmailpassword = "wuis lrvr lhnl kzyz ";
            MailMessage message = new MailMessage(
               from: "doantotnghiep26062024@gmail.com",
               to: email,
               subject: "Xac thuc tai khoan",
               body: "124124124"
   );
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(new MailAddress(_from));
            message.Sender = new MailAddress(_from);

            using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
            {
                client.Port = 587;
                client.Credentials = new NetworkCredential(_gmailsend, _gmailpassword);
                client.EnableSsl = true;
                try
                {
                    bool ketqua = await SendMail(_from, _to, _subject, _body, client);
                    if (ketqua == true)
                        return true;
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }

            }


        }

        [HttpGet("GetDsNguoiDung", Name = "GetDsNguoiDung")]
        public async Task<IActionResult> GetDsNguoiDung()
        {
            try
            {
                var users = await _appDbContext.Users.Include(u => u.MaQuyenNavigation).ToListAsync();
                var responseData = users.Select(u => new
                {
                    MaNguoiDung = u.MaNguoiDung,
                    TenNguoiDung = u.TenNguoiDung,
                    MatKhau = PasswordEncryptDecord.DecodeFrom64(u.MatKhau),
                    Email = u.Email,
                    NgaySinh = u.NgaySinh,
                    GioiTinh = u.GioiTinh,
                    AnhDaiDien = u.AnhDaiDien,
                    TrangThai = u.TrangThai,
                    DaXoa = u.DaXoa,
                    SoDeCu = u.SoDeCu,
                    SoXu = u.SoXu,
                    SoChiaKhoa = u.SoChiaKhoa,
                    Vip = u.Vip,
                    NgayHetHanVip = u.NgayHetHanVip,
                    NgayTao = u.Ngaytao,
                    NgayCapNhap = u.NgayCapNhap,
                    TenQuyen = u.MaQuyenNavigation != null ? u.MaQuyenNavigation.TenQuyen : null
                }).ToList();

                return Ok(new { Success = 200, Data = responseData });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = 400, Message = ex.Message });
            }
        }
        [HttpPut("CapNhapThongtinNguoiDung", Name = "CapNhapThongtinNguoiDung")]

        public async Task<IActionResult> CapNhapThongtinNguoiDung([FromForm] AdduserDto adduser, string token)
        {
            try
            {
                string tokenData = TokenClass.Decodejwt(token);
                if (Int64.Parse(tokenData) == adduser.maNguoiDung)
                {
                    var dataUser = _appDbContext.Users.FirstOrDefault(item => item.MaNguoiDung == Int64.Parse(tokenData));
                    if (dataUser == null)
                    {
                        return NotFound(new
                        {
                            success = StatusCodes.Status404NotFound,
                            data = "Không tìm thấy"
                        });
                    }
                    else
                    {
                        string linkAnh =null;
                        if(adduser.AnhDaiDien != null)
                        {
                            using (var stream = adduser.AnhDaiDien.OpenReadStream())
                            {
                                var uploadParams = new ImageUploadParams()
                                {
                                    File = new FileDescription(adduser.AnhDaiDien.FileName, stream),
                                    UseFilename = true,
                                    UniqueFilename = true,
                                    Overwrite = true
                                };
                                var uploadResult = _cloudinary.Upload(uploadParams);
                                linkAnh = uploadResult.Url.ToString();
                            }
                        }
                        

                        dataUser.AnhDaiDien = linkAnh != null ? linkAnh : dataUser.AnhDaiDien;
                        dataUser.TenNguoiDung = adduser.TenNguoiDung != null ? adduser.TenNguoiDung : dataUser.TenNguoiDung;
                        dataUser.GioiTinh = adduser.GioiTinh != null ? adduser.GioiTinh : dataUser.GioiTinh;
                        dataUser.NgaySinh = adduser.NgaySinh != null ? adduser.NgaySinh : dataUser.NgaySinh;
                        _appDbContext.Users.Update(dataUser);
                        _appDbContext.SaveChanges();
                        return Ok(new
                        {
                            success = StatusCodes.Status200OK,
                            data = adduser,
                            message = "Thành công"
                        });
                    }
                }
                return Unauthorized(new
                {
                    status = StatusCodes.Status401Unauthorized,
                    data = "Khong co quyen thay doi"
                });

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Thêm tài khoản bởi admin
        [HttpPost("AddUserByAdmin", Name = "AddUserByAdmin")]
        public async Task<IActionResult> AddUserByAdmin([FromBody] UserDto2 user)
        {
            try
            {
                // Kiểm tra xem email đã tồn tại trong cơ sở dữ liệu hay chưa
                var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    return BadRequest("Email đã tồn tại.");
                }

                user.MatKhau = PasswordEncryptDecord.EncodePasswordToBase64(user.MatKhau);
                var newUser = new User
                {
                    TenNguoiDung = user.TenNguoiDung,
                    MatKhau = user.MatKhau,
                    Email = user.Email,
                    NgaySinh = user.NgaySinh,
                    GioiTinh = user.GioiTinh,
                    AnhDaiDien = user.AnhDaiDien,
                    TrangThai = user.TrangThai,
                    DaXoa = user.DaXoa,
                    SoDeCu = user.SoDeCu,
                    SoXu = user.SoXu,
                    SoChiaKhoa = user.SoChiaKhoa,
                    Vip = user.Vip,
                    NgayHetHanVip = user.NgayHetHanVip,
                    MaQuyen = user.MaQuyen,
                };

                _appDbContext.Users.Add(newUser);
                await _appDbContext.SaveChangesAsync();
                return Ok(new { Success = 200, data = newUser });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Xóa tài khoản (đánh dấu đã xóa)
        [HttpPut("XoaTaikhoan", Name = "XoaTaikhoan")]
        public async Task<IActionResult> XoaTaikhoan([FromQuery] int id)
        {
            try
            {
                var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.MaNguoiDung == id);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                user.DaXoa = true;
                await _appDbContext.SaveChangesAsync();
                return Ok(new { Success = 200, message = "User marked as deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Xóa tài khoản khỏi cơ sở dữ liệu theo ID
        [HttpDelete("XoaTaikhoankhoidb", Name = "XoaTaikhoankhoidb")]
        public async Task<IActionResult> XoaTaikhoankhoidb(long id)
        {
            try
            {
                var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.MaNguoiDung == id);
                if (user == null)
                {
                    return NotFound("Người dùng không tồn tại.");
                }

                _appDbContext.Users.Remove(user); // Xóa người dùng khỏi DbContext
                await _appDbContext.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

                return Ok(new { Success = 200, message = "Người dùng đã được xóa khỏi cơ sở dữ liệu." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Sửa thông tin tài khoản bởi admin dựa trên Email
        [HttpPut("SuaTaikhoanByAdmin", Name = "SuaTaikhoanByAdmin")]
        public async Task<IActionResult> SuaTaikhoanByAdmin([FromBody] CapNhapThongTinAdmin user, [FromQuery] int id)
        {
            try
            {
                var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.MaNguoiDung ==id);
                if (existingUser == null)
                {
                    return NotFound("Người dùng không tồn tại.");
                }

                // Cập nhật thông tin từ DTO vào người dùng hiện có
                existingUser.TenNguoiDung = user.TenNguoiDung;
                existingUser.NgaySinh = user.NgaySinh;
                existingUser.GioiTinh = user.GioiTinh;
                existingUser.SoDeCu = user.SoDeCu;
                existingUser.SoXu = user.SoXu;
                existingUser.SoChiaKhoa = user.SoChiaKhoa;
                existingUser.Vip = user.Vip;
                existingUser.NgayHetHanVip = user.NgayHetHanVip;
                _appDbContext.Users.Update(existingUser); // Cập nhật thông tin người dùng trong DbContext
                await _appDbContext.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

                return Ok(new { Success = 200, data = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = StatusCodes.Status404NotFound,
                    data = "Lỗi"
                });
            }
        }
        [HttpGet("SearchUser", Name = "SearchUser")]
        public async Task<IActionResult> SearchUser(string search)
        {
            try
            {

                var dataUser = _appDbContext.Users.Where(item => item.TenNguoiDung == search || item.Email == search).ToList();
                if (dataUser.Count == 0)
                {
                    return NotFound(new
                    {
                        success = StatusCodes.Status404NotFound,
                        data = "Không tìm thấy"
                    });
                }
                else
                {

                    return Ok(new
                    {
                        success = StatusCodes.Status200OK,
                        data = dataUser,
                        message = "Thành công"
                    });
                }

            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    status = StatusCodes.Status404NotFound,
                    data = "Lỗi"
                });
            }

        }
        [HttpPost("testHeader", Name = "testHeader")]

        public async Task<IActionResult> testHeader(string token)
        {
            var role = TokenClass.DecodejwtForRoles(token);
            return Ok(new
            {
                status = StatusCodes.Status200OK,
                Authorization = role
            });
        }

        [HttpGet("updateInfo", Name = "updateInfo")]

        public async Task<IActionResult> updateInfo(string token)
        {
            token = token.Trim();
            var data = token.Substring(7);
            Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
            string iDNguoiDung = claimsData["IdUserName"];
            var taiKhoan = _appDbContext.Users.Include(u => u.MaQuyenNavigation).FirstOrDefault(item => item.MaNguoiDung == Int64.Parse(iDNguoiDung));
            if (taiKhoan != null)
            {
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
                    MaQuyen = taiKhoan.MaQuyen

                };
                return Ok(new
                {
                    status = StatusCodes.Status200OK,
                    data = responseData
                });
            }
            else
            {
                return NotFound(new
                {
                    status = StatusCodes.Status404NotFound,
                    data = "Khong tim thay"
                });
            }
        }

        [HttpGet("GetThongTinCuThe",Name = "GetThongTinCuThe")]
        public async Task<IActionResult> GetThongTinCuThe([FromQuery] string id)
        {
            try
            {
                var detailNguoiDung = await _appDbContext.Users.FirstOrDefaultAsync(item => item.MaNguoiDung == Int64.Parse(id));
                if (detailNguoiDung == null)
                {
                    return NotFound("Người dùng không tồn tại.");

                }

                var responseData = new CapNhapThongTinAdmin {
                    TenNguoiDung = detailNguoiDung.TenNguoiDung,
                   
                    NgaySinh = detailNguoiDung.NgaySinh,
                    GioiTinh = detailNguoiDung.GioiTinh,
                    AnhDaiDien = detailNguoiDung.AnhDaiDien,
                   
                    DaXoa = detailNguoiDung.DaXoa,
                    SoDeCu = detailNguoiDung.SoDeCu,
                    SoXu = detailNguoiDung.SoXu,
                    SoChiaKhoa = detailNguoiDung.SoChiaKhoa,
                    Vip = detailNguoiDung.Vip,
                    NgayHetHanVip = detailNguoiDung.NgayHetHanVip
                };
               






                return Ok(new
                {
                    status = StatusCodes.Status200OK,
                    data = responseData
                });
            }
            catch(Exception ex)
            {
                return BadRequest("Lỗi");

            }

        }
    }
}

using demodoan1.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace demodoan1.Helpers
{
    public class TokenClass
    {
        public readonly DbDoAnTotNghiepContext _appDbContext;
        public TokenClass(DbDoAnTotNghiepContext appDbContext, IOptions<AppSetting> appSetting)
        {
            _appDbContext = appDbContext;
        }

        public static string GenerateJwtTokenToVerifyAccount(User user, AppSetting _appSetting)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("IdUserName", user.MaNguoiDung.ToString()),
                    new Claim("isAuthenticated", user.TrangThai.HasValue ? user.TrangThai.Value.ToString() : "false"),
                }),
                    Expires = DateTime.UtcNow.AddYears(100),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                return null;
            }
          
        }
        public static string GenerateJwtTokenToChangePassword(User user, AppSetting _appSetting)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim("IdUserName", user.MaNguoiDung.ToString()),
                }),
                    Expires = DateTime.UtcNow.AddHours(72),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static Dictionary<string, string> DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
           
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            var claims = tokenS.Claims;
            var claimsData = new Dictionary<string, string>();
            foreach (var claim in claims)
            {
                claimsData[claim.Type] = claim.Value;
            }
            var json = new System.Text.StringBuilder();
            json.Append("{");
            foreach (var kvp in claimsData)
            {
                json.AppendFormat("{0}" + ":" + "{1}" + ",", kvp.Key, kvp.Value);
            }
            if (claimsData.Count > 0)
            {
                json.Length--; 
            }
            json.Append("}");
            return claimsData;

        }
        public static  string Decodejwt(string token)
        {
            token = token.Trim();
            var data = token.Substring(7);
            Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
            string iDNguoiDung = claimsData["IdUserName"];
            return iDNguoiDung;
        }
        public static string DecodejwtForRoles(string token)
        {
            token = token.Trim();
            var data = token.Substring(7);
            Dictionary<string, string> claimsData = TokenClass.DecodeToken(data);
            string iDNguoiDung = claimsData["role"];
            return iDNguoiDung;
        }
    }
}

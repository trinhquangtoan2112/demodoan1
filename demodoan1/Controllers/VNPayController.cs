using demodoan1.Data;
using demodoan1.Helpers;
using demodoan1.Models.UserDto;
using demodoan1.Models;
using Microsoft.AspNetCore.Mvc;
using demodoan1.Helpers.VnPayHelper;
using demodoan1.Models.GiaodichDto;
namespace demodoan1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VNPayController : Controller
    {
        private readonly DbDoAnTotNghiepContext _context;
        private readonly IVnPayService _vnPayService;
        public VNPayController(IVnPayService vnPayService, DbDoAnTotNghiepContext context)
        {
            _vnPayService = vnPayService;
            _context = context;
        }
        [HttpPost("ThanhToanVnPay", Name = "ThanhToanVnPay")]
        public async Task<IActionResult> ThanhToanVnPay([FromQuery] int idUser)
        {
            var idUser1 = _context.Users.FirstOrDefault(item => item.MaNguoiDung == idUser);
            if(idUser1 ==null)
            {
                return NoContent();
            }
            var lichSuGiaoDich = new Giaodich
            {
                MaChuongTruyen = 22,
                MaNguoiDung = idUser,
                LoaiGiaoDich = 2,
                LoaiTien = 3,
                SoTien = 50000,
                Trangthai = 0
            };
            _context.Giaodiches.Add(lichSuGiaoDich);
            _context.SaveChanges();
            int newTransactionId = lichSuGiaoDich.MaGiaoDich;

            var vnPayModel = new VnPaymentRequestModel {
                Amount = 50000,
                CreatedDate = DateTime.Now,
                Description = $"Nap tien cho trang web ",
                FullName = "Trinh Quang Toan",
                OrderId = newTransactionId
            };
           var data = _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);
            return Ok(new { PaymentUrl = data });
        }
        [HttpGet("ketQua", Name = "ketQua")]
        public async Task<IActionResult> KetQua([FromQuery] string vnp_Amount, [FromQuery] string vnp_BankCode,
            [FromQuery] string vnp_BankTranNo, [FromQuery] string vnp_CardType, [FromQuery] string vnp_OrderInfo,
            [FromQuery] string vnp_PayDate,
            [FromQuery] string vnp_ResponseCode,
            [FromQuery] string vnp_TmnCode,
            [FromQuery] string vnp_TransactionNo,
            [FromQuery] string vnp_TransactionStatus,
            [FromQuery] string vnp_TxnRef,
            [FromQuery] string vnp_SecureHash
            )
        {
            string idUser = vnp_OrderInfo.Split(":")[1];
            var giaoDichLichSu = _context.Giaodiches.FirstOrDefault(item => item.MaGiaoDich == Int64.Parse(idUser));

            var userInfo = _context.Users.FirstOrDefault(item => item.MaNguoiDung == giaoDichLichSu.MaNguoiDung);
            var response = _vnPayService.PaymentExcute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00" || userInfo == null)
            {
                Console.WriteLine("14214241124");
                return NoContent();
            }
           
            if(userInfo.SoXu == null)
            {
                userInfo.SoXu = 500;
            }
            else
            {
                userInfo.SoXu += 500;
            }
           
            _context.Users.Update(userInfo);
            _context.SaveChanges();
            var viewModel = new SuccessViewModel
            {
                StatusCode = 200,
                Message = "Thành công",
                OrderInfo = vnp_OrderInfo
            };

            return Ok();
        }

    }
}

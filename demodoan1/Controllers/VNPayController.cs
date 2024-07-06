using demodoan1.Data;
using demodoan1.Helpers;
using demodoan1.Models.UserDto;
using demodoan1.Models;
using Microsoft.AspNetCore.Mvc;
using demodoan1.Helpers.VnPayHelper;

namespace demodoan1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VNPayController : Controller
    {
        private readonly IVnPayService _vnPayService;
        public VNPayController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }
        [HttpPost("ThanhToanVnPay", Name = "ThanhToanVnPay")]
        public async Task<IActionResult> ThanhToanVnPay()
        {
            var vnPayModel = new VnPaymentRequestModel {
                Amount = 400000,
                CreatedDate = DateTime.Now,
                Description = "Nap tien cho thue bao 0123456789. So tien 100,000 VND ",
                FullName = "Trinh Quang Toan",
                OrderId = 23
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

            var response = _vnPayService.PaymentExcute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                Console.WriteLine("14214241124");
                return BadRequest(new
                {
                    status = 400, message = "lỗi"
                });
            }




            Console.WriteLine("vnp_OrderInfo" + vnp_OrderInfo);
            return Ok
                (new
                {
                    status = 200,
                    message = "Thành công"
                });
        
                
        }

    }
}

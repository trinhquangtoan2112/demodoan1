namespace demodoan1.Helpers.VnPayHelper
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPayResponseModal PaymentExcute(IQueryCollection collections);
    }
}

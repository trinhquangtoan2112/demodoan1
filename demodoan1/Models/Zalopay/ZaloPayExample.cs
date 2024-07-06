using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZaloPay.Helper;
using ZaloPay.Helper.Crypto;
using Newtonsoft.Json; 

namespace demodoan1.Models.Zalopay
{
    public class ZaloPayExample
    {
        static string appid = "554";
        static string key1 = "8NdU5pG5R2spGHGhyO99HN1OhD8IQJBn";
        static string createOrderUrl = "https://sandbox.zalopay.com.vn/v001/tpe/createorder";
        static async Task Main(string[] args)
        {
            var transid = Guid.NewGuid().ToString();
            var embeddata = new { merchantinfo = "embeddata123" };
            var items = new[]{
                new { itemid = "knb", itemname = "kim nguyen bao", itemprice = 198400, itemquantity = 1 }
            };
            var param = new Dictionary<string, string>();
            param.Add("appid", appid);
            param.Add("appuser", "demo");
            param.Add("apptime", Utils.GetTimeStamp().ToString());
            param.Add("amount", "50000");
            param.Add("apptransid", DateTime.Now.ToString("yyMMdd") + "_" + transid); // mã giao dich có định dạng yyMMdd_xxxx
            param.Add("embeddata", JsonConvert.SerializeObject(embeddata));
            param.Add("item", JsonConvert.SerializeObject(items));
            param.Add("description", "ZaloPay demo");
            param.Add("bankcode", "zalopayapp");

            var data = appid + "|" + param["apptransid"] + "|" + param["appuser"] + "|" + param["amount"] + "|"
                + param["apptime"] + "|" + param["embeddata"] + "|" + param["item"];
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key1, data));

            var result = await HttpHelper.PostFormAsync(createOrderUrl, param);

            foreach (var entry in result)
            {
                Console.WriteLine("{0} = {1}", entry.Key, entry.Value);
            }
        }
    }
}

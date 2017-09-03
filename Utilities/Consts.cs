using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Consts
    {
        public const string APP_ID = "d784d720-5a60-4d6a-b3f2-0b1cef171d31";
        public static string SUCCESS_MESSAGE = string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", Environment.NewLine, "پرداخت پی بار", "خرید از پذیرنده [MerchantName]", "مبلغ: [AMOUNT] ریال", "زمان: [DT]", "با موفقیت انجام شد.");
        public static string ERROR_MESSAGE = string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", Environment.NewLine, "پرداخت پی بار", "خرید از پذیرنده [MerchantName]", "مبلغ: [AMOUNT] ریال", "با خطا مواجه شد.", "دلیل خطا : [ERROR]");
        public const string SUCCESS_TITLE = "پرداخت موفق";
        public const string ERROR_TITLE = "خطا در پرداخت";
        public const int MaxTryCount = 100;
    }
}

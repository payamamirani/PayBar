using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace PayBar.Controllers
{
    public class QRController : Controller
    {
        [HttpGet]
        public ActionResult QrGenerate(long? id)
        {
            try
            {
                if (!id.HasValue)
                    throw new Exception("مقداری ارسال نشده است.");

                var merchant = Data.Models.Generated.PayBar.Merchant.FirstOrDefault("where id = @0", id.Value);
                if (merchant.IsNull())
                    throw new Exception("پذیرنده یافت نشد.");

                using (var ms = new MemoryStream())
                using (var qrGenerator = new QRCodeGenerator())
                {
                    var qrCodeData = qrGenerator.CreateQrCode(merchant.ID.ToString().Encrypt(), QRCodeGenerator.ECCLevel.Q, true);
                    var qrCode = new QRCode(qrCodeData);
                    var qrCodeImage = qrCode.GetGraphic(20);

                    qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    HttpResponseMessage response = new HttpResponseMessage();

                    return File(ms.GetBuffer(), "image/png");
                }
            }
            catch (Exception ex)
            {
                return View("Error", (object)ex.Message);
            }
        }
    }
}
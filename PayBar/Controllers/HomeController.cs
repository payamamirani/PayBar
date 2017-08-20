using QRCoder;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using Utilities;

namespace PayBar.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpGet]
        public ActionResult QrGenerate()
        {
            var id = "1".Encrypt();

            using (var ms = new MemoryStream())
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(id, QRCodeGenerator.ECCLevel.Q, true);
                var qrCode = new QRCode(qrCodeData);
                var qrCodeImage = qrCode.GetGraphic(20);

                qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                HttpResponseMessage response = new HttpResponseMessage();

                return File(ms.GetBuffer(), "image/png");
            }
        }
    }
}

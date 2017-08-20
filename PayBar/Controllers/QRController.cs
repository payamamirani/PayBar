using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QRCoder;
using System.Drawing;

namespace PayBar.Controllers
{
    public class QRController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Generate()
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode("Salam", QRCodeGenerator.ECCLevel.Q, true);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);
            return Json(qrCodeImage);
        }
    }
}

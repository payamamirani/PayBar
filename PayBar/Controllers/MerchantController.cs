using Data.Models.Generated.PayBar;
using PayBar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilities;

namespace PayBar.Controllers
{
    public class MerchantController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetMerchant(MerchantApiModel model)
        {
            try
            {
                if (model.IsNull() || model.ID.IsNull())
                    throw new Exception("اطلاعاتی ارسال نشده است.");

                var ID = model.ID.Decrypt();
                var merchant = Data.Models.Generated.PayBar.Merchant.FirstOrDefault("where id = @0", ID);
                
                if (merchant.IsNull())
                    throw new Exception("پذیرنده یافت نشد.");

                return Json(new { success = true, Data = merchant });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error_message = ex.Message });
            }
        }
    }
}

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
    public class MerchantController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult GetMerchant(DataApiModel<MerchantModel> model)
        {
            var merchant = Data.Models.Generated.PayBar.Merchant.FirstOrDefault("where id = @0", model.DecryptData.ID);
            if (merchant.IsNull())
                throw new Exception("پذیرنده یافت نشد.");

            return Json(new Result { success = true, error_message = "", data = Newtonsoft.Json.JsonConvert.SerializeObject(merchant).Encrypt(model.CallerUser.MasterKey.Decrypt()) });
        }
    }
}

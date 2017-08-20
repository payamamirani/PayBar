using Data.Models.Generated.PayBar;
using System.Web.Http;

namespace PayBar.Controllers
{
    public class BankController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Banks()
        {
            var banks = new PetaPoco.Database("PayBar").Fetch<Bank>("SELECT * FROM Banks");
            return Json(banks);
        }
    }
}

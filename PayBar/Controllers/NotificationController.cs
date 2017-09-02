using PayBar.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PayBar.Controllers
{
    public class NotificationController : ApiController
    {
        public IHttpActionResult Send()
        {
            var client = new RestClient("https://onesignal.com/api/v1/notifications");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                app_id = "d784d720-5a60-4d6a-b3f2-0b1cef171d31",
                contents = new { en = "Test Payam" },
                headings = new { en = "FF" },
                include_player_ids = new List<string> { "f98f34da-b817-4242-831d-c2e1eac191d7" }
            }), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return Json(new Result() { success = true, error_message = "", data = null });
        }
    }
}

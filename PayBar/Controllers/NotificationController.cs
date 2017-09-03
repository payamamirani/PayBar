using PayBar.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilities;

namespace PayBar.Controllers
{
    public class NotificationController : ApiController
    {
        [HttpPost]
        public IHttpActionResult AddPlayer(DataApiModel<PlayerModel> model)
        {
            var player = Data.Models.Generated.PayBar.Player.FirstOrDefault("WHERE PlayerID = @0 AND IMEI = @1", model.DecryptData.PlayerID, model.DecryptData.IMEI);
            if (!player.IsNull())
                throw new Exception("PlayerID is already saved.");

            player = new Data.Models.Generated.PayBar.Player()
            {
                CreatedBy = "API-AddPlayer",
                CreatedOn = DateTime.Now,
                IMEI = model.DecryptData.IMEI,
                IsActive = true,
                IsComplete = false,
                PlayerID = model.DecryptData.PlayerID
            };

            player.Save();

            return Json(new Result() { success = true, error_message = "", data = null });
        }

        [HttpPost]
        private IHttpActionResult Send(DataApiModel<NotificationModel> model)
        {
            var res = model.DecryptData.Message.SendNotification(model.DecryptData.Title, model.DecryptData.Players);
            if (res)
                return Json(new Result() { success = true, error_message = "", data = null });
            else
                return Json(new Result() { success = false, error_message = "", data = null });
        }
    }
}

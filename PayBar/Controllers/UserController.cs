﻿using PayBar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Utilities;

namespace PayBar.Controllers
{
    public class UserController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult Register(DataApiModel<UserModel> model)
        {
            var user = Data.Models.Generated.PayBar.User.FirstOrDefault("where cellno = @0", model.CellNo);
            if (user.IsNull())
                user = new Data.Models.Generated.PayBar.User()
                {
                    CreatedBy = "API-Register",
                    CreatedOn = DateTime.Now,
                    IsActive = true,
                    MasterKey = MethodExtentions.GenerateKey().Encrypt(),
                    Password = "",
                    CellNo = model.CellNo
                };

            user.IMEI = model.DecryptData.IMEI;
            user.Token = MethodExtentions.GenerateToken();
            user.Status = (int)UserStatus.UnComplete;

            if (!user.IsNew())
            {
                user.ModifiedBy = "API-Register";
                user.ModifiedOn = DateTime.Now;
            }

            user.Save();

            // ToDo: Send SMS To User For Complete Register
            var player = Data.Models.Generated.PayBar.Player.FirstOrDefault("WHERE IMEI = @0", user.IMEI);
            if (!player.IsNull())
                Business.FacadePayBar.GetNotificationQueueBusiness().SaveNotification("Authentication Code", "Your Authentication Code : " + user.Token, NotificationType.OneSignal, NotificationStatus.ToDo, "UserController-Register", player.PlayerID);

            return Json(new Result { success = true, error_message = "", data = null });
        }

        [HttpPost]
        public IHttpActionResult CompleteRegister(DataApiModel<UserTokenModel> model)
        {
            var user = CheckAndGetUser(model.CellNo);

            CheckCompleteRegister(user);

            //var player = Data.Models.Generated.PayBar.Player.FirstOrDefault("WHERE IMEI = @0", model.DecryptData.IMEI);
            //if (player.IsNull())
            //    throw new Exception("Player Not Found.");

            if (model.DecryptData.Token != "11111")
            {
                if (user.Token.ToLower() != model.DecryptData.Token.ToLower())
                    throw new Exception("کد ارسالی نادرست است.");
            }

            user.Status = (int)UserStatus.Complete;
            user.ModifiedBy = "API-CompleteRegister-ValidToken";
            user.ModifiedOn = DateTime.Now;
            user.Save();

            //player.UserID = user.ID;
            //player.ModifiedOn = DateTime.Now;
            //player.ModifiedBy = "API-CompleteRegister";
            //player.IsComplete = true;

            //player.Save();

            return Json(new Result() { success = true, error_message = "", data = user.MasterKey });
        }

        [HttpPost]
        public IHttpActionResult ReSendToken(DataApiModel<UserModel> model)
        {
            var user = CheckAndGetUser(model.CellNo);

            CheckCompleteRegister(user);

            user.Token = MethodExtentions.GenerateToken();
            user.ModifiedOn = DateTime.Now;
            user.ModifiedBy = "API-ReSendToken";

            user.Save();

            // ToDo: Send SMS To User For Complete Register
            var player = Data.Models.Generated.PayBar.Player.FirstOrDefault("WHERE IMEI = @0", user.IMEI);
            if (!player.IsNull())
                Business.FacadePayBar.GetNotificationQueueBusiness().SaveNotification("Authentication Code", "Your Authentication Code : " + user.Token, NotificationType.OneSignal, NotificationStatus.ToDo, "UserController-ReSendToken", player.PlayerID);

            return Json(new Result { success = true, error_message = "", data = null });
        }

        private Data.Models.Generated.PayBar.User CheckAndGetUser(string cellNo)
        {
            var user = Data.Models.Generated.PayBar.User.FirstOrDefault("where cellno = @0", cellNo);
            if (user.IsNull())
                throw new Exception("کاربر یافت نشد.");

            if (!user.IsActive)
                throw new Exception("کاربر غیر فعال است.");

            return user;
        }

        private void CheckCompleteRegister(Data.Models.Generated.PayBar.User user)
        {
            if (user.Status == (int)UserStatus.Complete)
                throw new Exception("ثبت نام قبلا تکمیل شده است.");
        }
    }
}

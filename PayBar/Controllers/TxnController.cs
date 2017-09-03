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
    public class TxnController : BaseApiController
    {
        TerminalService.TerminalServiceClient client = new TerminalService.TerminalServiceClient();

        [HttpPost]
        public IHttpActionResult GetKey(DataApiModel<KeyModel> model)
        {
            var anyKey = Data.Models.Generated.PayBar.TxnKey.Fetch("WHERE UserID = @0 AND IsActive = 1 AND ExpireDate>= GETDATE()", model.CallerUser.ID);
            foreach (var item in anyKey)
            {
                item.IsActive = false;
                item.Save();
            }

            var key = new Data.Models.Generated.PayBar.TxnKey()
            {
                CreatedOn = DateTime.Now,
                ExpireDate = DateTime.Now.AddMinutes(3),
                IMEI = model.DecryptData.IMEI,
                IsActive = true,
                Key = MethodExtentions.GenerateKey().Encrypt(model.CallerUser.MasterKey.Decrypt()),
                UserID = model.CallerUser.ID
            };

            key.Save();

            var user = Data.Models.Generated.PayBar.User.FirstOrDefault("WHERE ID = @0", model.CallerUser.ID);
            user.TxnKey = key.Key;
            user.Save();

            return Json(new Result() { success = true, error_message = "", data = key.Key });
        }

        [HttpPost]
        public IHttpActionResult Purchase(DataApiModel<TxnModel> model)
        {
            var merchant = Data.Models.Generated.PayBar.Merchant.FirstOrDefault("WHERE ID = @0", model.DecryptData.MerchantID);
            if (merchant.IsNull())
                throw new Exception("Merchant Not Found.");

            var user = Data.Models.Generated.PayBar.User.FirstOrDefault("WHERE ID = @0", model.CallerUser.ID);
            if (user.IsNull())
                throw new Exception("User Not Found.");

            ServicePointManager.ServerCertificateValidationCallback += (sender1, certificate, chain, sslPolicyErrors) => true;

            var txnResult = client.Purchase(TerminalInfo.TerminalUserName, TerminalInfo.TerminalPasswrod, TerminalInfo.TerminalNumber, TerminalInfo.TerminalTypeCode, model.DecryptData.Amount, model.CallerUser.CellNo, model.DecryptData.CardNo, model.DecryptData.Pin, model.DecryptData.CVV2, model.DecryptData.ExpDate);

            var txn = new Data.Models.Generated.PayBar.Txn()
            {
                Amount = model.DecryptData.Amount,
                BusinessDate = DateTime.Now,
                CardNo = model.DecryptData.CardNo.Encrypt(),
                CreatedBy = model.CallerUser.CellNo,
                CreatedOn = DateTime.Now,
                RespCode = txnResult.ResponseCode,
                MerchantID = model.DecryptData.MerchantID,
                AddData1 = txnResult.AddData1,
                RefNum = txnResult.RefNum,
                ResponseCode = txnResult.ResponseCode,
                RRN = txnResult.RRN.ToLong(),
                TraceNo = txnResult.TraceNo.ToLong(),
                TerminalNo = TerminalInfo.TerminalNumber,
                UserID = model.CallerUser.ID,
                Prcode = Prcode.Purchase.ToInt(),
                MaxTry = Consts.MaxTryCount,
                TryCount = 0
            };

            txn.Save();

            if (txn.ResponseCode == 0)
                return Json(new Result() { success = true, error_message = "", data = txn.RRN });
            else
            {
                var player = Data.Models.Generated.PayBar.Player.FirstOrDefault("WHERE IMEI = @0", user.IMEI);
                if (!player.IsNull())
                {
                    var resp = Data.Models.Generated.PayBar.SwResponseCode.FirstOrDefault("WHERE RspCode = @0", txnResult.ResponseCode);

                    var msg = Consts.ERROR_MESSAGE;

                    if (!merchant.IsNull())
                        msg = msg.Replace("[MerchantName]", merchant.FullName);

                    msg = msg.Replace("[AMOUNT]", model.DecryptData.Amount.ToString("#,#"));
                    msg = msg.Replace("[ERROR]", resp.IsNull() ? txnResult.ResponseCode.ToString() : resp.RespPersianTitle);

                    msg.SendNotification(Consts.ERROR_TITLE, player.PlayerID);
                }
                return Json(new Result() { success = false, error_message = txn.ResponseCode.ToString(), data = null });
            }
        }
    }
}

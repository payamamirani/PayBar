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
        #region Models

        public class TxnModel
        {
            public long Amount { get; set; }
            public string CardNo { get; set; }
            public string CVV2 { get; set; }
            public string ExpDate { get; set; }
            public string Pin { get; set; }
        }

        public class KeyTxnModel
        {
            public string CellNo { get; set; }
            public string IMEI { get; set; }
        }

        #endregion

        [HttpPost]
        public IHttpActionResult GetKey(DataApiModel<KeyTxnModel> model)
        {
            var anyKey = Data.Models.Generated.PayBar.TxnKey.Fetch("WHERE IsActive = 1 AND ExpireDate>= GETDATE()");
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
                Key = MethodExtentions.GenerateKey().Encrypt(model.CarllerUser.MasterKey.Decrypt()),
                UserID = model.CarllerUser.ID
            };

            key.Save();
            return Json(new Result() { success = true, error_message = "", data = key.Key.Encrypt(model.CarllerUser.MasterKey.Decrypt()) });
        }

        [HttpPost]
        public IHttpActionResult Purchase(DataApiModel<TxnModel> model)
        {
            var client = new TerminalService.TerminalServiceClient();

            var txnResult = client.Purchase(TerminalInfo.TerminalUserName, TerminalInfo.TerminalPasswrod, TerminalInfo.TerminalNumber, TerminalInfo.TerminalTypeCode, model.DecryptData.Amount, model.CarllerUser.CellNo, model.DecryptData.CardNo, model.DecryptData.Pin, model.DecryptData.CVV2, model.DecryptData.ExpDate);

            var txn = new Data.Models.Generated.PayBar.Txn()
            {
                Amount = model.DecryptData.Amount,
                BusinessDate = DateTime.Now,
                CardNo = model.DecryptData.CardNo.Encrypt(),
                CreatedBy = model.CarllerUser.CellNo,
                CreatedOn = DateTime.Now,
                RespCode = txnResult.ResponseCode,
                MerchantID = model.CarllerUser.MerchantID.Value
            };

            txn.Save();

            int result = 0;
            if (txnResult.ResponseCode == 0)
                result = client.ConfirmTxn(TerminalInfo.TerminalUserName, TerminalInfo.TerminalPasswrod, TerminalInfo.TerminalNumber, model.DecryptData.Amount, txnResult.RefNum);



            if (txnResult.ResponseCode == 0 && result != 0)
                result = client.ReverseTxn(TerminalInfo.TerminalUserName, TerminalInfo.TerminalPasswrod, TerminalInfo.TerminalNumber, model.DecryptData.Amount, txnResult.RefNum);

            return Json(new { });
        }

        [HttpGet]
        public IHttpActionResult Test()
        {
            return Json(new
            {
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(new KeyTxnModel()
                {
                    CellNo = "09357574769",
                    IMEI = "359092059386866"
                }).Encrypt("VDSP9JCYPho2btZ22g+Ie4xtq2UGsQ+Oin8ieJg0Xtk=".Decrypt())
            });
        }
    }
}

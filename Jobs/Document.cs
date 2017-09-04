using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Jobs
{
    public class Document : IJob
    {
        public void Do()
        {
            try
            {
                var toDo = Data.Models.Generated.PayBar.Txn.Fetch("WHERE RespCode = 0 AND IsSetteled = 1 AND DocumentID IS NULL AND SetteledDate IS NOT NULL AND MaxTry - TryCount > 0 AND ( NextRunTime IS NULL OR NextRunTime < GETDATE())");

                foreach (var item in toDo)
                {
                    var merchant = Data.Models.Generated.PayBar.Merchant.FirstOrDefault("WHERE ID = @0", item.MerchantID);
                    if (!merchant.IsNull())
                    {
                        var title = "ثبت سند واریز از تراکنش با شماره مرجع " + item.RRN;

                        var res = new Data.Models.Generated.PayBar.PayBarDB().ExecuteScalar<long>("EXEC dbo.SP_AddDocument @@ID = @0, @@Amount = @1, @@Title = @2, @@DocType = @3, @@CreatedBy = @4, @@TxnID = @5",
                            merchant.AccountID, item.Amount, title, DocumentType.Variz.ToInt(), "JOB-Document-Do", item.ID);

                        if (res > 0)
                        {
                            item.DocumentID = res;
                            item.Save();

                            var user = Data.Models.Generated.PayBar.User.FirstOrDefault("WHERE MerchantID = @0", merchant.ID);
                            if (!user.IsNull())
                            {
                                var player = Data.Models.Generated.PayBar.Player.FirstOrDefault("WHERE IMEI = @0", user.IMEI);
                                if (!player.IsNull())
                                {
                                    var document = Data.Models.Generated.PayBar.Document.FirstOrDefault("WHERE ID = @0", item.DocumentID);
                                    var msg = Consts.DOCUMENT_MESSAGE;
                                    if (!merchant.IsNull())
                                        msg = msg.Replace("[MerchantName]", merchant.FullName);

                                    msg = msg.Replace("[AMOUNT]", item.Amount.ToString("#,#"));
                                    msg = msg.Replace("[DT]", item.SetteledDate.Value.ToPersian());
                                    msg = msg.Replace("[BALANCEAMOUNT]", document.BalanceAmount.ToString("#,#"));

                                    Business.FacadePayBar.GetNotificationQueueBusiness().SaveNotification(Consts.DOCUMENT_TITLE, msg, NotificationType.OneSignal, NotificationStatus.ToDo, "Document-Do", player.PlayerID);
                                }
                            }
                        }
                        else
                        {
                            item.TryCount += 1;
                            item.NextRunTime = DateTime.Now.AddMinutes(2);
                            item.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogBiz.SaveError("Document-Do", ex);
            }
        }
    }
}

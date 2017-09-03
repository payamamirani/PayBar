using Jobs.TerminalService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities;

namespace Jobs
{
    public class SuccessTxn : IJob
    {
        TerminalServiceClient client = new TerminalServiceClient();

        public void Do()
        {
            try
            {
                var toDo = Data.Models.Generated.PayBar.Txn.Fetch("WHERE RespCode = 0 AND IsSetteled IS NULL AND BusinessDate = CAST(GETDATE() AS DATE) AND MaxTry - TryCount > 0 AND (NextRunTime IS NULL OR NextRunTime < GETDATE())");
                Console.WriteLine(string.Format("\r{0} Count: {1}.", this.GetType().Name, toDo.Count));

                foreach (var item in toDo)
                {
                    var result = client.ConfirmTxn(TerminalInfo.TerminalUserName, TerminalInfo.TerminalPasswrod, TerminalInfo.TerminalNumber, item.Amount.ToLong(), item.RefNum);
                    if (result == 0)
                    {
                        item.IsSetteled = true;
                        item.SetteledDate = DateTime.Now;
                        item.TryCount = 0;
                        item.NextRunTime = null;
                        item.Save();

                        var user = Data.Models.Generated.PayBar.User.FirstOrDefault("WHERE ID = @0", item.UserID);
                        if (!user.IsNull())
                        {
                            var player = Data.Models.Generated.PayBar.Player.FirstOrDefault("WHERE IMEI = @0", user.IMEI);
                            var merchant = Data.Models.Generated.PayBar.Merchant.FirstOrDefault("WHERE ID = @0", item.MerchantID);
                            if (!player.IsNull())
                            {
                                var msg = Consts.SUCCESS_MESSAGE;
                                if (!merchant.IsNull())
                                    msg = msg.Replace("[MerchantName]", merchant.FullName);

                                msg = msg.Replace("[AMOUNT]", item.Amount.ToString("#,#"));
                                msg = msg.Replace("[DT]", item.SetteledDate.Value.ToPersian());

                                msg.SendNotification(Consts.SUCCESS_TITLE, player.PlayerID);
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
            catch (Exception ex)
            {
                LogBiz.SaveError("SuccessTxn-Do", ex);
            }
        }
    }
}

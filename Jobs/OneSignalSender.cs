using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Jobs
{
    public class OneSignalSender : IJob
    {
        public void Do()
        {
            try
            {
                var toDo = Data.Models.Generated.PayBar.NotificationQueue.Fetch("WHERE Status = 1 AND Type = 3");

                foreach (var item in toDo)
                {
                    try
                    {
                        item.Content.SendNotification(item.Title, item.PlayerID);

                        item.Status = NotificationStatus.Done.ToInt();
                        item.ResultMessage = "Success";
                    }
                    catch (Exception ex)
                    {
                        item.Status = NotificationStatus.Error.ToInt();
                        item.ResultMessage = ex.Message;
                    }
                    finally
                    {
                        item.ModifiedBy = "OneSignalSender-Do";
                        item.ModifiedOn = DateTime.Now;

                        item.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                LogBiz.SaveError("NotificationSender-Do", ex);
            }
        }
    }
}

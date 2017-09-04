using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Business.PayBar
{
    public class NotificationQueueBusiness
    {
        public Data.Models.Generated.PayBar.NotificationQueue SaveNotification(string Title, string Content, NotificationType type, NotificationStatus status, string CreatedBy, string to)
        {
            try
            {
                var notif = new Data.Models.Generated.PayBar.NotificationQueue()
                    {
                        Content = Content,
                        CreatedBy = CreatedBy,
                        CreatedOn = DateTime.Now,
                        PlayerID = to,
                        Status = (int)status,
                        Title = Title,
                        Type = (int)type
                    };

                notif.Save();

                return notif;
            }
            catch
            {
                return null;
            }
        }
    }
}

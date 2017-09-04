using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public static class FacadePayBar
    {
        public static Business.PayBar.NotificationQueueBusiness GetNotificationQueueBusiness()
        {
            return new PayBar.NotificationQueueBusiness();
        }
    }
}

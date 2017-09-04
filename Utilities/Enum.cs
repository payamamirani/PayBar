using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public enum UserStatus
    {
        UnComplete = 0,
        Complete = 1
    }

    public enum Prcode
    {
        Purchase = 0,
        Charge = 15,
        Bill = 17
    }

    public enum DocumentType
    {
        Variz = 1,
        Bardasht = 2
    }

    public enum NotificationType
    {
        SMS = 1,
        Email = 2,
        OneSignal = 3
    }

    public enum NotificationStatus
    {
        ToDo = 1,
        Done = 2,
        Error = 3
    }
}

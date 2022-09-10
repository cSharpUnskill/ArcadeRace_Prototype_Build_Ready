using System;
using System.Collections;

namespace Cars
{
    public interface INotificationWrapper
    {
        void RegisterNotification(string title, string body, DateTime fireTime,string channel);
        void ClearNotifications();
        IEnumerator RequestAuthorization();
    }
}
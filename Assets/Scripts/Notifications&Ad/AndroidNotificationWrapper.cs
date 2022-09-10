using System;
using System.Collections;
using Unity.Notifications.Android;

namespace Cars
{
    public class AndroidNotificationWrapper : INotificationWrapper
    {
        public AndroidNotificationWrapper(string[] channels)
        {
            foreach (var channel in channels)
            {
                AndroidNotificationCenter.RegisterNotificationChannel(new AndroidNotificationChannel()
                {
                    Id = channel,
                    Name = channel,
                    Importance = Importance.Default
                });
            }
        }

        public void RegisterNotification(string title, string body, DateTime fireTime, string channel)
        {
            AndroidNotificationCenter.SendNotification(new AndroidNotification()
            {
                Title = title,
                Text = body,
                FireTime = fireTime
            }, channel);
        }

        public void ClearNotifications()
        {
            AndroidNotificationCenter.CancelAllNotifications();
        }

        public IEnumerator RequestAuthorization()
        {
            return null;
        }
    }
}
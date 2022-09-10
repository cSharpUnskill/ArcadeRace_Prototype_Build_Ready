using System;
using System.Collections;
using Unity.Notifications.iOS;
using UnityEngine;

namespace Cars
{
    public class iOSNotificationWrapper : INotificationWrapper
    {
        public void RegisterNotification(string title, string body, DateTime fireTime, string channel)
        {
            iOSNotificationCenter.ScheduleNotification(new iOSNotification()
            {
                Title = title,
                Body = body,
                ShowInForeground = true,
                ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Sound,
                CategoryIdentifier = channel,
                Trigger = new iOSNotificationCalendarTrigger()
                {
                    Year = fireTime.Year,
                    Month = fireTime.Month,
                    Day = fireTime.Day,
                    Minute = fireTime.Minute,
                    Second = fireTime.Second,
                    Repeats = false
                }
            });
        }

        public void ClearNotifications()
        {
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
            iOSNotificationCenter.RemoveAllScheduledNotifications();
        }

        public IEnumerator RequestAuthorization()
        {
            const AuthorizationOption option = AuthorizationOption.Alert | AuthorizationOption.Sound;
            using (AuthorizationRequest request = new AuthorizationRequest(option, false))
            {
                yield return new WaitWhile(() => request.IsFinished);
                Debug.Log("Result " + request.IsFinished);
            }
        }
    }
}
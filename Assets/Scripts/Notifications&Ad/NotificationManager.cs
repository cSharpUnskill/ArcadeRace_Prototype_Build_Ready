using System;
using UnityEngine;

namespace Cars
{
    public class NotificationManager : MonoBehaviour
    {
        private INotificationWrapper _wrapper;
        [SerializeField]
        private string[] _channels = { "default" };

        private void Awake()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _wrapper = new iOSNotificationWrapper();
            }
            else
            {
                _wrapper = new AndroidNotificationWrapper(_channels);
            }

            _wrapper.ClearNotifications();

            DontDestroyOnLoad(gameObject);

            _wrapper.RequestAuthorization();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus) RegisterNotifications();
            else _wrapper.ClearNotifications();
        }

        private void RegisterNotifications()
        {
            _wrapper.RegisterNotification("title", "body", DateTime.Now.AddDays(1), _channels[0]);
        }
    }
}
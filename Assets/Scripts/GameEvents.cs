using UnityEngine;
using System;

namespace Cars
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents Singleton;

        public event Action OnCameraAnimationEnd;

        public event Action ReadyToTransition;

        public event Action OnShowUpMenu;

        public event Action OnBlackout;

        private string _playerName;
        public String PlayerName => _playerName;

        void Awake()
        {
            if (!Singleton)
            {
                Singleton = this;
                DontDestroyOnLoad(gameObject);
                GetComponent<AudioSource>().Play();
            }

            else Destroy(gameObject);

        }

        public void SetPlayerName(string userName) => _playerName = userName;
        public void Blackout() => OnBlackout?.Invoke();
        public void CameraAnimationEnd() => OnCameraAnimationEnd?.Invoke();
        public void ReadyToChangeScene() => ReadyToTransition?.Invoke();
        public void ShowUpMenu() => OnShowUpMenu?.Invoke();
    }
}
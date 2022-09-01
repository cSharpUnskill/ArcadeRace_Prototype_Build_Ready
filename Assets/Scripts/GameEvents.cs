using UnityEngine;
using System;

namespace Cars
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents Singleton;
        public event Action ReadyToTransition;
        public event Action OnShowUpMenu;
        public event Action OnBlackout;

        public string PlayerName { get; private set; }

        private void Awake()
        {
            if (!Singleton)
            {
                Singleton = this;
                DontDestroyOnLoad(gameObject);
            }

            else Destroy(gameObject);

        }

        public void SetPlayerName(string userName) => PlayerName = userName;
        public void Blackout() => OnBlackout?.Invoke();
        public void ReadyToChangeScene() => ReadyToTransition?.Invoke();
        public void ShowUpMenu() => OnShowUpMenu?.Invoke();
    }
}
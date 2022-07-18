using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cars
{
    public class LapTimer : MonoBehaviour
    {
        [SerializeField]
        private Text _text;

        private float _currentTime;

        private bool _lapTimerActive = true;

        private TimeSpan _time;
        public TimeSpan LapTime => _time;
        void Start()
        {
            _currentTime = 0f;
            GameEvents.Singleton.OnCameraAnimationEnd += CameraEndStartAnimation;
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (!_lapTimerActive) return;

            if (gameObject.activeSelf)
            {
                _currentTime = _currentTime + Time.deltaTime;
            }

            _time = TimeSpan.FromSeconds(_currentTime);
            _text.text = _time.ToString(@"mm\:ss\:f");
        }

        public void TurnOffTimer() 
        { 
            _lapTimerActive = false; 
            gameObject.SetActive(false);
        }

        void CameraEndStartAnimation() => gameObject.SetActive(true);
        void OnDestroy() => GameEvents.Singleton.OnCameraAnimationEnd -= CameraEndStartAnimation;
    }
}
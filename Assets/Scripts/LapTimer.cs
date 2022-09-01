using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cars
{
    public class LapTimer : MonoBehaviour
    {
        [SerializeField]
        private Text _text;
        [SerializeField]
        private CameraController _cam;
        private float _currentTime;
        private bool _lapTimerActive = true;
        private TimeSpan _time;
        public TimeSpan LapTime => _time;

        private void Start()
        {
            _currentTime = 0f;
            _cam.StartAnimationEndEvent += CameraEndStartAnimation;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_lapTimerActive) return;

            if (gameObject.activeSelf)
                _currentTime += Time.deltaTime;

            _time = TimeSpan.FromSeconds(_currentTime);
            _text.text = _time.ToString(@"mm\:ss\:f");
        }

        public void TurnOffTimer() 
        { 
            _lapTimerActive = false; 
            gameObject.SetActive(false);
        }

        private void CameraEndStartAnimation() => gameObject.SetActive(true);
        private void OnDestroy() => _cam.StartAnimationEndEvent += CameraEndStartAnimation;
    }
}
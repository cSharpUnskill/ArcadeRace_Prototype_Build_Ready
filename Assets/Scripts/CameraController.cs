using System;
using UnityEngine;

namespace Cars
{
    public class CameraController : MonoBehaviour
    {
        public event Action StartAnimationEndEvent;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private GameObject _timer;
        [SerializeField]
        private Transform _target;
        [SerializeField]
        private int _lerpSpeed;
        private bool _isReady;
        private static readonly int StartTrigger = Animator.StringToHash("StartTrigger");

        public void ReadyToDrive()
        {
            _animator.enabled = false;
            StartAnimationEndEvent?.Invoke();
            _isReady = true;
        }

        public void OnLookaroundEnd()
        {
            _animator.SetTrigger(StartTrigger);
            _timer.SetActive(true);
        }

        private void FixedUpdate()
        {
            if (!_isReady) return;

            transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * _lerpSpeed);
            transform.eulerAngles = new Vector3(_target.eulerAngles.x, _target.eulerAngles.y, 0f);
        }
    }
}
using System;
using UnityEngine;

namespace Cars
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputController _player;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private GameObject _timer;
        [SerializeField]
        private Transform _baseViewSpot;
        [SerializeField]
        private Transform _rearViewSpot;
        private Transform _currentTarget;
        [SerializeField]
        private int _lerpSpeed;
        [SerializeField]
        private bool _isTutorialScene;
        private bool _isReadyToLerp;
        private static readonly int StartTrigger = Animator.StringToHash("StartTrigger");
        private static readonly int Right = Animator.StringToHash("right");
        private static readonly int Back = Animator.StringToHash("back");
        public event Action StartAnimationEndEvent;

        public void ReadyToDrive()
        {
            _animator.enabled = false;
            StartAnimationEndEvent?.Invoke();
            _isReadyToLerp = true;
            _currentTarget = _baseViewSpot;
        }

        public void OnLookaroundEnd()
        {
            _animator.SetTrigger(StartTrigger);
            if (!_isTutorialScene) _timer.SetActive(true);
        }

        public void RearView(bool state)
        {
            ScenarioSwitcher.OnEvent(PlayerAction.MiddleMouseButton);
            _currentTarget = state
                ? _rearViewSpot
                : _baseViewSpot;
        }

        public void ReturnToBasePosition()
        {
            _animator.SetTrigger(Back);
        }

        public void CheckRightWheel()
        {
            _animator.enabled = true;
            _isReadyToLerp = false;
            _animator.SetTrigger(Right);
        }

        public void ReturnedToBasePosition_EditorEvent()
        {
            _animator.enabled = false;
            _isReadyToLerp = true;
            _player.ControlToPlayer();
        }

        private void FixedUpdate()
        {
            if (!_isReadyToLerp) return;

            transform.position = Vector3.Lerp(transform.position, _currentTarget.position, Time.deltaTime * _lerpSpeed);
            transform.eulerAngles = new Vector3(_currentTarget.eulerAngles.x, _currentTarget.eulerAngles.y, 0f);
        }
    }
}
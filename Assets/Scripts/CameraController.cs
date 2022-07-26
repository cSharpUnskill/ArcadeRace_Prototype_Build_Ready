using UnityEngine;

namespace Cars
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private GameObject _timer;
        [SerializeField]
        private Transform _target;
        
        private static readonly int StartTrigger = Animator.StringToHash("StartTrigger");
        public void ReadyToDrive() => GameEvents.Singleton.CameraAnimationEnd();

        public void OnLookaroundEnd()
        {
            _animator.SetTrigger(StartTrigger);
            _timer.SetActive(true);
        }
    }
}
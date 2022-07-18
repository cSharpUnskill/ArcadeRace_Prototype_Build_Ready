using UnityEngine;

namespace Cars
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private GameObject _timer;

        public void ReadyToDrive() => GameEvents.Singleton.CameraAnimationEnd();  
        
        public void OnLookaroundEnd()
        {
            _animator.SetTrigger("StartTrigger");
            _timer.SetActive(true);
        }
    }
}
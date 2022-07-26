using System.Collections;
using UnityEngine;

namespace Cars
{
    public class MainMenuCameraController : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        private static readonly int StartAnimation = Animator.StringToHash("StartAnimation");
        private static readonly int ChangeScene = Animator.StringToHash("ChangeScene");

        IEnumerator Start()
        {
            yield return new WaitForSeconds(2f);
            _animator.SetTrigger(StartAnimation);
        }

        public void BalckoutAnimation() => GameEvents.Singleton.Blackout(); 
        public void OnEndAnimationToTransition() => GameEvents.Singleton.ReadyToChangeScene();
        public void OnEndAnimationShowMenu() => GameEvents.Singleton.ShowUpMenu();
        public void StartTransitionAnimation() => _animator.SetTrigger(ChangeScene);
    }
}
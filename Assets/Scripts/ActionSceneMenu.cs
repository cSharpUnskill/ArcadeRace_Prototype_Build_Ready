using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cars
{
    public class ActionSceneMenu : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputController _player;
        [SerializeField]
        private Animator _animator;

        private static readonly int Hide = Animator.StringToHash("Hide");
        private static readonly int Show = Animator.StringToHash("Show");

        public void Resume_EditorEvent()
        {
            _animator.SetTrigger(Hide);
            Time.timeScale = 1f;
        }

        public void Pause_EditorEvent()
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
            _animator.SetTrigger(Show);
        }

        public void Exit_EditorEvent() => SceneManager.LoadScene(0);
        public void SetMenuNotReady() => _player.SetMenuReadiness(false);
        public void SetMenuReady() => _player.SetMenuReadiness(true);
        public void SetMenuStateOpen() => _player.SetMenuState(true);
        public void SetMenuStateHide() => _player.SetMenuState(false);
    }
}
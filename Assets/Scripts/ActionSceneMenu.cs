using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cars
{
    public class ActionSceneMenu : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputController _player;

        private static readonly int Hide = Animator.StringToHash("Hide");

        public void Resume_EditorEvent()
        {
            GetComponent<Animator>().SetTrigger(Hide);
            Time.timeScale = 1f;
        }

        public void Exit_EditorEvent() => SceneManager.LoadScene(0);
        public void SetMenuNotReady() => _player.SetMenuReadiness(false);
        public void SetMenuReady() => _player.SetMenuReadiness(true);
        public void SetMenuStateOpen() => _player.SetMenuState(true);
        public void SetMenuStateHide() => _player.SetMenuState(false);
    }
}
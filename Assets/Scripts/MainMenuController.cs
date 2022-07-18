using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Cars
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _arcade;
        [SerializeField]
        private GameObject _race;
        [SerializeField]
        private GameObject _menu;
        [SerializeField]
        private Button _goButton;
        [SerializeField]
        private GameObject _nameSettings;
        [SerializeField]
        private TextMeshProUGUI _nameSettingsText;
        [SerializeField]
        private TMP_InputField _inputField;
        [SerializeField]
        private GameObject _nameField;
        [SerializeField]
        private Animator _nameFieldAnimator;
        [SerializeField]
        private Animator _buttonsAnimator;
        [SerializeField]
        private GameObject _blackScreen;
        [SerializeField]
        private Animator _blackScreenAnimator;
        [SerializeField]
        private MainMenuCameraController _camController;

        private Color _grayColor;

        void Start()
        {
            Time.timeScale = 1f;
            _arcade.SetActive(true);
            _race.SetActive(true);

            GameEvents.Singleton.ReadyToTransition += ChangeScene;
            GameEvents.Singleton.OnShowUpMenu += MenuShowUp;
            GameEvents.Singleton.OnBlackout += AnimationBlackout;

            _grayColor = _nameSettingsText.color;
        }

        public void ExitButton_EditorEvent()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        public void BigOkButton_EditorEvent()
        {
            _goButton.interactable = false;
            _nameFieldAnimator.SetTrigger("ScaleDown");
            _blackScreenAnimator.SetTrigger("Whitening");
            _camController.StartTransitionAnimation();
        }

        public void CheckName_EditorEvent(string name)
        {
            if (name.Length > 0 && name.Length <= 8)
            {
                if (CheckExistingNicknames(name))
                {
                    _goButton.interactable = false;
                    _nameSettingsText.text = "User with same nickname\nalready been added";
                    _nameSettingsText.color = Color.red;
                    _nameSettings.SetActive(true);
                }
                else
                {
                    GameEvents.Singleton.SetPlayerName(name);
                    _nameSettings.SetActive(false);
                    Invoke(nameof(EnableGoButton), 0.3f);
                }
            }
            else
            {
                _goButton.interactable = false;
                _nameSettings.SetActive(true);
                _nameSettingsText.color = _grayColor;
                _nameSettingsText.text = "name must be\n1 to 8 characters long";
                _inputField.text = "";
            }
        }

        bool CheckExistingNicknames(string name) => Recorder.Singleton.CurrentLeaderboard().ContainsKey(name);
        
        public void Race_EditorEvent()
        {
            _nameField.SetActive(true);
            _buttonsAnimator.SetTrigger("Upwards");
            _blackScreen.SetActive(true);
        }

        void EnableGoButton() => _goButton.interactable = true;
        private void AnimationBlackout() => _blackScreenAnimator.SetTrigger("Blackout");
        private void MenuShowUp() => _menu.SetActive(true);
        private void ChangeScene() => SceneManager.LoadScene(1);

        void OnDestroy()
        {
            GameEvents.Singleton.ReadyToTransition -= ChangeScene;
            GameEvents.Singleton.OnShowUpMenu -= MenuShowUp;
            GameEvents.Singleton.OnBlackout -= AnimationBlackout;
        }
    }
}

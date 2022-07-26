using UnityEngine;

namespace Cars
{
    public class PlayerInputController : BaseInputController
    {
        private CarControls _controls;

        private bool _isReady;

        private string _name;
        public string Name => _name;

        [SerializeField]
        private Animator _topMenuAnimator;
        [SerializeField]
        private float _wheelsTurnToBasePosSpeed; //однажды преподаватель сказал не использовать безымянные цифры и все делать переменными, ну другого названия придумать не смог :)

        private bool _isMenuOpen;

        private bool _isMenuReady = true;
        private static readonly int Show = Animator.StringToHash("Show");
        private static readonly int Hide = Animator.StringToHash("Hide");

        void Awake()
        {
            _name = GameEvents.Singleton.PlayerName;
            _controls = new CarControls();
            GameEvents.Singleton.OnCameraAnimationEnd += CameraEndAnimation;
            _controls.Player.Pause.performed += Pause_EditorEvent;
            _isMenuOpen = false;
        }

        private void Pause_EditorEvent(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (!_isMenuReady) return;

            if (!_isMenuOpen)
            {
                _isMenuOpen = true;
                Time.timeScale = 0f;
                _topMenuAnimator.SetTrigger(Show);
            }
            else
            {
                _isMenuOpen = false;
                _topMenuAnimator.SetTrigger(Hide);
                Time.timeScale = 1f;
            }
        }

        public void SetMenuReadiness(bool b) => _isMenuReady = b;
        public void SetMenuState(bool b) => _isMenuOpen = b;

        protected override void FixedUpdate()
        {
            if (!_isReady) return;

            Acceleration = _controls.Player.Acceleration.ReadValue<float>();

            CallHandBrake(_controls.Player.Brake.ReadValue<float>());

            var direction = _controls.Player.Rotate.ReadValue<float>();

            if (direction == 0f && Rotate != 0f)
            {
                Rotate = Mathf.Lerp(Rotate, 0f, Time.fixedDeltaTime * _wheelsTurnToBasePosSpeed);
            }
            else
            {
                Rotate = Mathf.Clamp(Rotate + direction * Time.fixedDeltaTime, -1f, 1f);
            }
        }

        public void FinishRace() 
        {
            _isReady = false;
            _isMenuReady = false;
        } 

        void CameraEndAnimation() => _isReady = true;
        
        void OnEnable() => _controls.Player.Enable();  

        void OnDisable() => _controls.Player.Disable();

        void OnDestroy()
        {
            GameEvents.Singleton.OnCameraAnimationEnd -= CameraEndAnimation;
            _controls.Player.Pause.performed -= Pause_EditorEvent;
            _controls.Dispose();
        }
    }
}

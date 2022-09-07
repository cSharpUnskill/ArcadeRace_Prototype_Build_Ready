using UnityEngine;
using UnityEngine.InputSystem;

namespace Cars
{
    public class PlayerInputController : BaseInputController
    {
        [SerializeField]
        private CameraController _cam;
        private CarControls _controls;
        private bool _isReady;
        public string Name { get; private set; }
        [SerializeField]
        private ActionSceneMenu _topMenu;
        [SerializeField]
        private float _wheelsTurnToBasePosSpeed;
        private bool _isMenuOpen;
        private bool _isMenuReady = true;
        private bool _isRearViewDisabled;
        private bool _isAccelerationDisabled;
        private static readonly int Show = Animator.StringToHash("Show");
        private static readonly int Hide = Animator.StringToHash("Hide");

        private void Awake()
        {
            Name = GameEvents.Singleton.PlayerName;
            _controls = new CarControls();
            _cam.StartAnimationEndEvent += CameraEndAnimation;
            _controls.Player.Pause.performed += Pause_EditorEvent;
            _controls.Player.RearView.started += RearView_EditorEvent;
            _controls.Player.RearView.canceled += RearView_EditorEvent;
            _isMenuOpen = false;
        }

        private void RearView_EditorEvent(InputAction.CallbackContext obj)
        {
            if (_isRearViewDisabled) return;
            _cam.RearView(obj.started);
        }

        public void DisableRearView() => _isRearViewDisabled = true;
        public void DisableAcceleration() => _isAccelerationDisabled = true;
        public void ControlToPlayer()
        {
            _isAccelerationDisabled = false;
            _isRearViewDisabled = false;
        }

        private void Pause_EditorEvent(InputAction.CallbackContext obj)
        {
            if (!_isMenuReady) return;

            if (!_isMenuOpen)
            {
                ScenarioSwitcher.OnEvent(PlayerAction.EscButton);
                _isMenuOpen = true;
                Time.timeScale = 0f;
                _topMenu.Pause_EditorEvent();
            }
            else
            {
                _isMenuOpen = false;
                Time.timeScale = 1f;
                _topMenu.Resume_EditorEvent();
            }
        }

        public void SetMenuReadiness(bool b) => _isMenuReady = b;
        public void SetMenuState(bool b) => _isMenuOpen = b;

        protected override void FixedUpdate()
        {
            if (!_isReady) return;

            if (!_isAccelerationDisabled)
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

            switch (Rotate)
            {
                case 1:
                    ScenarioSwitcher.OnEvent(PlayerAction.DButton);
                    break;
                case -1:
                    ScenarioSwitcher.OnEvent(PlayerAction.AButton);
                    break;
            }

            switch (Acceleration)
            {
                case 1:
                    ScenarioSwitcher.OnEvent(PlayerAction.WButton);
                    break;
                case -1:
                    ScenarioSwitcher.OnEvent(PlayerAction.SButton);
                    break;
            }
        }

        public void FinishRace() 
        {
            _isReady = false;
            _isMenuReady = false;
        }

        private void CameraEndAnimation() => _isReady = true;

        private void OnEnable() => _controls.Player.Enable();

        private void OnDisable() => _controls.Player.Disable();

        private void OnDestroy()
        {
            _cam.StartAnimationEndEvent -= CameraEndAnimation;
            _controls.Player.Pause.performed -= Pause_EditorEvent;
            _controls.Player.RearView.started -= RearView_EditorEvent;
            _controls.Player.RearView.canceled -= RearView_EditorEvent;
            _controls.Dispose();
        }
    }
}

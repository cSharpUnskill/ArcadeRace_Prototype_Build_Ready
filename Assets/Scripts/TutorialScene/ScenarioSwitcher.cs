using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Cars
{
    public class ScenarioSwitcher : MonoBehaviour
    {
        [SerializeField, Range(3, 7)]
        private int _delay;
        public int Delay => _delay;
        private Dictionary<Type, Scenario> _dictionary;
        private Scenario _currentScenario;
        public TutorialField TextField { get; private set; }
        public PlayerInputController PlayerInput { get; private set; }
        public CarComponent CarComponent { get; private set; }
        public CameraController Camera { get; private set; }
        public CheckpointComponent Checkpoint { get; private set; }
        public TextMeshProUGUI Speedometer { get; private set; }
        public static PlayerAction CurrentEvent { get; private set; }
        public readonly int FieldShowUp = Animator.StringToHash("showUp");
        public readonly int FieldHighlight = Animator.StringToHash("highlight");

        private void Start()
        {
            TextField = FindObjectOfType<TutorialField>();
            PlayerInput = FindObjectOfType<PlayerInputController>();
            CarComponent = FindObjectOfType<CarComponent>();
            Camera = FindObjectOfType<CameraController>();
            Camera.StartAnimationEndEvent += CameraOnStartAnimationEndEvent;
            Checkpoint = FindObjectOfType<CheckpointComponent>();
            Checkpoint.gameObject.SetActive(false);
            Speedometer = FindObjectOfType<Speedometer>().GetComponent<TextMeshProUGUI>();

            _dictionary = new Dictionary<Type, Scenario>()
            {
                {typeof(WelcomeScenario), new WelcomeScenario(this)},
                {typeof(MiddleMouseButtonScenario), new MiddleMouseButtonScenario(this)},
                {typeof(AButtonScenario), new AButtonScenario(this)},
                {typeof(DButtonScenario), new DButtonScenario(this)},
                {typeof(SButtonScenario), new SButtonScenario(this)},
                {typeof(SpaceButtonScenario), new SpaceButtonScenario(this)},
                {typeof(WButtonScenario), new WButtonScenario(this)},
                {typeof(CheckpointReachedScenario), new CheckpointReachedScenario(this)},
                {typeof(EscButtonScenario), new EscButtonScenario(this)},
                {typeof(GuideEndScenario), new GuideEndScenario(this)},
            };
        }

        private void CameraOnStartAnimationEndEvent()
        {
            SwitchScenario<WelcomeScenario>();
        }

        public void SwitchScenario<S>(float extraDelay = 0) where S : Scenario
        {
            _currentScenario = _dictionary[typeof(S)];
            _currentScenario.SetExtraDelay(extraDelay);
            _currentScenario.Exec();
        }

        private void Update() => _currentScenario?.Update();
        public static void OnEvent(PlayerAction @event) => CurrentEvent = @event;
    }

    public enum PlayerAction
    {
        None,
        MiddleMouseButton,
        AButton,
        DButton,
        SButton,
        SpaceButton,
        WButton,
        Checkpoint,
        EscButton,
    }
}
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Cars
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField]
        private TutorialScenario _tutorial;
        [SerializeField]
        private CameraController _camera;
        [SerializeField, Range(3f, 8f)]
        private float _actionsDelay;
        private static TutorialEvent _currentEvent;
        private Coroutine _timerCor;

        private void Start()
        {
            _camera.StartAnimationEndEvent += CameraOnStartAnimationEndEvent;
        }

        private void CameraOnStartAnimationEndEvent() => StartCoroutine(TutorialThread());

        private IEnumerator TutorialThread()
        {
            foreach (TutorialStep step in _tutorial.Steps)
            {
                if (step.PlayerAction != TutorialEvent.None)
                    yield return new WaitWhile(() => _currentEvent != step.PlayerAction);
                yield return new WaitWhile(() => _timerCor != null);

                step.ManagerAction.Invoke();
                yield return new WaitForSecondsRealtime(_actionsDelay);
            }
        }

        public void StartTimer(float time) => _timerCor = StartCoroutine(Timer(time));
        private IEnumerator Timer(float time)
        {
            yield return new WaitForSecondsRealtime(time);
            _timerCor = null;
        }

        public static void OnEvent(TutorialEvent @event) => _currentEvent = @event;
    }

    [Serializable]
    public class TutorialStep
    {
        public TutorialEvent PlayerAction;
        public UnityEvent ManagerAction;
    }
    [Serializable]
    public enum TutorialEvent
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
        Exit,
    }
}
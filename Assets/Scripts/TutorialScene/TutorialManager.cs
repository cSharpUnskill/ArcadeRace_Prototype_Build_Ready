using System;
using System.Collections;
using UnityEngine;

namespace Cars
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField]
        private TutorialScript _tutorial;
        [SerializeField]
        private CameraController _camera;
        private static TutorialEvent _currentEvent;

        private void Start()
        {
            _camera.StartAnimationEndEvent += CameraOnStartAnimationEndEvent;
        }

        private void CameraOnStartAnimationEndEvent()
        {
            StartCoroutine(TutorialThread());
        }

        private IEnumerator TutorialThread()
        {
            foreach (TutorialStep step in _tutorial.Steps)
            {
                yield return new WaitWhile(() => _currentEvent != step.PlayerAction);
                step.ManagerAction.Invoke();
            }
        }

        public static void OnEvent(TutorialEvent @event)
        {
            _currentEvent = @event;
        }
    }
}
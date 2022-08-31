using System;
using UnityEngine;
using UnityEngine.Events;

namespace Cars
{
    [CreateAssetMenu(fileName = "Tutorial.asset", menuName = "Tutorial/Create Tutorial")]
    public class TutorialScript : ScriptableObject
    {
        public TutorialStep[] Steps;
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
        MiddleMouseButton,
        AButton,
        DButton,
        SButton,
        WButton,
        SpeedMore50,
        Checkpoint,
        EscButton,
        Exit
    }
}
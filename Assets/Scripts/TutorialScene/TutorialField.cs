using UnityEngine;

namespace Cars
{
    public class TutorialField : MonoBehaviour
    {
        private Animator _animator;
        private LocalizationComponent _localization;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _localization = GetComponentInChildren<LocalizationComponent>();
        }

        public void SetText(string text, int trigger)
        {
            _localization.Key = text;
            _animator.SetTrigger(trigger);
        }
    }
}
using UnityEngine;
using TMPro;

namespace Cars
{
    public class TimerController : MonoBehaviour
    {
        private int _index = 3;

        [SerializeField]
        private TextMeshProUGUI _text;

        public void OnAnimationEnd()
        {
            if (_index != 0)
            {
                _index--;
                _text.text = _index.ToString();
            }

            if (_index != 0) return;
            _text.text = "GO!";
            _text.color = Color.green;
            Invoke(nameof(DisableAnimator), 1f);
        }

        private void DisableAnimator() => gameObject.SetActive(false);
    }
}

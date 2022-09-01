using TMPro;
using UnityEngine;

namespace Cars
{
    public class TutorialTextFieldComponent : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        public void SetText(string text) => _text.text = text;
    }
}
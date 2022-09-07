using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Cars
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizationComponent : MonoBehaviour
    {
        private Dictionary<string, string> _parameters;
        [SerializeField]
        private TextMeshProUGUI _text;
        [SerializeField]
        private string _key;

        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                UpdateTerm();
            }
        }

        private void OnEnable()
        {
            Localization.OnLanguageChanged += UpdateTerm;
            UpdateTerm();
        }

        private void OnDisable()
        {
            Localization.OnLanguageChanged -= UpdateTerm;
        }

        private void UpdateTerm()
        {
            _text.font = Localization.SuggestedFont;
            _text.text = Localization.GetTerm(_key, _parameters);
        }

        public void SetParams(Dictionary<string, string> parameters)
        {
            _parameters = parameters;
            UpdateTerm();
        }

        private void Awake()
        {
            if (_text == null)
            {
                _text = GetComponent<TextMeshProUGUI>();
            }
        }

        private void OnValidate()
        {
            if (_text == null)
            {
                _text = GetComponent<TextMeshProUGUI>();
            }
            UpdateTerm();
        }
    }
}
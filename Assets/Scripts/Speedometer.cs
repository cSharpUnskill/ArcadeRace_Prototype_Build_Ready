using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cars
{
    public class Speedometer : MonoBehaviour
    {
        private const float c_converter = 3.6f;
        private Transform _car;
        private Text _text;

        [SerializeField]
        private float _maxSpeed = 300f;
        [SerializeField]
        private Color _minColor;
        [SerializeField]
        private Color _maxColor;

        [SerializeField, Range(0.1f, 1f)]
        private float _delay = 0.3f;

        void Start()
        {
            _text = GetComponent<Text>();
            _car = FindObjectOfType<PlayerInputController>().transform;
            gameObject.SetActive(false);
            GameEvents.Singleton.OnCameraAnimationEnd += CameraEndStartAnimation;
        }

        void CameraEndStartAnimation()
        {
            gameObject.SetActive(true);
            StartCoroutine(Speed());
        }

        public void FinishRace() => gameObject.SetActive(false);
        
        private IEnumerator Speed()
        {
            var prevPos = _car.position;
            while (true)
            {
                var distance = Vector3.Distance(_car.position, prevPos);
                var speed = (float)System.Math.Round(distance / _delay * c_converter);

                _text.text = speed.ToString();
                _text.color = Color.Lerp(_minColor, _maxColor, speed / _maxSpeed);

                prevPos = _car.position;
                yield return new WaitForSeconds(_delay);
            }
        }

        void OnDestroy() => GameEvents.Singleton.OnCameraAnimationEnd -= CameraEndStartAnimation;
    }
}
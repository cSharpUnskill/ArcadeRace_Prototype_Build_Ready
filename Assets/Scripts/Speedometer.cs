using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Cars
{
    public class Speedometer : MonoBehaviour
    {
        private const float c_converter = 3.6f;
        private Transform _car;
        [SerializeField]
        private CameraController _cam;
        [SerializeField]
        private TextMeshProUGUI _text;
        [SerializeField, Range(0.1f, 1f)]
        private float _delay = 0.3f;

        private IEnumerator Start()
        {
            _car = FindObjectOfType<PlayerInputController>().transform;
            _cam.StartAnimationEndEvent += CameraEndStartAnimation;
            yield return null;
            gameObject.SetActive(false);
        }

        private void CameraEndStartAnimation()
        {
            gameObject.SetActive(true);
            StartCoroutine(Speed());
        }

        public void FinishRace() => gameObject.SetActive(false);
        private IEnumerator Speed()
        {
            Vector3 prevPos = _car.position;
            while (true)
            {
                var distance = Vector3.Distance(_car.position, prevPos);
                var speed = (float)System.Math.Round(distance / _delay * c_converter);

                _text.text = speed.ToString(CultureInfo.CurrentCulture);

                prevPos = _car.position;
                yield return new WaitForSeconds(_delay);
            }
        }

        private void OnDestroy() => _cam.StartAnimationEndEvent -= CameraEndStartAnimation;
    }
}
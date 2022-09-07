using System.Collections;
using UnityEngine;

namespace Cars
{
    [RequireComponent(typeof(Rigidbody), typeof(BaseInputController), typeof(WheelsComponent))]
    public class CarComponent : MonoBehaviour
    {
        private Rigidbody _body;
        private BaseInputController _input;
        private WheelsComponent _wheels;

        [SerializeField, Range(5f, 40f)]
        private float _maxSteerAngle = 25f;
        [SerializeField]
        private float _torque = 1500f;
        [SerializeField, Range(0f, float.MaxValue)]
        private float _handBrakeTorque = float.MaxValue;
        [SerializeField]
        private Vector3 _centerOfMass;
        private bool _isCheckingAcceleration;
        private bool _handbrakePressed;

        private void Start()
        {
            _body = GetComponent<Rigidbody>();
            _input = GetComponent<BaseInputController>();
            _wheels = GetComponent<WheelsComponent>();

            _input.OnHandBrake += HandBrake;
            _body.centerOfMass = _centerOfMass;
        }

        private void Update()
        {
            _wheels.UpdateVisual(_input.Rotate * _maxSteerAngle);

            var torque = _input.Acceleration * _torque / 2f;

            foreach (WheelCollider wheel in _wheels.GetRearWheels)
            {
                wheel.motorTorque = torque;
            }
            CheckingAcceleration();
        }

        public void SetCheckingAccelerationState(bool state) => _isCheckingAcceleration = state;
        private void CheckingAcceleration()
        {
            if (!_isCheckingAcceleration) return;
            if (_body.velocity.magnitude > 0.0001f) return;
            if (_handbrakePressed)
                ScenarioSwitcher.OnEvent(PlayerAction.SpaceButton);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.TransformPoint(_centerOfMass), 0.5f);
        }

        private void HandBrake(float isOn)
        {
            if (isOn != 0)
            {
                _handbrakePressed = true;
                foreach (WheelCollider wheel in _wheels.GetAllWheels)
                {
                    wheel.brakeTorque = _handBrakeTorque;
                    wheel.motorTorque = 0f;
                }
            }
            else
            {
                foreach (WheelCollider wheel in _wheels.GetAllWheels)
                {
                    wheel.brakeTorque = 0f;
                }
            }
        }

        public void RemoteHandBrake()
        {
            StartCoroutine(Lerp(2f));
            foreach (WheelCollider wheel in _wheels.GetAllWheels) wheel.brakeTorque = _handBrakeTorque;
        }

        private IEnumerator Lerp(float lerpTime)
        {
            var time = 0f;

            while (time < 1f)
            {
                _body.velocity = Vector3.Lerp(_body.velocity, Vector3.zero, time * time);
                time += Time.deltaTime / lerpTime;
                yield return null;
            }
            _body.velocity = Vector3.zero;
        }

        private void OnDestroy()
        {
            if (_input != null)
                _input.OnHandBrake -= HandBrake;
        }
    }
}
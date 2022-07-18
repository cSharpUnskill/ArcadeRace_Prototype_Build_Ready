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

        void Start()
        {
            _body = GetComponent<Rigidbody>();
            _input = GetComponent<BaseInputController>();
            _wheels = GetComponent<WheelsComponent>();

            _input.OnHandBrake += HandBrake;
            _body.centerOfMass = _centerOfMass;
        }

        void FixedUpdate()
        {
            _wheels.UpdateVisual(_input.Rotate * _maxSteerAngle);

            var torque = _input.Acceletartion * _torque / 2f;

            foreach (var wheel in _wheels.GetRearWheels)
            {
                wheel.motorTorque = torque;
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.TransformPoint(_centerOfMass), 0.5f);
        }

        private void HandBrake(float isOn)
        {
            if (isOn != 0)
            {
                foreach (var wheel in _wheels.GetAllWheels)
                {
                    wheel.brakeTorque = _handBrakeTorque;
                    wheel.motorTorque = 0f;
                }
            }
            else
            {
                foreach (var wheel in _wheels.GetAllWheels)
                {
                    wheel.brakeTorque = 0f;
                }
            }
        }

        public void RemoteHandBrake()
        {
            StartCoroutine(Lerp(2f));
            foreach (var wheel in _wheels.GetAllWheels) wheel.brakeTorque = _handBrakeTorque;
        }

        IEnumerator Lerp(float LerpTime)
        {
            var time = 0f;

            while (time < 1f)
            {
                _body.velocity = Vector3.Lerp(_body.velocity, Vector3.zero, time * time);
                time += Time.deltaTime / LerpTime;
                yield return null;
            }
            _body.velocity = Vector3.zero;
        }

        void OnDestroy()
        {
            if (_input != null)
                _input.OnHandBrake -= HandBrake;
        }
    }
}
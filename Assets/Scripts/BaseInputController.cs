using UnityEngine;
using System;

namespace Cars
{
    public abstract class BaseInputController : MonoBehaviour
    {
        public float Acceleration { get; protected set; }
        public float Rotate { get; protected set; }

        public event Action<float> OnHandBrake;

        protected abstract void FixedUpdate();

        protected void CallHandBrake(float val) => OnHandBrake?.Invoke(val);
    }
}

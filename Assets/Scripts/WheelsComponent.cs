using UnityEngine;

namespace Cars
{
    public class WheelsComponent : MonoBehaviour
    {
        private Transform[] _frontMeshes;
        private Transform[] _rearMeshes;
        private Transform[] _allMeshes;

        private WheelCollider[] _frontWheels;
        private WheelCollider[] _rearWheels;
        private WheelCollider[] _allWheels;

        [SerializeField]
        private Transform _leftFrontMesh;
        [SerializeField]
        private Transform _rightFrontMesh;
        [SerializeField]
        private Transform _leftRearMesh;
        [SerializeField]
        private Transform _rightRearMesh;

        [SerializeField, Space]
        private WheelCollider _leftFrontWheel;
        [SerializeField]
        private WheelCollider _rightFrontWheel;
        [SerializeField]
        private WheelCollider _leftRearWheel;
        [SerializeField]
        private WheelCollider _rightRearWheel;

        public WheelCollider[] GetAllWheels => _allWheels;
        public WheelCollider[] GetFrontWheels => _frontWheels;
        public WheelCollider[] GetRearWheels => _rearWheels;

        private void Start()
        {
            _frontMeshes = new[] { _leftFrontMesh, _rightFrontMesh };
            _rearMeshes = new[] { _leftRearMesh, _rightRearMesh };
            _allMeshes = new[] { _leftFrontMesh, _rightFrontMesh, _leftRearMesh, _rightRearMesh };

            _frontWheels = new[] { _leftFrontWheel, _rightFrontWheel };
            _rearWheels = new[] { _leftRearWheel, _rightRearWheel };
            _allWheels = new[] { _leftFrontWheel, _rightFrontWheel, _leftRearWheel, _rightRearWheel };
        }

        public void UpdateVisual(float angle)
        {
            for (int i = 0; i < _frontWheels.Length; i++)
            {
                _frontWheels[i].steerAngle = angle;
                _frontWheels[i].GetWorldPose(out var pos, out var rot);
                _frontMeshes[i].position = pos;
                _frontMeshes[i].rotation = rot;

                _rearWheels[i].GetWorldPose(out pos, out rot);
                _rearMeshes[i].position = pos;
                _rearMeshes[i].rotation = rot;
            }
        }
    }
}

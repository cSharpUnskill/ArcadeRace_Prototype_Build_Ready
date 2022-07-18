using System.Collections;
using UnityEngine;

namespace Cars
{
    public class BotInputController : BaseInputController
    {
        private int _index = 0;

        [SerializeField]
        private BotTargetPoint[] _points;

        private bool _isReady = false;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(12f);
            _isReady = true;   
        }

        protected override void FixedUpdate()
        {
            if (!_isReady) return;

            Acceletartion = 1f;

            var direction = GetAngle();

            if (direction == 0f && Rotate != 0f)
            {
                Rotate = Rotate > 0f ? Rotate - Time.fixedDeltaTime : Rotate + Time.fixedDeltaTime;
            }
            else
            {
                Rotate = Mathf.Clamp(Rotate + direction * Time.fixedDeltaTime, -1f, 1f);
            }
        }

        private float GetAngle()
        {
            var targetPos = _points[_index].transform.position;
            targetPos.y = transform.position.y;
            var direction = targetPos - transform.position;
            return -Vector3.SignedAngle(direction, transform.forward, Vector3.up);
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<BotTargetPoint>() != null)
                _index++;

        }
    }
}
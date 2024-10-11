using UnityEngine;

namespace PlayerComponent
{
    public class EnemyTarget : MonoBehaviour
    {
        public Transform EnemyTransform;

        private int _frameDelay = 0;

        private void OnTriggerEnter(Collider col)
        {
            if (EnemyTransform)
                EnemyTransform.GetComponent<Outline>().OutlineWidth = 0;

            EnemyTransform = col.GetComponent<Transform>();
            col.GetComponent<Outline>().OutlineWidth = 5;
        }

        private void FixedUpdate()
        {
            if (_frameDelay++ < 10 || !EnemyTransform || Vector3.Distance(transform.position, EnemyTransform.position) < 15) return;

            EnemyTransform.GetComponent<Outline>().OutlineWidth = 0;
            EnemyTransform = null;
            _frameDelay = 0;
        }
    }
}

using UnityEngine;

namespace PlayerComponent
{
    public class EnemyTarget : MonoBehaviour
    {
        [HideInInspector] public Transform EnemyTransform;

        private void OnTriggerEnter(Collider col)
        {
            if (EnemyTransform)
                EnemyTransform.GetComponent<Outline>().OutlineWidth = 0f;

            col.GetComponent<Outline>().OutlineWidth = 5f;
            EnemyTransform = col.GetComponent<Transform>();
        }

        private void OnTriggerExit(Collider col)
        {
            col.GetComponent<Outline>().OutlineWidth = 0f;
            EnemyTransform = null;
        }
    }
}

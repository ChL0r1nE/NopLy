using UnityEngine;

namespace PlayerComponent
{
    public class EnemyTarget : MonoBehaviour
    {
        public Transform EnemyTransform { get; private set; }

        private UI.HealthBar _healthBar;

        private void Start() => _healthBar = FindObjectOfType<UI.HealthBar>();

        private void OnTriggerEnter(Collider col)
        {
            if (EnemyTransform)
                EnemyTransform.GetComponent<Outline>().OutlineWidth = 0f;

            col.GetComponent<Outline>().OutlineWidth = 5f;
            EnemyTransform = col.GetComponent<Transform>();
            _healthBar.SetEnemyHealth(col.GetComponent<Enemy.Health>());
        }

        private void OnTriggerExit(Collider col)
        {
            col.GetComponent<Outline>().OutlineWidth = 0f;
            _healthBar.SetEnemyHealth(null);
            EnemyTransform = null;
        }
    }
}

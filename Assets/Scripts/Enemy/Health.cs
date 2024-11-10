using UnityEngine;

namespace Enemy
{
    public class Health : MonoBehaviour
    {
        public float HealthValue { get; private set; }

        [SerializeField] private GameObject _loot;
        [SerializeField] private GameObject _enemy;
        [SerializeField] private int _maxHealth;
        [SerializeField] private DamageText _damageText;

        private void Start() => HealthValue = _maxHealth;

        public int GetMaxHealth() => _maxHealth;

        public void TakeDamage(int damage)
        {
            _damageText.ResetTextDelay(damage);

            if (HealthValue == -1) return;

            HealthValue -= damage;

            if (HealthValue > 0) return;

            Instantiate(_loot, transform.position, Quaternion.identity);
            Destroy(_enemy);
        }
    }
}

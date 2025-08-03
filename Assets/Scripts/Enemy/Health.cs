using UnityEngine;

namespace Enemy
{
    public interface IEnemySpawn
    {
        public void EnemyLeft(int number);
    }

    public class Health : MonoBehaviour
    {
        public int GetMaxHealth() => _maxHealth;

        public float HealthValue { get; private set; }

        [SerializeField] private GameObject _loot;
        [SerializeField] private GameObject _enemy;
        [SerializeField] private int _maxHealth;
        [SerializeField] private DamageText _damageText;
        private IEnemySpawn _iEnemySpawn;
        private int _number;

        private void Start() => HealthValue = _maxHealth;

        public void SetSpawn(IEnemySpawn iEnemySpawn, int number)
        {
            _iEnemySpawn = iEnemySpawn;
            _number = number;
        }

        public void TakeDamage(int damage)
        {
            _damageText.ResetTextDelay(damage);

            if (HealthValue == -1) return;

            HealthValue -= damage;

            if (HealthValue > 0) return;

            _iEnemySpawn.EnemyLeft(_number);
            Instantiate(_loot, transform.position, Quaternion.identity);
            Destroy(_enemy);
        }
    }
}

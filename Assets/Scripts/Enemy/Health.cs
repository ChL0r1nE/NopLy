using UnityEngine;

namespace Enemy
{
    public interface IEnemyLeft
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
        private IEnemyLeft _iEnemyLeft;
        private int _number;

        private void Start() => HealthValue = _maxHealth;

        public void SetSpawn(IEnemyLeft iEnemyleft, int number = 0)
        {
            _iEnemyLeft = iEnemyleft;
            _number = number;
        }

        public void TakeDamage(int damage)
        {
            _damageText.ResetTextDelay(damage);

            if (HealthValue == -1) return;

            HealthValue -= damage;

            if (HealthValue > 0) return;

            _iEnemyLeft.EnemyLeft(_number);
            Instantiate(_loot, transform.position, Quaternion.identity);
            Destroy(_enemy);
        }
    }
}

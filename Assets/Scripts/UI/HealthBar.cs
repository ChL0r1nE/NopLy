using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _playerHealthStripe;
        [SerializeField] private Image _enemyHealthStripe;
        [SerializeField] private Image _enemyHealthBack;

        private Enemy.Health _enemyHealth;
        private int _maxEnemyHealth;
        private bool _isEnemy;

        public float PlayerAmount
        {
            get => _playerHealthStripe.fillAmount;
            set => _playerHealthStripe.fillAmount = value;
        }

        private void Update()
        {
            if (_enemyHealth)
                _enemyHealthStripe.fillAmount = _enemyHealth.HealthValue / _maxEnemyHealth;
            else if (_isEnemy)
            {
                _isEnemy = false;
                _enemyHealthBack.gameObject.SetActive(false);
            }
        }

        public void SetEnemyHealth(Enemy.Health health)
        {
            _isEnemy = health;
            _enemyHealth = health;
            _maxEnemyHealth = _enemyHealth.GetMaxHealth();
            _enemyHealthBack.gameObject.SetActive(_enemyHealth);
        }
    }
}
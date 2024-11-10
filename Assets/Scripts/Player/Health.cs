using UnityEngine;

namespace PlayerComponent
{
    public class Health : MonoBehaviour
    {
        public float HealthValue;

        [SerializeField] private Armor PlayerArmor;
        [SerializeField] private float _maxHealth; //ser
        private UI.HealthBar _healthBar;

        public void HealthChange(float change) => HealthValue += change >= 0 ? change : change / PlayerArmor.ArmorValue;

        private void Start() => _healthBar = FindObjectOfType<UI.HealthBar>();

        private void Update() => _healthBar.PlayerAmount = Mathf.MoveTowards(_healthBar.PlayerAmount, HealthValue / _maxHealth, Time.deltaTime);
    }
}
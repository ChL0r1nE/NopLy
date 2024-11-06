using UnityEngine.UI;
using UnityEngine;

namespace PlayerComponent
{
    public class Health : MonoBehaviour
    {
        public float HealthValue
        {
            get => _health;
            set => _health = Mathf.Clamp(value >= _health ? value : _health + ((value - _health) / PlayerArmor.ArmorValue), 0, _maxHealth);
        }

        [SerializeField] private Image _hpStripe;
        [SerializeField] private Armor PlayerArmor;
        [SerializeField] private float _maxHealth; //ser
        [SerializeField] private float _health; //ser

        private void Update() => _hpStripe.fillAmount = Mathf.MoveTowards(_hpStripe.fillAmount, _health / _maxHealth, Time.deltaTime);
    }
}
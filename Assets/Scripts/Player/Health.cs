using UnityEngine.UI;
using UnityEngine;

namespace PlayerComponent
{
    public class Health : MonoBehaviour
    {
        public void HealthChange(float change) => _health += change >= 0 ? change : change / PlayerArmor.ArmorValue;

        [SerializeField] private Image _hpStripe;
        [SerializeField] private Armor PlayerArmor;
        [SerializeField] private float _maxHealth; //ser
        [SerializeField] private float _health; //ser

        private void Update() => _hpStripe.fillAmount = Mathf.MoveTowards(_hpStripe.fillAmount, _health / _maxHealth, Time.deltaTime);
    }
}
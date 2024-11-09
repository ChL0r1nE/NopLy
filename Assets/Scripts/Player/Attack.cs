using UnityEngine;

namespace PlayerComponent
{
    public class Attack : MonoBehaviour
    {
        public int WeaponDamage { private get; set; }

        [SerializeField] private Weapon _playerWeapon;

        private void OnTriggerEnter(Collider col)
        {
            col.GetComponent<Enemy.Health>().TakeDamage(WeaponDamage);
            _playerWeapon.WeaponUse(1);
        }
    }
}
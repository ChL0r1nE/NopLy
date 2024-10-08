using UnityEngine;

namespace PlayerComponent
{
    public class Attack : MonoBehaviour
    {
        public int WeaponDamage;

        private void OnTriggerEnter(Collider col) => col.GetComponent<EnemyHealth>().TakeDamage(WeaponDamage);
    }
}
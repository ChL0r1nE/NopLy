using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    public void SetWeaponDamage(int i) => _weaponDamage = i;

    private int _weaponDamage;

    private void OnTriggerEnter(Collider col) => col.GetComponent<EnemyHealthScript>().TakeDamage(_weaponDamage);
}

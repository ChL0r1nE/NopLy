using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    private WeaponInfo _weaponInfo;

    public void SetWeaponInfo(WeaponInfo info) => _weaponInfo = info;

    private void OnTriggerEnter(Collider col) => col.GetComponent<EnemyHealthScript>().TakeDamage(_weaponInfo.Damage);
}

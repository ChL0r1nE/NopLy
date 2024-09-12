using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    private WeaponInfo _weaponInfo;

    private void OnTriggerEnter(Collider col) => col.gameObject.GetComponent<EnemyHealthScript>().TakeDamage(_weaponInfo.Damage);

    public void SetWeaponInfo(WeaponInfo info) => _weaponInfo = info;
}

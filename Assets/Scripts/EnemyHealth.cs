using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject _loot;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private DamageText _damageText;
    [SerializeField] private int _health;

    public void TakeDamage(int damage)
    {
        _damageText.ResetTextDelay(damage);

        if (_health == -1) return;

        _health -= damage;

        if (_health > 0) return;

        Instantiate(_loot, transform.position, Quaternion.identity);
        Destroy(_enemy);
    }
}

using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] private DamageText _damageText;
    [SerializeField] private int _health;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        _damageText.ResetTextDelay(damage);

        if (_health <= 0)
            Destroy(_enemy);
    }
}

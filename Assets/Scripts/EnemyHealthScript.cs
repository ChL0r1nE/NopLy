using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] private DamageTextScript _damageTextScript;
    [SerializeField] private int _health;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        _damageTextScript.ResetTextDelay(damage);

        if (_health <= 0)
            Destroy(_enemy);
    }
}

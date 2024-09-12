using UnityEngine;

public class EnemyAttackScript : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
            col.gameObject.GetComponent<PlayerHealthScript>().TakeDamage(_damage);
    }
}

using System.Collections;
using UnityEngine;

public class EnemyMoveScript : MonoBehaviour
{
    public float Speed;

    [SerializeField] private Animator _animator;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackTime;
    private Transform _playerTransform;
    private float _distance;
    private bool _isAttack;

    private bool _isMoveDistance => _distance < 20;
    private bool _isAttackDistance => _distance < 1;

    private void Start() => _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    private void FixedUpdate()
    {
        _animator.SetBool("IsMove", _isMoveDistance);
        _distance = Vector3.Distance(transform.position, _playerTransform.position);

        if (_isAttackDistance)
        {
            _animator.SetTrigger("Attack");

            if (!_isAttack)
                StartCoroutine(WaitAttack());
        }
        else if (_isMoveDistance)
        {
            transform.LookAt(_playerTransform.position);
            transform.Translate(Vector3.forward * Speed);
        }
    }

    IEnumerator WaitAttack()
    {
        _isAttack = true;
        yield return new WaitForSeconds(_attackTime);

        if (Vector3.Distance(transform.position, _playerTransform.position) < 1.5f)
            _playerTransform.gameObject.GetComponent<PlayerHealthScript>().TakeDamage(_damage);

        _isAttack = false;
    }
}

using System.Collections;
using UnityEngine;

public class EnemyFollowingScript : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private int _damage;
    [SerializeField] public float _speed;
    [SerializeField] private float _attackTime;
    private WaitForSeconds _halfAttackTime;
    private Transform _playerTransform;
    private float _distance;
    private bool _isAttack;

    private bool _isMoveDistance => _distance < 20;
    private bool _isAttackDistance => _distance < 1;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _halfAttackTime = new WaitForSeconds(_attackTime / 2);
    }

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
            transform.Translate(Vector3.forward * _speed);
        }
    }

    IEnumerator WaitAttack()
    {
        _isAttack = true;
        yield return _halfAttackTime;

        if (Vector3.Distance(transform.position, _playerTransform.position) < 1.5f)
            _playerTransform.gameObject.GetComponent<PlayerHealthScript>().TakeDamage(_damage);

        yield return _halfAttackTime;
        _isAttack = false;
    }
}

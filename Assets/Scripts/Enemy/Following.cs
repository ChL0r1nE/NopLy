using UnityEngine;

namespace Enemy
{
    public class Following : MonoBehaviour
    {
        private bool IsMoveDistance => _distance < 20f;
        private bool IsAttackDistance => _distance < _attackDistance;

        [SerializeField] private Animator _animator;
        [SerializeField] private float _speed;
        [SerializeField] private float _attackTime;
        [SerializeField] private float _attackDistance;
        [SerializeField] private int _damage;
        private Transform _playerTransform;
        private PlayerComponent.Health _playerHealth;
        private Vector3 _targetDelta;
        private int _frameDelay;
        private float _distance;
        private float _angle;
        private bool _isAttack;

        private void Start()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            _playerHealth = _playerTransform.GetComponent<PlayerComponent.Health>();
        }

        private void FixedUpdate()
        {
            if (_isAttack) return;

            if (_frameDelay++ >= 10)
            {
                _frameDelay = 0;
                _distance = Vector3.Distance(transform.position, _playerTransform.position);
                _animator.SetBool("IsMove", IsMoveDistance && !IsAttackDistance);

                if (IsAttackDistance)
                {
                    _isAttack = true;
                    _animator.SetBool("IsAttack", true);
                }
            }

            if (IsMoveDistance)
            {
                _targetDelta = transform.position - _playerTransform.position;
                _angle = Mathf.Atan(_targetDelta.x / _targetDelta.z) * 57;

                if (_targetDelta.z > 0)
                    _angle += 180;

                transform.rotation = Quaternion.Euler(Vector3.up * Mathf.Repeat(_angle, 359));

                if (!IsAttackDistance)
                    transform.Translate(Vector3.forward * _speed);
            }
        }

        private void Attack()
        {
            if (Vector3.Distance(transform.position, _playerTransform.position) < _attackDistance)
                _playerHealth.HealthChange(-_damage);
        }

        private void EndAttack()
        {
            _distance = Vector3.Distance(transform.position, _playerTransform.position);
            _isAttack = IsAttackDistance;

            _animator.SetBool("IsAttack", _isAttack);
        }
    }
}

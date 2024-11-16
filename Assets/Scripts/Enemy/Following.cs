using UnityEngine;

namespace Enemy
{
    public class Following : MonoBehaviour
    {
        private void EndAttack() => _animator.SetBool("IsAttack", Vector3.Distance(transform.position, _playerTransform.position) < _attackDistance);

        [SerializeField] private Animator _animator;
        [SerializeField] private float _speed;
        [SerializeField] private float _attackDistance;
        [SerializeField] private int _damage;
        private Transform _playerTransform;
        private PlayerComponent.Health _playerHealth;
        private Vector3 _targetDelta;
        private int _frameDelay;
        private float _distance;
        private float _angle;

        private void Start()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            _playerHealth = _playerTransform.GetComponent<PlayerComponent.Health>();
        }

        private void FixedUpdate()
        {
            if (_animator.GetBool("IsAttack")) return;

            if (_frameDelay++ >= 15)
            {
                _frameDelay = 0;
                _distance = Vector3.Distance(transform.position, _playerTransform.position);
                _animator.SetFloat("MoveBlend", _distance < 20f ? 1f : 0f);

                if (_distance < _attackDistance)
                    _animator.SetBool("IsAttack", true);
            }

            if (_distance < 20f)
            {
                _targetDelta = transform.position - _playerTransform.position;
                _angle = Mathf.Atan(_targetDelta.x / _targetDelta.z) * 57;

                if (_targetDelta.z > 0)
                    _angle += 180;

                transform.rotation = Quaternion.Euler(Vector3.up * Mathf.Repeat(_angle, 359));

                if (_distance >= _attackDistance)
                    transform.Translate(Vector3.forward * _speed);
            }
        }

        private void Attack()
        {
            if (Vector3.Distance(transform.position, _playerTransform.position) < _attackDistance)
                _playerHealth.HealthChange(-_damage);
        }
    }
}

using UnityEngine;

namespace PlayerComponent
{
    public class Move : MonoBehaviour
    {
        private bool IsRun => Input.GetKey(KeyCode.LeftShift);

        private bool Forward => Input.GetKey(KeyCode.W);

        private bool Back => Input.GetKey(KeyCode.S);

        private bool Left => Input.GetKey(KeyCode.A);

        private bool Right => Input.GetKey(KeyCode.D);

        public float Speed;

        [SerializeField] private Animator _animator;
        private Rigidbody _rigidbody;
        private float _targetRotationAngle = -1;
        private float _targetBlend = 0;
        private bool _isAttack = false;
        private bool _isLunge = false;

        private bool LungeOff() => _isLunge = false;

        private void Start() => _rigidbody = GetComponent<Rigidbody>();

        private void FixedUpdate()
        {
            if (_isLunge) return;

            if (_isAttack)
            {
                _isAttack = Mathf.Abs(transform.rotation.eulerAngles.y - _targetRotationAngle) > 5;

                if (_isAttack)
                {
                    transform.rotation = Quaternion.Euler(0, Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, _targetRotationAngle, 12f), 0);
                    _rigidbody.velocity = transform.forward * (IsRun ? Speed : Left || Right || Forward || Back ? Speed / 2 : 0);
                }
                else
                    _animator.SetTrigger("Strike");

                return;
            }

            _animator.SetFloat("BlendRun", Mathf.MoveTowards(_animator.GetFloat("BlendRun"), _targetBlend, 0.05f));

            if (_targetRotationAngle == -1f)
            {
                _targetBlend = 0f;

                if (!_isLunge)
                    _rigidbody.velocity = Vector3.zero;

                return;
            }

            _targetBlend = IsRun ? 1f : 0.5f;

            transform.rotation = Quaternion.Euler(0, Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, _targetRotationAngle, 7f), 0);
            _rigidbody.velocity = transform.forward * (IsRun ? Speed * 2f : Speed);
        }

        private void Update()
        {
            if (_isAttack || _isLunge) return;

            if (Forward)
                _targetRotationAngle = Right ? 45f : Left ? 315f : 0f;
            else if (Back)
                _targetRotationAngle = Right ? 135f : Left ? 225f : 180f;
            else
                _targetRotationAngle = Right ? 90f : Left ? 270f : -1f;
        }

        public void AddImpulse()
        {
            _rigidbody.AddForce(transform.forward * 1500);
            _isLunge = true;
        }

        public void SetTargetRotation(float target)
        {
            _targetRotationAngle = target;
            _isAttack = true;
        }
    }
}
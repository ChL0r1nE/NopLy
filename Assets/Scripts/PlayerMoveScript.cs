using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    public void AddImpulse() => _rigidbody.AddForce(transform.forward * 1500);

    private bool IsRun => Input.GetKey(KeyCode.LeftShift);

    private bool Forward => Input.GetKey(KeyCode.W);

    private bool Back => Input.GetKey(KeyCode.S);

    private bool Left => Input.GetKey(KeyCode.A);

    private bool Right => Input.GetKey(KeyCode.D);

    public float Speed;

    [SerializeField, HideInInspector] private bool _isLunge = false;
    [SerializeField] private Animator _animator;
    private Rigidbody _rigidbody;
    private float _targetRotationAngle = -1;
    private bool _isAttack = false;

    private void Start() => _rigidbody = GetComponent<Rigidbody>();

    private void FixedUpdate()
    {
        if (_isLunge) return;

        if (_isAttack)
        {
            _isAttack = !Mathf.Approximately(transform.rotation.eulerAngles.y / 10, _targetRotationAngle / 10f);

            if (!_isAttack)
                _animator.SetTrigger("Strike");

            transform.rotation = Quaternion.Euler(0, Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, _targetRotationAngle, 12f), 0);
            return;
        }

        if (_targetRotationAngle == -1f)
        {
            _animator.SetInteger("MoveModifier", 0);

            if (!_isLunge)
                _rigidbody.velocity = Vector3.zero;

            return;
        }

        _animator.SetInteger("MoveModifier", IsRun ? 2 : 1);

        transform.rotation = Quaternion.Euler(0, Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, _targetRotationAngle, 7f), 0);
        _rigidbody.velocity = transform.forward * (IsRun ? Speed : Speed / 2);
    }

    private void Update()
    {
        if (_isAttack) return;

        if (Forward)
            _targetRotationAngle = Right ? 45f : Left ? 315f : 0f;
        else if (Back)
            _targetRotationAngle = Right ? 135f : Left ? 225f : 180f;
        else
            _targetRotationAngle = Right ? 90f : Left ? 270f : -1f;
    }

    public void SetTargetRotation(float target)
    {
        _targetRotationAngle = Mathf.Repeat(target, 359);
        _isAttack = true;
    }
}

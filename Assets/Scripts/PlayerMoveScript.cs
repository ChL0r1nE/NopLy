using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    public Animator Animator;
    public float Speed;

    private Rigidbody _rigidbody;
    private Attack _attack;
    private float _targetRotationAngle = -1;
    private float _nowRotation;
    private bool _isAttack = false;
    private bool _isLunge = false;

    private bool _forward => Input.GetKey(KeyCode.W);
    private bool _back => Input.GetKey(KeyCode.S);
    private bool _right => Input.GetKey(KeyCode.D);
    private bool _left => Input.GetKey(KeyCode.A);
    private bool _isRun => Input.GetKey(KeyCode.LeftShift);

    public void SetTargetRotation(float target, Attack attack)
    {
        _attack = attack;
        _targetRotationAngle = target;
        _isAttack = true;
    }

    public void Lunge()
    {
        _isLunge = true;
        _rigidbody.AddForce(transform.forward * 1500);

        Invoke("LungeOff", 0.5f);
    }

    private void LungeOff() => _isLunge = false;

    private void Start() => _rigidbody = GetComponent<Rigidbody>();

    private void Update()
    {
        if (_isAttack) return;

        _targetRotationAngle = -1f;

        if (_forward)
            _targetRotationAngle = _right ? 45f : _left ? 315f : 0f;
        else if (_back)
            _targetRotationAngle = _right ? 135f : _left ? 225f : 180f;
        else if (_right)
            _targetRotationAngle = 90f;
        else if (_left)
            _targetRotationAngle = 270f;
    }

    private void FixedUpdate()
    {
        if (_isAttack)
        {
            _isAttack = !Mathf.Approximately(transform.rotation.eulerAngles.y, _targetRotationAngle);

            if (Mathf.Approximately(transform.rotation.eulerAngles.y / 20f, _targetRotationAngle / 20f))
                _attack.Invoke();

            _nowRotation = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, _targetRotationAngle, 15f);
            transform.localRotation = Quaternion.Euler(0, _nowRotation, 0);
            return;
        }

        if (_targetRotationAngle == -1f)
        {
            Animator.SetBool("IsWalk", false);
            Animator.SetBool("IsRun", false);

            if (!(_isAttack || _isLunge))
                _rigidbody.velocity = Vector3.zero;

            return;
        }

        Animator.SetBool("IsRun", _isRun);
        Animator.SetBool("IsWalk", !_isRun);

        _nowRotation = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, _targetRotationAngle, 7f);

        transform.localRotation = Quaternion.Euler(0, _nowRotation, 0);
        _rigidbody.velocity = transform.forward * (_isRun ? Speed : Speed / 2f);
    }
}

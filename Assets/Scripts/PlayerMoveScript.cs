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

        if (_forward)
            _targetRotationAngle = _right ? 45f : _left ? 315f : 0f;
        else if (_back)
            _targetRotationAngle = _right ? 135f : _left ? 225f : 180f;
        else if (_right || _left)
            _targetRotationAngle = _right ? 90f : 270f;
        else
            _targetRotationAngle = -1f;
    }

    private void FixedUpdate()
    {
        if (_isAttack)
        {
            _isAttack = !Mathf.Approximately(transform.rotation.eulerAngles.y, _targetRotationAngle);

            if (Mathf.Approximately(transform.rotation.eulerAngles.y / 20f, _targetRotationAngle / 20f))
                _attack.Invoke();

            _nowRotation = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, _targetRotationAngle, 15f);
            transform.rotation = Quaternion.Euler(0, _nowRotation, 0);
            return;
        }

        if (_targetRotationAngle == -1f)
        {
            Animator.SetInteger("MoveModifier", 0); //LugeMoveModifier 3

            if (!_isLunge)
                _rigidbody.velocity = Vector3.zero;

            return;
        }

        Animator.SetInteger("MoveModifier", _isRun ? 2 : 1);

        _nowRotation = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, _targetRotationAngle, 7f);

        transform.rotation = Quaternion.Euler(0, _nowRotation, 0);
        _rigidbody.velocity = transform.forward * (_isRun ? Speed : Speed / 2f);
    }
}

using UnityEngine;

public delegate void Attack();

public class PlayerWeaponScript : MonoBehaviour
{
    public WeaponInfo WeaponInfo;
    public Attack Attack;

    [SerializeField] private PlayerAttackScript _playerAttackScript;
    [SerializeField] private PlayerEnemyTargetScript _playerEnemyTargetScript;
    [SerializeField] private MeshFilter _meshFilter;
    private Transform _playerTransform;
    private Animator _animator;
    private PlayerMoveScript _playerMoveScript;
    private Vector3 _nowRotation;

    private void Start()
    {
        Attack = Strike;
        _animator = GetComponent<Animator>();
        _playerTransform = GetComponent<Transform>();
        _playerMoveScript = GetComponent<PlayerMoveScript>();
        SetWeapon(WeaponInfo);
    }

    public void SetWeapon(WeaponInfo info)
    {
        WeaponInfo = info;
        _playerAttackScript.SetWeaponInfo(info);

        _animator.SetTrigger(info.WeaponType.ToString());
        _meshFilter.mesh = info.WeaponMesh;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (WeaponInfo.WeaponType == WeaponType.Shaft)
            {
                _playerMoveScript.Lunge();
                Strike();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_playerEnemyTargetScript.EnemyTransform)
            {
                _nowRotation = _playerTransform.rotation.eulerAngles;

                _playerTransform.LookAt(_playerEnemyTargetScript.EnemyTransform.position);
                _playerMoveScript.SetTargetRotation(_playerTransform.rotation.eulerAngles.y, Attack);
                _playerTransform.rotation = Quaternion.Euler(_nowRotation);
            }
            else
                Strike();
        }
    }

    private void Strike() => _animator.SetTrigger("Strike");
}

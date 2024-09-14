using UnityEngine;

public delegate void Attack();

public class PlayerWeaponScript : MonoBehaviour
{
    public WeaponInfo WeaponInfo;
    public Animator Animator;
    public Attack Attack;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerMoveScript _playerMoveScript;
    [SerializeField] private PlayerAttackScript _playerAttackScript;
    [SerializeField] private EnemyActivateScript _enemyActivateScript;
    [SerializeField] private bool _isMainWeapon;
    private MeshFilter _meshFilter;
    private Vector3 _nowRotation;

    private void Start()
    {
        Attack = Strike;
        _meshFilter = GetComponent<MeshFilter>();

        if (_isMainWeapon)
            _playerAttackScript.SetWeaponInfo(WeaponInfo);
    }

    public void SetWeapon(WeaponInfo info)
    {
        WeaponInfo = info;
        _playerAttackScript.SetWeaponInfo(info);

        Animator.SetTrigger(info.WeaponType.ToString());
        _meshFilter.mesh = info.WeaponMesh;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && _isMainWeapon)
        {
            if (WeaponInfo.WeaponType == WeaponType.Shaft)
            {
                _playerMoveScript.Lunge();
                Strike();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && _isMainWeapon)
        {
            if (_enemyActivateScript.EnemyTransform)
            {
                _nowRotation = _playerTransform.rotation.eulerAngles;

                _playerTransform.LookAt(_enemyActivateScript.EnemyTransform.position);
                _playerMoveScript.SetTargetRotation(_playerTransform.rotation.eulerAngles.y, Attack);
                _playerTransform.rotation = Quaternion.Euler(_nowRotation);
            }
            else
                Strike();
        }
    }

    private void Strike() => Animator.SetTrigger("Strike");
}

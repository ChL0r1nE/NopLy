using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TargetActivateScript))]
public class GetLootStrategy : Strategy
{
    [SerializeField] private GameObject Loot;
    [SerializeField] private Animator _animator;
    [SerializeField] private TargetActivateScript _targetActivateScript;
    [SerializeField] private bool _destroyObjectAfter;
    private Vector2 _barScale = new(0, 0.04f);

    [Header("HardLoot")]
    [SerializeField] private SpriteRenderer _barRenderer;
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private int _health = 1;
    private PlayerWeaponScript _playerWeaponScript;
    private bool _isHardLoot = false;

    private void Start()
    {
        if (_health == 1) return;

        _isHardLoot = true;
        _playerWeaponScript = FindObjectOfType<PlayerWeaponScript>();
    }

    public override void Interact()
    {
        if (_isHardLoot)
        {
            if (_health <= 0 || _playerWeaponScript.WeaponInfo.WeaponType != _weaponType) return;

            _playerWeaponScript.RotateToTransforn(transform.position);
            _barScale.x = --_health * 0.09f;
            _barRenderer.size = _barScale;
        }
        else
            _health--;

        if (_health != 0) return;

        _animator.SetTrigger("Collect");
        _targetActivateScript.SetOffActive();

        Instantiate(Loot, transform.position, Quaternion.identity).GetComponent<LootScript>().SetRandomCount(4);

        if (_destroyObjectAfter)
            Destroy(gameObject, 2f);
    }
}

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

    private void Start()
    {
        if (_health != 1)
            _playerWeaponScript = FindObjectOfType<PlayerWeaponScript>();
    }

    public override void Interact() //ReFactor??
    {
        if (_barRenderer && _playerWeaponScript.WeaponInfo.WeaponType != _weaponType) return;

        _health--;

        if (_health > 0)
        {
            _playerWeaponScript.RotateToTransforn(transform.position);
            _barScale.x = _health * 0.09f;
            _barRenderer.size = _barScale;
        }

        if (_health != 0) return;

        _animator.SetTrigger("Collect");
        _targetActivateScript.SetOffActive();

        Instantiate(Loot, transform.position, Quaternion.identity);

        if (_destroyObjectAfter)
            Destroy(gameObject, 2f);
    }
}

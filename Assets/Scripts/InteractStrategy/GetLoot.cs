using UnityEngine;

namespace Interact
{
    public class GetLoot : AbstractInteract
    {
        [SerializeField] private GameObject _loot;
        [SerializeField] private Animator _animator;
        [SerializeField] private TargetActivate _targetActivate;
        [SerializeField] private bool _destroyObjectAfter;
        private Vector2 _barScale = new(0, 0.04f);

        [Header("HardLoot")]
        [SerializeField] private SpriteRenderer _barRenderer;
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private int _health = 1;
        private PlayerComponent.Weapon _playerWeapon;
        private bool _isHardLoot = false;

        private void Start()
        {
            if (_health == 1) return;

            _isHardLoot = true;
            _playerWeapon = FindObjectOfType<PlayerComponent.Weapon>();
        }

        public override void Interact()
        {
            if (_isHardLoot)
            {
                if (_health <= 0 || _playerWeapon.WeaponInfo.WeaponType != _weaponType) return;

                _playerWeapon.RotateToTransforn(transform.position);
                _barScale.x = --_health * 0.09f;
                _barRenderer.size = _barScale;
            }
            else
                _health--;

            if (_health != 0) return;

            _animator.SetTrigger("Collect");
            _targetActivate.SetOffActive();

            Instantiate(_loot, transform.position, Quaternion.identity).GetComponent<Loot>().Slot.Count = Random.Range(1, 4);

            if (_destroyObjectAfter)
                Destroy(gameObject, 2f);
        }
    }
}

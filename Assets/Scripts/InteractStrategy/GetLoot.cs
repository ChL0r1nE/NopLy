using System.Collections;
using System.Linq;
using UnityEngine;

namespace Interact
{
    public class GetLoot : AbstractInteract
    {
        [SerializeField] private MeshRenderer[] _meshRenderers;

        [SerializeField] private Texture _mainTexture;
        [SerializeField] private Loot _loot;
        [SerializeField] private TargetActivate _targetActivate;
        [SerializeField] private bool _destroyObjectAfter;
        private Material _destroyMaterial;
        private Vector2 _barScale = new(0, 0.04f);

        [Header("HardLoot")]
        [SerializeField] private SpriteRenderer _barRenderer;
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private int _health = 1;
        private PlayerComponent.Weapon _playerWeapon;
        private float _resetInteract = 0f;
        private float _timer = 0f;
        private bool _isHardLoot = false;

        private void Start()
        {
            _destroyMaterial = Instantiate(Resources.Load<Material>(@"Materials/Destroy"));
            _destroyMaterial.SetTexture("_MainTex", _mainTexture);

            foreach (MeshRenderer renderer in _meshRenderers)
            {
                var materials = renderer.sharedMaterials.ToList();
                materials.Add(_destroyMaterial);
                renderer.materials = materials.ToArray();
            }

            if (_health == 1) return;

            _isHardLoot = true;
            _playerWeapon = FindObjectOfType<PlayerComponent.Weapon>();
        }

        private void Update()
        {
            if (_isHardLoot)
                _resetInteract += Time.deltaTime;
        }

        public override void Interact()
        {
            if (_isHardLoot)
            {
                if (_health <= 0 || _resetInteract < 1.5f || _playerWeapon.WeaponInfo?.WeaponType != _weaponType) return;

                _resetInteract = 0;
                _playerWeapon.RotateToTransforn(transform.position);

                _barScale.x = --_health * 0.09f;
                _barRenderer.size = _barScale;
            }
            else
                _health--;

            if (_health != 0) return;

            _targetActivate.SetOffActive();
            Instantiate(_loot, transform.position, Quaternion.identity).Slot.Count = Random.Range(1, 4);

            if (_destroyObjectAfter)
                StartCoroutine("Destroy");
        }

        IEnumerator Destroy()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                _timer += Time.deltaTime;

                if (_timer >= 1f)
                    Destroy(gameObject);
                else
                    _destroyMaterial.SetFloat("_Amount", _timer);
            }
        }
    }
}

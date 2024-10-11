using UnityEngine;

namespace Skill
{
    public class Spawn : AbstractSkill
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private PlayerComponent.Weapon _playerWeapon;

        public override void Execute()
        {
            Instantiate(_gameObject, transform.position + transform.forward + transform.right, Quaternion.identity);
            _playerWeapon.WeaponInfo = null;
        }
    }
}

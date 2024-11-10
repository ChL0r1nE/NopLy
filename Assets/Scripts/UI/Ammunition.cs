using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class Ammunition : AbstractInventory
    {
        [SerializeField] private Image _weaponEnduranceImage;
        [SerializeField] private Image _weaponImage;
        private PlayerComponent.Weapon _playerWeapon;
        private PlayerComponent.Armor _playerArmor;
        private PlayerComponent.Buff _playerBuff;
        private CameraFollowing _cameraFollowing;

        private void Start()
        {
            _inventoryTargetPosition.x = -310;
            _cameraFollowing = FindObjectOfType<CameraFollowing>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                SwitchOpen(true);
        }

        public void SetPlayerArmor(PlayerComponent.Armor armor) => _playerArmor = armor;

        public void SetPlayerWeapon(PlayerComponent.Weapon weapon) => _playerWeapon = weapon;

        public void SetPlayerBuff(PlayerComponent.Buff buff) => _playerBuff = buff;

        public override void SwitchOpen(bool baseOpen)
        {
            base.SwitchOpen(baseOpen);
            _cameraFollowing.SwitchToPlayer();
        }

        public override void UpdateMenu(Slot[] slots)
        {
            for (int i = 0; i < 5; i++)
            {
                _images[i].gameObject.SetActive(_playerArmor.Armors[i].Sprite);

                if (_playerArmor.Armors[i].Sprite)
                    _images[i].sprite = _playerArmor.Armors[i].Sprite;
            }
        }

        public override void DeleteItem(int id)
        {
            int remain;

            if (id == 0)
                _inventoryPlayer.AddItem(new WeaponSlot(_playerWeapon.GetInfoWeapon(), 1, _playerWeapon.WeaponEndurance), out remain);
            else
                _inventoryPlayer.AddItem(new(_playerArmor.Armors[id - 1], 1), out remain);

            if (remain != 0) return;

            if (id == 0)
                _playerWeapon.SetWeaponSlot(null);
            else
            {
                _playerArmor.SetDefaultArmor(id - 1);
                _images[id - 1].gameObject.SetActive(false);
            }
        }

        public override void AddItem(Slot slot, out int countRemain)
        {
            countRemain = slot.Count - 1;

            switch (slot.Item.Type)
            {
                case Info.ItemType.Potion:
                    _playerBuff.AddBuff((slot.Item as Info.Potion).Buff);
                    break;
                case Info.ItemType.Weapon:
                    if (_playerWeapon.GetInfoWeapon())
                        _inventoryPlayer.AddItem(new WeaponSlot(_playerWeapon.GetInfoWeapon(), 1, _playerWeapon.WeaponEndurance), out _);

                    _playerWeapon.SetWeaponSlot(slot as WeaponSlot);
                    break;
                case Info.ItemType.Armor:
                    _playerArmor.SetArmor(slot.Item as Info.Armor);
                    break;
                default:
                    countRemain++;
                    break;
            }
        }

        public void UpdateWeaponImage(Info.Weapon weapon)
        {
            _images[5].gameObject.SetActive(_playerWeapon.GetInfoWeapon());
            _images[5].sprite = _playerWeapon.GetInfoWeapon()?.Sprite;
            _weaponImage.sprite = weapon.Sprite;
        }

        public void UpdateEndurance(int endurance, int maxEndurance)
        {
            _weaponEnduranceImage.fillAmount = (float)endurance / maxEndurance;
            _weaponEnduranceImage.color = Color.green * ((float)endurance / maxEndurance) + Color.red * (1 - (float)endurance / maxEndurance);
        }
    }
}

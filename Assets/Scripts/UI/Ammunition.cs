using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class Ammunition : AbstractInventory
    {
        public void SetPlayerWeapon(PlayerComponent.Weapon weapon) => _playerWeapon = weapon;

        public void SetPlayerArmor(PlayerComponent.Armor armor) => _playerArmor = armor;

        public void SetPlayerBuff(PlayerComponent.Buff buff) => _playerBuff = buff;

        [SerializeField] private Image _weaponEnduranceImage;
        [SerializeField] private Image _weaponImage;
        private PlayerComponent.Weapon _playerWeapon;
        private PlayerComponent.Armor _playerArmor;
        private PlayerComponent.Buff _playerBuff;
        private CameraFollowing _cameraFollowing;

        private void Start()
        {
            _cameraFollowing = FindObjectOfType<CameraFollowing>();
            _inventoryTargetPosition.x = 0;
            _inventoryPosition.x = 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                SwitchOpen(true);
        }

        public override void AddItem(ref Slot slot)
        {
            switch (slot.Item.Type)
            {
                case Info.ItemType.Potion:
                    _playerBuff.AddBuff((slot.Item as Info.Potion).Buff);
                    slot.Count--;
                    break;
                case Info.ItemType.Weapon:
                    if (_playerWeapon.GetInfoWeapon())
                    {
                        WeaponSlot weaponSlot = new(slot.Item, 1, (slot as WeaponSlot).Endurance);
                        slot = new WeaponSlot(_playerWeapon.GetInfoWeapon(), 1, _playerWeapon.Endurance);
                        _playerWeapon.SetWeaponSlot(weaponSlot);

                        break;
                    }

                    _playerWeapon.SetWeaponSlot(new(slot.Item, slot.Count, (slot as WeaponSlot).Endurance));
                    slot.Count = 0;
                    break;
                case Info.ItemType.Armor:
                    if(_playerArmor.GetArmor((int)(slot.Item as Info.Armor).ArmorType))
                    {
                        Info.Armor armorInfo = _playerArmor.GetArmor((int)(slot.Item as Info.Armor).ArmorType);
                        _playerArmor.SetArmor(slot.Item as Info.Armor);
                        slot = new(armorInfo, 1);

                        break;
                    }

                    _playerArmor.SetArmor(slot.Item as Info.Armor);
                    slot.Count = 0;
                    break;
            }
        }

        public override void DeleteItem(int id)
        {
            Slot slot;

            if (id == 0)
                slot = new WeaponSlot(_playerWeapon.GetInfoWeapon(), 1, _playerWeapon.Endurance);
            else
                slot = new(_playerArmor.Armors[id - 1], 1);

            _inventoryPlayer.AddItem(ref slot);

            if (id == 0)
                _playerWeapon.SetWeaponSlot(null);
            else
            {
                _playerArmor.SetDefaultArmor(id - 1);
                _images[id - 1].gameObject.SetActive(false);
            }
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

        public override void SwitchOpen(bool baseOpen)
        {
            base.SwitchOpen(baseOpen);
            _cameraFollowing.SwitchToPlayer();
        }

        public void UpdateWeaponImage(Info.Weapon weapon)
        {
            _images[5].gameObject.SetActive(_playerWeapon.GetInfoWeapon());
            _images[5].sprite = _playerWeapon.GetInfoWeapon()?.Sprite;
            _weaponImage.sprite = weapon.Sprite;
        }

        public void UpdateEndurance(float endurance)
        {
            _weaponEnduranceImage.fillAmount = endurance;
            _weaponEnduranceImage.color = Color.green * endurance + Color.red * (1 - endurance);
        }
    }
}

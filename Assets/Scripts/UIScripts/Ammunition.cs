using UnityEngine;

namespace InventoryUI
{
    public class Ammunition : AbstractInventory
    {
        [SerializeField] private ArmorInfo[] _defaultArmors; //MoveTo Player.Armor

        public PlayerComponent.Armor PlayerArmor;

        [SerializeField] private PlayerComponent.Buff _playerBuff;
        [SerializeField] private CameraFollowing _cameraFollowing;
        private PlayerComponent.Weapon _playerWeapon;

        private void Start()
        {
            _inventoryTargetPosition.x = -310;
            PlayerArmor = FindObjectOfType<PlayerComponent.Armor>();
            _playerWeapon = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponent.Weapon>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                SwitchOpen(true);
        }

        public override void SwitchOpen(bool baseOpen)
        {
            base.SwitchOpen(baseOpen);
            _cameraFollowing.SwitchToPlayer();
        }

        public override void AddItem(Slot slot, out int countRemain)
        {
            countRemain = slot.Count - 1;

            switch (slot.Info.Type)
            {
                case ItemType.Potion:
                    PotionInfo potionInfo = slot.Info as PotionInfo;
                    _playerBuff.AddBuff(potionInfo.Buff);
                    break;
                case ItemType.Weapon:
                    WeaponInfo weaponInfo = slot.Info as WeaponInfo;

                    if (_playerWeapon.WeaponInfo)
                        _inventoryPlayer.AddItem(new(_playerWeapon.WeaponInfo, 1), out int remain);

                    _playerWeapon.WeaponInfo = weaponInfo;
                    UpdateMenu(null);
                    break;
                case ItemType.Armor:
                    ArmorInfo armorInfo = slot.Info as ArmorInfo;
                    PlayerArmor.SetArmor(armorInfo);
                    UpdateMenu(null);
                    break;
                default:
                    countRemain = slot.Count;
                    break;
            }
        }

        public override void DeleteItem(int id)
        {
            _inventoryPlayer.AddItem(new(id == 0 ? _playerWeapon.WeaponInfo : PlayerArmor.Armors[id - 2], 1), out int remain);

            if (remain != 0) return;

            if (id == 0)
                _playerWeapon.WeaponInfo = null;
            else
                PlayerArmor.SetArmor(_defaultArmors[id - 2]);

            _images[id].gameObject.SetActive(false);
        }

        public override void UpdateMenu(Slot[] slots)
        {
            _images[0].gameObject.SetActive(_playerWeapon.WeaponInfo);

            if (_playerWeapon.WeaponInfo)
                _images[0].sprite = _playerWeapon.WeaponInfo.Sprite;

            for (int i = 0; i < 5; i++)
            {
                _images[i + 2].gameObject.SetActive(PlayerArmor.Armors[i].Sprite);

                if (PlayerArmor.Armors[i].Sprite)
                    _images[i + 2].sprite = PlayerArmor.Armors[i].Sprite;
            }
        }
    }
}

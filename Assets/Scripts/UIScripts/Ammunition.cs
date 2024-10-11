using UnityEngine;

namespace InventoryUI
{
    public class Ammunition : AbstractInventory
    {

        [SerializeField] private ArmorInfo[] _defaultArmors;

        public PlayerComponent.Armor PlayerArmor;

        [SerializeField] private CameraFollowing _cameraFollowing;
        [SerializeField] private PlayerComponent.Buff _playerBuff;
        private PlayerComponent.Weapon _playerMainWeapon;

        private void Start()
        {
            _inventoryTargetPosition.x = -310;
            PlayerArmor = FindObjectOfType<PlayerComponent.Armor>();
            _playerMainWeapon = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponent.Weapon>();
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

                    if (_playerMainWeapon.WeaponInfo)
                        _inventoryPlayer.AddItem(new(_playerMainWeapon.WeaponInfo, 1), out int remain);

                    if (weaponInfo.WeaponPlace == WeaponPlace.Main)
                        _playerMainWeapon.WeaponInfo = weaponInfo;

                    break;
                case ItemType.Armor:
                    ArmorInfo armorInfo = slot.Info as ArmorInfo;
                    PlayerArmor.SetArmor(armorInfo);
                    break;
                default:
                    countRemain = slot.Count;
                    break;
            }

            UpdateMenu(null);
        }

        public override void DeleteItem(int id)
        {
            if (id == 0)
            {
                _inventoryPlayer.AddItem(new(_playerMainWeapon.WeaponInfo, 1), out int remain);

                if (remain == 0)
                    _playerMainWeapon.WeaponInfo = null;
            }
            else
            {
                _inventoryPlayer.AddItem(new(PlayerArmor.Armors[id - 2], 1), out int remain);

                if (remain == 0)
                    PlayerArmor.SetArmor(_defaultArmors[id - 2]);
            }

            UpdateMenu(null); //TargetlyOnOff, not all
        }

        public override void UpdateMenu(Slot[] slots)
        {
            _images[0].gameObject.SetActive(_playerMainWeapon.WeaponInfo);

            if (_playerMainWeapon.WeaponInfo)
                _images[0].sprite = _playerMainWeapon.WeaponInfo.Sprite;

            for (int i = 0; i < 5; i++)
            {
                _images[i + 2].gameObject.SetActive(PlayerArmor.Armors[i].Sprite);

                if (PlayerArmor.Armors[i].Sprite)
                    _images[i + 2].sprite = PlayerArmor.Armors[i].Sprite;
            }
        }
    }
}

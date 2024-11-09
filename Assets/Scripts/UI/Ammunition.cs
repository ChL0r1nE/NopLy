using UnityEngine;

namespace UI
{
    public class Ammunition : AbstractInventory
    {
        [SerializeField] private Info.Armor[] _defaultArmors;

        public global::Player.Armor PlayerArmor;

        [SerializeField] private global::Player.Buff _playerBuff;
        [SerializeField] private CameraFollowing _cameraFollowing;
        private global::Player.Weapon _playerWeapon;

        private void Start()
        {
            _inventoryTargetPosition.x = -310;
            PlayerArmor = FindObjectOfType<global::Player.Armor>();
            _playerWeapon = GameObject.FindGameObjectWithTag("Player").GetComponent<global::Player.Weapon>();
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

        public override void UpdateMenu(Slot[] slots)
        {
            for (int i = 0; i < 5; i++)
            {
                _images[i].gameObject.SetActive(PlayerArmor.Armors[i].Sprite);

                if (PlayerArmor.Armors[i].Sprite)
                    _images[i].sprite = PlayerArmor.Armors[i].Sprite;
            }
        }

        public override void DeleteItem(int id)
        {
            int remain;

            if (id == 0)
                _inventoryPlayer.AddItem(new WeaponSlot(_playerWeapon.GetInfoWeapon(), 1, _playerWeapon.WeaponEndurance), out remain);
            else
                _inventoryPlayer.AddItem(new(PlayerArmor.Armors[id - 1], 1), out remain);

            if (remain != 0) return;

            if (id == 0)
                _playerWeapon.SetWeaponSlot(null);
            else
            {
                PlayerArmor.SetArmor(_defaultArmors[id - 1]);
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
                    PlayerArmor.SetArmor(slot.Item as Info.Armor);
                    UpdateMenu(null);
                    break;
                default:
                    countRemain++;
                    break;
            }
        }

        public void UpdateWeaponImage()
        {
            _images[5].gameObject.SetActive(_playerWeapon.GetInfoWeapon());
            _images[5].sprite = _playerWeapon.GetInfoWeapon()?.Sprite;
        }
    }
}

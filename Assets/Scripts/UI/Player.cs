using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class Player : AbstractInventory
    {
        [SerializeField] private Image[] _quickPanelImages = new Image[10];
        private int[] _quickPanelLink = new int[10];

        public PlayerComponent.Inventory PlayerInventory;

        [SerializeField] private Ammunition _inventoryAmmunition;
        private AbstractInventory _secondInventory;
        private int _enterID;

        public void SetEnterID(int id) => _enterID = id;

        private void Start()
        {
            for (int i = 0; i < 10; i++)
                _quickPanelLink[i] = -1;

            _inventoryTargetPosition.x = -400;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                InteractQuickPanel(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                InteractQuickPanel(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                InteractQuickPanel(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                InteractQuickPanel(3);
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                InteractQuickPanel(4);
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                InteractQuickPanel(5);
            else if (Input.GetKeyDown(KeyCode.Alpha7))
                InteractQuickPanel(6);
            else if (Input.GetKeyDown(KeyCode.Alpha8))
                InteractQuickPanel(7);
            else if (Input.GetKeyDown(KeyCode.Alpha9))
                InteractQuickPanel(8);
            else if (Input.GetKeyDown(KeyCode.Alpha0))
                InteractQuickPanel(9);
        }

        public override void AddItem(Slot slot, out int _slotCount)
        {
            PlayerInventory.AddItem(slot, out int remain);
            _slotCount = remain;
        }

        public override void DeleteItem(int id)
        {
            if (PlayerInventory.Slots[id].GetType() == typeof(WeaponSlot))
            {
                WeaponSlot nowWeapon = PlayerInventory.Slots[id] as WeaponSlot;
                WeaponSlot slot = new(PlayerInventory.Slots[id].Item, PlayerInventory.Slots[id].Count, nowWeapon.Endurance);
                _secondInventory.AddItem(slot, out int remain);
                PlayerInventory.SetSlotCount(id, remain);
            }
            else
            {
                Slot slot = new(PlayerInventory.Slots[id].Item, PlayerInventory.Slots[id].Count);
                _secondInventory.AddItem(slot, out int remain);
                PlayerInventory.SetSlotCount(id, remain);
            }
        }

        public void SetSecondInventory(AbstractInventory inventory)
        {
            _secondInventory = inventory;
            _isOpen = inventory;

            _inventoryTargetPosition.y = _isOpen ? 0 : -1000;
        }

        private void InteractQuickPanel(int selectID)
        {
            if (_enterID == -1)
            {
                if (_quickPanelLink[selectID] == -1) return;

                Slot slot = PlayerInventory.Slots[_quickPanelLink[selectID]];

                _inventoryAmmunition.AddItem(slot, out int remain);
                PlayerInventory.SetSlotCount(_quickPanelLink[selectID], remain);

                if (remain != 0) return;

                _quickPanelImages[selectID].enabled = false;
                _quickPanelLink[selectID] = -1;

                return;
            }

            for (int i = 0; i < 10; i++)
            {
                if (_quickPanelLink[i] != _enterID) continue;

                _quickPanelImages[i].enabled = false;
                _quickPanelLink[i] = -1;
            }

            _quickPanelLink[selectID] = _enterID;
            _quickPanelImages[selectID].sprite = PlayerInventory.Slots[_enterID].Item.Sprite;
            _quickPanelImages[selectID].enabled = true;
        }
    }
}
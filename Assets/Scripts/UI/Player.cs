using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class Player : AbstractInventory
    {
        public void SetEnterID(int id) => _enterID = id;

        [SerializeField] private Image[] _quickPanelImages;
        private int[] _quickPanelLinks = new int[10];

        public PlayerComponent.Inventory PlayerInventory;

        [SerializeField] private Ammunition _inventoryAmmunition;
        private AbstractInventory _secondInventory;
        private Slot _reserveSlot;
        private Slot _slot;
        private int _enterID;
        private bool _isStart = false;
        private bool _isAmmunition = false;

        private void Start()
        {
            for (int i = 0; i < 10; i++)
                _quickPanelLinks[i] = -1;

            _inventoryTargetPosition.x = -400;
            _isStart = true;
        }

        private void Update()
        {
            for (int i = 48; i < 58; i++)
            {
                if (Input.GetKeyDown((KeyCode)i))
                    InteractQuickPanel(i == 48 ? 9 : i - 49);
            }
        }

        public override void AddItem(Slot slot, out int _slotCount)
        {
            PlayerInventory.AddItem(slot, out int remain);
            _slotCount = remain;
        }

        public override void DeleteItem(int id)
        {
            int remain;

            if (PlayerInventory.Slots[id].GetType() == typeof(WeaponSlot))
            {
                _slot = new WeaponSlot(PlayerInventory.Slots[id].Item, PlayerInventory.Slots[id].Count, (PlayerInventory.Slots[id] as WeaponSlot).Endurance);
                _reserveSlot = new WeaponSlot(_slot.Item, _slot.Count, (_slot as WeaponSlot).Endurance);
            }
            else
            {
                _slot = new(PlayerInventory.Slots[id].Item, PlayerInventory.Slots[id].Count);
                _reserveSlot = new(_slot.Item, _slot.Count);
            }

            PlayerInventory.SetSlotCount(id, 0);

            if (_isAmmunition)
            {
                _inventoryAmmunition.AddItem(_slot, out remain);
                _isAmmunition = false;
            }
            else
                _secondInventory.AddItem(_slot, out remain);

            if (remain != 0)
                PlayerInventory.AddItem(_reserveSlot, out _);
        }

        public override void UpdateMenu(Slot[] slots)
        {
            base.UpdateMenu(slots);

            for (int i = 0; i < 10; i++)
            {
                _quickPanelImages[i].enabled = _isStart && _quickPanelLinks[i] != -1 && slots[_quickPanelLinks[i]].Count != 0;

                if (_quickPanelImages[i].enabled)
                    _quickPanelImages[i].sprite = slots[_quickPanelLinks[i]].Item.Sprite;
            }
        }

        public void SetSecondInventory(AbstractInventory inventory)
        {
            _secondInventory = inventory;
            _inventoryTargetPosition.y = inventory ? 0 : -1000;
        }

        private void InteractQuickPanel(int selectID)
        {
            if (_enterID != -1)
            {
                for (int i = 0; i < 10; i++)
                    if (_quickPanelLinks[i] == _enterID)
                    {
                        _quickPanelLinks[i] = -1;
                        _quickPanelImages[i].enabled = false;

                        break;
                    }

                _quickPanelLinks[selectID] = _enterID;
                _quickPanelImages[selectID].enabled = true;
                _quickPanelImages[selectID].sprite = PlayerInventory.Slots[_enterID].Item.Sprite;

                return;
            }

            if (!_quickPanelImages[selectID].enabled) return;

            _isAmmunition = true;
            DeleteItem(_quickPanelLinks[selectID]);
        }
    }
}
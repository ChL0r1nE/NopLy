using UnityEngine;

namespace Interact
{
    [RequireComponent(typeof(Animator))]
    public class Storage : AbstractInteract
    {
        public Slot[] Slots;

        [SerializeField] private Animator _animator;
        [SerializeField] private SlotsSerialize _slotsSerialize;
        [SerializeField] private string _saveID;
        private UI.Storage _inventoryStorage;
        private bool _isOpen = false;

        private void OnDisable() => _slotsSerialize.SerializeData(Slots, $"Storage{_saveID}");

        private void Start()
        {
            _inventoryStorage = FindObjectOfType<UI.Storage>();
            _slotsSerialize.DeserializeData(Slots, $"Storage{_saveID}");
        }

        private void OnTriggerExit()
        {
            if (_isOpen)
                _inventoryStorage.SwitchOpen(false);
        }

        public override void Interact()
        {
            _inventoryStorage.StorageStrategy = this;
            _inventoryStorage.SwitchOpen(true);

            if (_isOpen)
                _inventoryStorage.UpdateMenu(Slots);
        }

        public void SetOpen(bool isOpen)
        {
            _isOpen = isOpen;
            _animator.SetBool("IsOpen", _isOpen);
        }

        public void AddItem(Slot slot, out int countRemain)
        {
            foreach (Slot foreachSlot in Slots)
            {
                if (!foreachSlot.Item || foreachSlot.Item.ID != slot.Item.ID) continue;

                foreachSlot.AddCount(slot.Count, out int remain);
                slot.Count = remain;

                if (remain != 0) continue;

                countRemain = 0;
                _inventoryStorage.UpdateMenu(Slots);
                return;
            }

            countRemain = slot.Count;

            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].Item) continue;

                Slots[i] = slot;
                countRemain = 0;
                _inventoryStorage.UpdateMenu(Slots);
                return;
            }
        }

        public void SetSlotCount(int id, int count)
        {
            Slots[id].Count = count;
            _inventoryStorage.UpdateMenu(Slots);
        }
    }
}

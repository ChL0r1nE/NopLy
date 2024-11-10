using UnityEngine;

namespace Interact
{
    [RequireComponent(typeof(Animator))]
    public class Storage : AbstractInteract
    {
        public Slot[] Slots;

        public void SetOpen(bool isOpen)
        {
            _isOpen = isOpen;
            _animator.SetBool("IsOpen", _isOpen);
        }

        [SerializeField] private Animator _animator;
        [SerializeField] private SlotsSerialize _slotsSerialize;
        [SerializeField] private string _saveID;
        private UI.Storage _inventoryStorage;
        private bool _isOpen = false;

        private void OnDisable() => _slotsSerialize.SerializeData(Slots, _saveID);

        private void Start()
        {
            _inventoryStorage = FindObjectOfType<UI.Storage>();
            _slotsSerialize.DeserializeData(Slots, _saveID);
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

        public void AddItem(Slot slot, out int countRemain)
        {
            countRemain = 0;

            foreach (Slot foreachSlot in Slots)
            {
                if (foreachSlot.Item?.ID == slot.Item.ID)
                {
                    foreachSlot.AddCount(slot.Count, out int remain);

                    if (remain != 0)
                        slot.Count = remain;
                    else
                    {
                        _inventoryStorage.UpdateMenu(Slots);
                        return;
                    }
                }
            }

            countRemain = slot.Count;

            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].Item) continue;

                Slots[i] = slot;
                countRemain = 0;
                break;
            }

            _inventoryStorage.UpdateMenu(Slots);
        }

        public void DeleteItem(int id)
        {
            Slots[id] = new(null, 0);
            _inventoryStorage.UpdateMenu(Slots);
        }
    }
}

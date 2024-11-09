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

        private void Awake()
        {
            _inventoryStorage = FindObjectOfType<UI.Storage>();
            SlotsData data = _slotsSerialize.DeserializeData(_saveID);

            if (data.ItemRecords == null) return;

            for (int i = 0; i < Slots.Length; i++)
            {
                if (data.ItemRecords[i] == null) continue;

                for (int j = 0; j < ItemDictionary.Instance.Items.Length; j++)
                {
                    if (ItemDictionary.Instance.Items[j].ID != data.ItemRecords[i].ID) continue;

                    if (data.ItemRecords[i].GetType() == typeof(WeaponRecord))
                        Slots[i] = new WeaponSlot(ItemDictionary.Instance.Items[j], data.ItemRecords[i].Count, (data.ItemRecords[i] as WeaponRecord).Endurance);
                    else
                        Slots[i] = new(ItemDictionary.Instance.Items[j], data.ItemRecords[i].Count);
                }
            }
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
                        Save();
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
            Save();
        }

        public void DeleteItem(int id)
        {
            Slots[id] = new(null, 0);
            _inventoryStorage.UpdateMenu(Slots);
            Save();
        }

        public void Save()
        {
            SlotsData data = new(Slots);
            _slotsSerialize.SerializeData(data, _saveID);
        }
    }
}

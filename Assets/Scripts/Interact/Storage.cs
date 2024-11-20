using UnityEngine;

namespace Interact
{
    public class Storage : AbstractInteract
    {
        protected virtual void OnDisableStorage() => _serialize.CreateSave($"Storage{_saveID}", new Data.Slots(Slots));

        protected virtual void StartStorage()
        {
            if (_serialize.ExistSave($"Storage{_saveID}"))
                _serialize.Records2Slots(_serialize.LoadSave<Data.Slots>($"Storage{_saveID}").ItemRecords, Slots);
        }

        public void UpdateMenu() => _storageUI.UpdateMenu(Slots);

        public Slot[] Slots;

        [SerializeField] protected int _saveID;

        [SerializeField] private Animator _animator;
        private UI.Storage _storageUI;
        private bool _isOpen = false;

        private readonly Serialize _serialize = new();

        private void OnDisable() => OnDisableStorage();

        private void Start()
        {
            _storageUI = FindObjectOfType<UI.Storage>();
            StartStorage();
        }

        private void OnTriggerExit()
        {
            if (_isOpen)
                _storageUI.SwitchOpen(false);
        }

        public override void Interact()
        {
            _storageUI.StorageStrategy = this;
            _storageUI.SwitchOpen(true);

            if (_isOpen)
                _storageUI.UpdateMenu(Slots);
        }

        public void SetOpen(bool isOpen)
        {
            _isOpen = isOpen;
            _animator.SetBool("IsOpen", _isOpen);
        }

        public void AddItem(ref Slot slot)
        {
            int remain = slot.Count;

            foreach (Slot foreachSlot in Slots)
            {
                if (!foreachSlot.Item || foreachSlot.Item.ID != slot.Item.ID) continue;

                foreachSlot.AddCount(ref remain);
                slot.Count = remain;

                if (remain != 0) continue;

                _storageUI.UpdateMenu(Slots);
                return;
            }

            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].Item) continue;

                if (slot is WeaponSlot weaponSlot)
                    Slots[i] = new WeaponSlot(slot.Item, remain, weaponSlot.Endurance);
                else
                    Slots[i] = new(slot.Item, remain);

                slot.Count = 0;
                _storageUI.UpdateMenu(Slots);
                return;
            }
        }
    }
}

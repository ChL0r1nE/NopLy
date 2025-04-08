using System.Collections.Generic;

namespace Map
{
    public class Town : AbstractConnectableLocation
    {
        private void UpdateTarget(int id) => CaravanTargetsID.Add(id);

        public List<int> RepairTargetsID = new();
        public Slot[] Slots = new Slot[10];

        private Data.Slots _locationState;

        private readonly Serialize _serialize = new();

        private void OnEnable() => Production.UpdateTarget += UpdateTarget;

        private void OnDisable() => _serialize.CreateSave($"Location{_mapID}", new Data.Slots(Slots));

        private void Start()
        {
            _locationState = _serialize.LoadSave<Data.Slots>($"Location{_mapID}");
            _serialize.Records2Slots(_locationState.ItemRecords, Slots);
            UpdateTargetsID();
        }

        public override void UnitInteract(int id, int count)
        {
            Slot slot = new(ItemDictionary.Instance.GetInfo(id), count);
            int remain = slot.Count;

            foreach (Slot foreachSlot in Slots)
            {
                if (!foreachSlot.Item || foreachSlot.Item.ID != slot.Item.ID) continue;

                foreachSlot.AddCount(ref remain);
                slot.Count = remain;

                if (remain != 0) continue;

                _mapLocation.SetCityMenu(Slots);
                return;
            }

            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].Item) continue;

                Slots[i] = (slot is WeaponSlot weaponSlot) ? new WeaponSlot(slot.Item, remain, weaponSlot.Endurance) : new Slot(slot.Item, remain);

                slot.Count = 0;
                _mapLocation.SetCityMenu(Slots);
                return;
            }
        }

        protected override void OnDown()
        {
            base.OnDown();
            _mapLocation.SetCityMenu(Slots);
        }

        protected override void UpdateTargetsID()
        {
            foreach (int id in _serialize.LoadSave<Data.IDArray>("LocationsID").IDs)
                if (id != _mapID && _serialize.LoadSave<object>($"Location{id}") is Data.Production)
                    (_serialize.LoadSave<Data.Production>($"Location{id}").IsWork ? CaravanTargetsID : RepairTargetsID).Add(id);
        }
    }
}

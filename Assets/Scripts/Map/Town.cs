using System.Collections.Generic;

namespace Map
{
    public class Town : AbstractConnectableLocation
    {
        public List<int> RepairTargetsID = new();
        public Slot[] Slots = new Slot[10];

        private Data.Slots _locationState;

        private readonly Serialize _serialize = new();

        protected override void UpdateTargetsID()
        {
            foreach (int id in _serialize.LoadSave<Data.IDArray>("LocationsID").IDs)
                if (id != _mapID && _serialize.LoadSave<object>($"Location{id}") is Data.Production dataProduction)
                    (dataProduction.IsWork ? CaravanTargetsID : RepairTargetsID).Add(id);
        }

        private void OnEnable() => Production.AddWorkTarget += UpdateTarget;

        private void OnDisable()
        {
            Production.AddWorkTarget -= UpdateTarget;
            _serialize.CreateSave($"Location{_mapID}", new Data.Slots(Slots));
        }

        private void Start()
        {
            _locationState = _serialize.LoadSave<Data.Slots>($"Location{_mapID}");
            _serialize.Records2Slots(_locationState.ItemRecords, Slots);
            UpdateTargetsID();
        }

        public override void UnitInteract(ref int id, ref int count)
        {
            foreach (Slot foreachSlot in Slots)
            {
                if (!foreachSlot.Item || foreachSlot.Item.ID != id) continue;

                foreachSlot.AddCount(ref count);

                if (count != 0) continue;

                id = 0;
                _mapLocation.SetTownMenu(Slots);
                return;
            }

            Slot slot = new(ItemDictionary.Instance.GetInfo(id), count);

            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].Item) continue;

                Slots[i] = slot is WeaponSlot weaponSlot ? new WeaponSlot(slot.Item, count, weaponSlot.Endurance) : new Slot(slot.Item, count);

                id = 0;
                count = 0;
                _mapLocation.SetTownMenu(Slots);
                return;
            }
        }

        public override void ShowMenu()
        {
            base.ShowMenu();
            _mapLocation.SetTownMenu(Slots);
        }

        private void UpdateTarget(int id)
        {
            CaravanTargetsID.Add(id);
            RepairTargetsID.Remove(id);
        }
    }
}

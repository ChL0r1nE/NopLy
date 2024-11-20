using System.Collections.Generic;
using System.Linq;

namespace Map
{
    public class Storage : AbstractConnectableLocation
    {
        private List<int> _ids = new();

        public Slot[] Slots = new Slot[10];

        private Data.LocationState _locationState;
        private int _slotCount;

        private readonly Serialize _serialize = new();

        private void OnDisable()
        {
            _serialize.CreateSave($"Location{_mapID}", _locationState with { ItemRecords = _serialize.Slots2Record(Slots), IsWork = true });

            if (_serialize.ExistSave("LocationsID"))
            {
                _ids = _serialize.LoadSave<Data.IDArray>("LocationsID").IDs.ToList();

                if (_ids.IndexOf(_mapID) == -1)
                    _ids.Add(_mapID);
            }
            else
                _ids.Add(_mapID);

            _serialize.CreateSave("LocationsID", new Data.IDArray { IDs = _ids.ToArray() });
        }

        private void Start()
        {
            if (!_serialize.ExistSave($"Location{_mapID}"))
            {
                bool[] b = { false };
                _locationState = new(b, null)
                {
                    IsWork = true
                };

                return;
            }

            _locationState = _serialize.LoadSave<Data.LocationState>($"Location{_mapID}");
            _serialize.Records2Slots(_locationState.ItemRecords, Slots);
            SetTargetsID();
        }

        public override void SetCargo(int id, int count)
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

                if (slot is WeaponSlot weaponSlot)
                    Slots[i] = new WeaponSlot(slot.Item, remain, weaponSlot.Endurance);
                else
                    Slots[i] = new(slot.Item, remain);

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

        protected override void SetTargetsID()
        {
            if (!_serialize.ExistSave("LocationsID")) return;

            int[] ids = _serialize.LoadSave<Data.IDArray>("LocationsID").IDs;

            foreach (int id in ids)
            {
                if (id == _mapID) continue;

                Data.LocationState state = _serialize.LoadSave<Data.LocationState>($"Location{id}");

                if (state.IsClean() && !state.IsWork)
                {
                    TargetsID.Add(id);
                    Slot[] slots = new Slot[state.ItemRecords.Length];

                    for (int j = 0; j < state.ItemRecords.Length; j++)
                        slots[j] = new(ItemDictionary.Instance.GetInfo(state.ItemRecords[j].ID), state.ItemRecords[j].Count);
                }
            }
        }

        public bool DeleteSlots(Slot[] slots)
        {
            bool canDeleteSlot;
            bool isFreeSlot = false;

            foreach (Slot recipeSlot in slots)
            {
                canDeleteSlot = false;
                _slotCount = recipeSlot.Count;

                foreach (Slot slot in Slots)
                {
                    if (slot.Item != recipeSlot.Item) continue;

                    canDeleteSlot = slot.Count - _slotCount >= 0;
                    isFreeSlot |= slot.Count - _slotCount == 0;
                    _slotCount = slot.Count - _slotCount;

                    if (canDeleteSlot) break;
                }

                if (!canDeleteSlot) return false;
            }

            if (!isFreeSlot)
            {
                foreach (Slot slot in Slots)
                    isFreeSlot |= !slot.Item;

                if (!isFreeSlot)
                    return false;
            }

            foreach (Slot recipeSlot in slots)
            {
                _slotCount = recipeSlot.Count;

                foreach (Slot slot in Slots)
                {
                    if (slot.Item != recipeSlot.Item) continue;

                    slot.DeleteCount(ref _slotCount);

                    if (_slotCount == 0) break;
                }
            }

            return true;
        }
    }
}

using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

namespace Map
{
    public class Storage : AbstractConnectableLocation
    {
        private List<int> _ids = new();

        public Slot[] Slots = new Slot[10];

        private BinaryFormatter _formatter = new();
        private FileStream _file;
        private SlotsSerialize _slotsSerialize = new();
        private Data.LocationState _locationState;
        private int _slotCount;

        private void OnDisable()
        {
            _file = File.Create($"{Application.persistentDataPath}/Location{_mapID}.dat");
            _formatter.Serialize(_file, _locationState with { ItemRecords = _slotsSerialize.ItemRecords(Slots), IsWork = true });
            _file.Close();

            if (File.Exists($"{Application.persistentDataPath}/LocationsID.dat"))
            {
                _file = File.Open($"{Application.persistentDataPath}/LocationsID.dat", FileMode.Open);
                _ids = (_formatter.Deserialize(_file) as Data.IDArray).IDs.ToList();
                _file.Close();

                if (_ids.IndexOf(_mapID) == -1)
                    _ids.Add(_mapID);
            }
            else
                _ids.Add(_mapID);

            _file = File.Create($"{Application.persistentDataPath}/LocationsID.dat");
            _formatter.Serialize(_file, new Data.IDArray { IDs = _ids.ToArray() });
            _file.Close();
        }

        private void Start()
        {
            if (!File.Exists($"{Application.persistentDataPath}/Location{_mapID}.dat"))
            {
                bool[] b = { false };
                _locationState = new(b, null)
                {
                    IsWork = true
                };

                return;
            }

            _file = File.Open($"{Application.persistentDataPath}/Location{_mapID}.dat", FileMode.Open);
            _locationState = _formatter.Deserialize(_file) as Data.LocationState;
            _file.Close();

            _slotsSerialize.DeserializeData(Slots, _locationState.ItemRecords);
            SetTargetsID();
        }

        public override void SetCargo(int id, int count)
        {
            Slot slot = new(ItemDictionary.Instance.GetInfo(id), count);
            int remain = slot.Count;
            Debug.Log(slot);

            foreach (Slot foreachSlot in Slots)
            {
                if (!foreachSlot.Item || foreachSlot.Item.ID != slot.Item.ID) continue;

                foreachSlot.AddCount(slot.Count, out remain);
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
            if (!File.Exists($"{Application.persistentDataPath}/LocationsID.dat")) return;

            _file = File.Open($"{Application.persistentDataPath}/LocationsID.dat", FileMode.Open);
            int[] ids = (_formatter.Deserialize(_file) as Data.IDArray).IDs;
            _file.Close();

            foreach (int id in ids)
            {
                if (id == _mapID) continue;

                _file = File.Open($"{Application.persistentDataPath}/Location{id}.dat", FileMode.Open);
                Data.LocationState state = _formatter.Deserialize(_file) as Data.LocationState;
                _file.Close();

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

                    slot.DeleteCount(_slotCount, out _slotCount);

                    if (_slotCount == 0) break;
                }
            }

            return true;
        }
    }
}

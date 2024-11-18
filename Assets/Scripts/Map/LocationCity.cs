using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

namespace Map
{
    public class LocationCity : AbstractConnectableLocation
    {
        public Slot[] Slots = new Slot[10];

        private BinaryFormatter _formatter = new();
        private FileStream _file;
        private int _slotCount;

        private void Start()
        {
            new SlotsSerialize().DeserializeData(Slots, $"Storage{_mapID}");
            SetTargetsID();
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

using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public record Caravan : Slots
    {
        public Caravan(Slot[] slots, int startID, int targetID) : base(slots)
        {
            SlotsSerialize(slots);
            Progress = 0;
            StartID = startID;
            TargetID = targetID;
        }

        public float Progress;
        public int StartID;
        public int TargetID;
    }
}

namespace Interact
{
    public class Caravaner : AbstractInteract
    {
        private List<Slot[]> _caravanWays = new();
        private List<int> _targetIDs = new();

        [SerializeField] private Storage _cityStorage;
        [SerializeField] private int _mapID;
        private BinaryFormatter _formatter = new();
        private FileStream _file;
        private UI.Caravan _caravanUI;
        private bool _isOpen = false;

        private void OnTriggerExit()
        {
            if (_isOpen)
                _caravanUI.Close();
        }

        private void Start()
        {
            _caravanUI = FindObjectOfType<UI.Caravan>();

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
                    _targetIDs.Add(id);
                    Slot[] slots = new Slot[state.ItemRecords.Length];

                    for (int j = 0; j < state.ItemRecords.Length; j++)
                    {
                        foreach (Info.Item item in ItemDictionary.Instance.Items)
                        {
                            if (item.ID != state.ItemRecords[j].ID) continue;

                            slots[j] = new(item, state.ItemRecords[j].Count);
                            break;
                        }
                    }

                    _caravanWays.Add(slots);
                }
            }
        }

        public override void Interact()
        {
            _isOpen = !_isOpen;

            if (_isOpen)
                _caravanUI.SetCaravaner(this, _caravanWays);
            else
                _caravanUI.Close();
        }

        public void AddCaravan(int i)
        {
            int slotCount;
            bool canDeleteSlot;

            foreach (Slot recipeSlot in _caravanWays[i])
            {
                canDeleteSlot = false;
                slotCount = recipeSlot.Count;

                foreach (Slot slot in _cityStorage.Slots)
                {
                    if (slot.Item.ID != recipeSlot.Item.ID) continue;

                    canDeleteSlot = slot.Count - slotCount >= 0;
                    slotCount = slot.Count - slotCount;

                    if (canDeleteSlot) break;
                }

                if (!canDeleteSlot) return;
            }

            foreach (Slot recipeSlot in _caravanWays[i])
            {
                slotCount = recipeSlot.Count;

                foreach (Slot slot in _cityStorage.Slots)
                {
                    if (slot.Item != recipeSlot.Item) continue;

                    slot.DeleteCount(slotCount, out int remain);
                    slotCount = remain;

                    if (remain == 0) break;
                }
            }

            _cityStorage.UpdateMenu();

            List<int> caravansID = new();

            if (File.Exists($"{Application.persistentDataPath}/CaravansID.dat"))
            {
                _file = File.Open($"{Application.persistentDataPath}/CaravansID.dat", FileMode.Open);
                caravansID = (_formatter.Deserialize(_file) as Data.IDArray).IDs.ToList();
                _file.Close();
            }

            int id;

            do
                id = Random.Range(0, 100000);
            while (File.Exists($"{Application.persistentDataPath}/Caravan{id}.dat"));

            caravansID.Add(id);

            _file = File.Create($"{Application.persistentDataPath}/CaravansID.dat");
            _formatter.Serialize(_file, new Data.IDArray { IDs = caravansID.ToArray() });
            _file.Close();

            _file = File.Create($"{Application.persistentDataPath}/Caravan{id}.dat");
            _formatter.Serialize(_file, new Data.Caravan(_caravanWays[i], _mapID, _targetIDs[i]));
            _file.Close();
        }
    }
}

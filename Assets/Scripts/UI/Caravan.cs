using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public record Caravan : Slots
    {
        public Caravan(Slot[] slots, Vector2 targetXZ) : base(slots)
        {
            SlotsSerialize(slots);
            TargetX = targetXZ.x;
            TargetZ = targetXZ.y;
        }

        public float TargetX;
        public float TargetZ;
    }
}

namespace UI
{
    public class Caravan : MonoBehaviour
    {
        public void SetOpen(bool open) => _isOpen = open;

        private List<Slot[]> _repairWays = new();
        private List<int> _targetIDs = new();

        public Slot[] _slots; //pub

        [SerializeField] private RectTransform _iconPrefab;
        [SerializeField] private SlotsSerialize _slotsSerialize;
        private RectTransform _rectTransform;
        private BinaryFormatter _formatter = new();
        private FileStream _file;
        private Vector2 _iconPosition;
        private Vector2 _rectPosition = new(0, -1000);
        private int _mapID;
        private bool _isOpen = false;

        private void Start() => _rectTransform = GetComponent<RectTransform>();

        private void Update()
        {
            _rectPosition.y = Mathf.MoveTowards(_rectPosition.y, _isOpen ? 0 : -1000, Time.deltaTime * 5000);
            _rectTransform.anchoredPosition = _rectPosition;
        }

        public void AddCaravan(int i)
        {
            int slotCount;
            bool canDeleteSlot;

            foreach (Slot recipeSlot in _repairWays[i])
            {
                canDeleteSlot = false;
                slotCount = recipeSlot.Count;

                foreach (Slot slot in _slots)
                {
                    if (slot.Item.ID != recipeSlot.Item.ID) continue;

                    canDeleteSlot = slot.Count - slotCount >= 0;
                    slotCount = slot.Count - slotCount;

                    if (canDeleteSlot) break;
                }

                if (!canDeleteSlot) return;
            }

            foreach (Slot recipeSlot in _repairWays[i])
            {
                slotCount = recipeSlot.Count;

                foreach (Slot slot in _slots)
                {
                    if (slot.Item != recipeSlot.Item) continue;

                    slot.DeleteCount(slotCount, out int remain);
                    slotCount = remain;

                    if (remain == 0) break;
                }
            }

            _slotsSerialize.SerializeData(_slots, $"Storage{_mapID}");

            _file = File.Open($"{Application.persistentDataPath}/LocationState{_targetIDs[i]}.dat", FileMode.Open);
            Data.LocationState state = _formatter.Deserialize(_file) as Data.LocationState;
            _file.Close();

            _file = File.Create($"{Application.persistentDataPath}/LocationState{_targetIDs[i]}.dat");
            _formatter.Serialize(_file, new Data.LocationState(null, state, true));
            _file.Close();
        }

        public void SetCaravanMenu(int mapID)
        {
            if (!File.Exists($"{Application.persistentDataPath}/LocationsList.dat")) return;

            _mapID = mapID;

            _slots = new Slot[10];

            for (int i = 0; i < 10; i++)
                _slots[i] = new(null, 0);

            _slotsSerialize.DeserializeData(_slots, $"Storage{_mapID}");

            _file = File.Open($"{Application.persistentDataPath}/LocationsList.dat", FileMode.Open);
            int[] ids = (_formatter.Deserialize(_file) as Data.LocationsList).IDs;
            _file.Close();

            foreach (int id in ids)
            {
                _iconPosition.x += 80;

                _file = File.Open($"{Application.persistentDataPath}/LocationState{id}.dat", FileMode.Open);
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

                            _iconPosition.y = -160 + j * 80;
                            RectTransform iconRect = Instantiate(_iconPrefab, transform);

                            iconRect.anchoredPosition = _iconPosition;
                            iconRect.transform.GetChild(0).GetComponent<Image>().sprite = item.Sprite;
                            iconRect.transform.GetChild(1).GetComponent<Text>().text = state.ItemRecords[j].Count != 1 ? $"{state.ItemRecords[j].Count}" : "";
                        }
                    }

                    _repairWays.Add(slots);
                }
            }
        }
    }
}

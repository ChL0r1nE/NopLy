using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MercenariesList : MonoBehaviour
    {
        private List<int> _freeCaravansID = new();
        private List<int> _targetsID = new();

        [SerializeField] private Map.CaravanSpawn _caravanSpawn;
        [SerializeField] private Button _caravanButtonPrefab;
        private int _selectCaravanID;
        private int _startID;
        private bool _isStart = false;

        private readonly Serialize _serialize = new();

        private void Start()
        {
            if (!_serialize.ExistSave("CaravansID")) return;

            int[] ids = _serialize.LoadSave<Data.IDArray>("CaravansID").IDs;
            Data.Caravan caravan;
            int i = 0;

            foreach (int id in ids)
            {
                caravan = _serialize.LoadSave<Data.Caravan>($"Caravan{id}");

                if (caravan.TargetID != -1) continue;

                _freeCaravansID.Add(id);
                Instantiate(_caravanButtonPrefab, transform).SetMercenariesList(this, i++);
            }
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Q)) return;

            Map.AbstractLocation.IsPlayerMoveMode = true;
            _isStart = false;
        }

        public void CaravanButtonDown(int id)
        {
            _selectCaravanID = id;
            Map.AbstractLocation.IsPlayerMoveMode = false;

            foreach (int targetID in _targetsID)
                LocationDictionary.Instance.GetTransform(targetID).Translate(Vector3.down);
        }

        public void SetLocationID(Transform transform, int mapID)
        {
            if (!_isStart)
            {
                if (!transform.TryGetComponent(out Map.AbstractConnectableLocation abstractConnectableLocation)) return;

                _startID = mapID;
                _isStart = true;
                _targetsID = abstractConnectableLocation.TargetsID;

                foreach (int targetID in _targetsID)
                    LocationDictionary.Instance.GetTransform(targetID).Translate(Vector3.up);

                return;
            }

            if (_targetsID.IndexOf(mapID) == -1) return;

            Data.LocationState state = _serialize.LoadSave<Data.LocationState>($"Location{mapID}");
            Data.Caravan caravan;

            if (state.IsWork)
                caravan = new(_startID, mapID, _freeCaravansID[_selectCaravanID]);
            else
            {
                Slot[] slots = new Slot[state.ItemRecords.Length];

                for (int i = 0; i < state.ItemRecords.Length; i++)
                    slots[i] = new(ItemDictionary.Instance.GetInfo(state.ItemRecords[i].ID), state.ItemRecords[i].Count);

                if (!LocationDictionary.Instance.GetTransform(_startID).GetComponent<Map.Storage>().DeleteSlots(slots)) return;

                caravan = new(_startID, mapID, _freeCaravansID[_selectCaravanID], true);
                _serialize.CreateSave($"Caravan{_freeCaravansID[_selectCaravanID]}", caravan);

                foreach (int targetID in _targetsID)
                    LocationDictionary.Instance.GetTransform(targetID).Translate(Vector3.down);
            }

            _caravanSpawn.Spawn(caravan);
            Map.AbstractLocation.IsPlayerMoveMode = true;
            _isStart = false;
        }
    }
}

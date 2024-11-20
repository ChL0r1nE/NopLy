using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UI
{
    public class MercenariesList : MonoBehaviour
    {
        private List<int> _caravansID = new();
        private List<int> _targetsID = new();

        [SerializeField] private Map.CaravanSpawn _caravanSpawn;
        [SerializeField] private Button _caravanButtonPrefab;
        private BinaryFormatter _formatter = new();
        private FileStream _file;
        private int _selectCaravanID;
        private int _startID;
        private bool _isStart = false;

        private void Start()
        {
            if (!File.Exists($"{Application.persistentDataPath}/CaravansID.dat")) return;

            _file = File.Open($"{Application.persistentDataPath}/CaravansID.dat", FileMode.Open);
            int[] ids = (_formatter.Deserialize(_file) as Data.IDArray).IDs;
            _file.Close();

            int i = 0;

            foreach (int id in ids)
            {
                _caravansID.Add(id);
                _file = File.Open($"{Application.persistentDataPath}/Caravan{id}.dat", FileMode.Open);
                Data.Caravan caravan = _formatter.Deserialize(_file) as Data.Caravan;
                _file.Close();

                if (caravan.TargetID == -1)
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

            _file = File.Open($"{Application.persistentDataPath}/Location{mapID}.dat", FileMode.Open);
            Data.LocationState state = _formatter.Deserialize(_file) as Data.LocationState;
            _file.Close();

            Data.Caravan caravan;

            if (state.IsWork)
                caravan = new(_startID, mapID, _caravansID[_selectCaravanID]);
            else
            {
                Slot[] slots = new Slot[state.ItemRecords.Length];

                for (int i = 0; i < state.ItemRecords.Length; i++)
                    slots[i] = new(ItemDictionary.Instance.GetInfo(state.ItemRecords[i].ID), state.ItemRecords[i].Count);

                if (!LocationDictionary.Instance.GetTransform(_startID).GetComponent<Map.Storage>().DeleteSlots(slots)) return;

                caravan = new(_startID, mapID, _caravansID[_selectCaravanID], true);

                _file = File.Create($"{Application.persistentDataPath}/Caravan{_caravansID[_selectCaravanID]}.dat");
                _formatter.Serialize(_file, caravan);
                _file.Close();

                foreach (int targetID in _targetsID)
                    LocationDictionary.Instance.GetTransform(targetID).Translate(Vector3.down);
            }

            Debug.Log(caravan.TargetID);
            _caravanSpawn.Spawn(caravan);
            Map.AbstractLocation.IsPlayerMoveMode = true;
            _isStart = false;
        }
    }
}

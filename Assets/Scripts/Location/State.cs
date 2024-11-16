using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public record LocationsList
    {
        public int[] IDs;
    }

    [Serializable]
    public record LocationState : Slots
    {
        public LocationState(bool[] isAlive, Slot[] slots, Vector3 position, int id) : base(slots)
        {
            PositionX = position.x;
            PositionZ = position.z;
            LocationID = id;
            IsAlive = isAlive;

            if (IsClean())
                SlotsSerialize(slots);
        }

        public LocationState(Slot[] slots, LocationState state, bool isWork) : base(slots)
        {
            IsAlive = state.IsAlive;
            PositionX = state.PositionX;
            PositionZ = state.PositionZ;
            LocationID = state.LocationID;
            IsWork = isWork;
        }

        public bool[] IsAlive;

        public float PositionX;
        public float PositionZ;
        public int LocationID;
        public bool IsWork;

        public bool IsClean()
        {
            bool isClean = true;

            foreach (bool flag in IsAlive)
                isClean &= !flag;

            return isClean;
        }
    }
}

namespace Location
{
    public class State : MonoBehaviour, Enemy.IEnemyLeft
    {
        public void EnemyLeft(int number) => _alives[number] = false;

        [Serializable]
        private record EnemyRecord
        {
            public GameObject EnemyPrefab;
            public Vector3 Position;
        }

        private List<int> _ids = new();

        [SerializeField] private Slot[] _repairMaterials;
        [SerializeField] private EnemyRecord[] _enemys;
        private bool[] _alives;

        [SerializeField] private Vector3 _mapPosition;
        [SerializeField] private int _mapID;
        private BinaryFormatter _formatter = new();
        private FileStream _file;
        private bool _isClean;

        private void OnEnable()
        {
            if (File.Exists($"{Application.persistentDataPath}/LocationState{_mapID}.dat"))
            {
                _file = File.Open($"{Application.persistentDataPath}/LocationState{_mapID}.dat", FileMode.Open);
                Data.LocationState state = _formatter.Deserialize(_file) as Data.LocationState;
                _isClean = state.IsClean();
                _alives = state.IsAlive;

                if (_isClean)
                {
                    Destroy(this);
                    return;
                }

                _file.Close();
                return;
            }

            _alives = new bool[_enemys.Length];

            for (int i = 0; i < _alives.Length; i++)
                _alives[i] = true;
        }

        private void Start()
        {
            for (int i = 0; i < _enemys.Length; i++)
                if (_alives[i])
                    Instantiate(_enemys[i].EnemyPrefab, _enemys[i].Position, Quaternion.identity).GetComponentInChildren<Enemy.Health>().SetSpawn(this, i);
        }

        private void OnDisable()
        {
            if (_isClean) return;

            _file = File.Create($"{Application.persistentDataPath}/LocationState{_mapID}.dat");
            _formatter.Serialize(_file, new Data.LocationState(_alives, _repairMaterials, _mapPosition, _mapID));
            _file.Close();

            if (File.Exists($"{Application.persistentDataPath}/LocationsList.dat"))
            {
                _file = File.Open($"{Application.persistentDataPath}/LocationsList.dat", FileMode.Open);
                Data.LocationsList data = (Data.LocationsList)_formatter.Deserialize(_file);
                _file.Close();

                _ids = data.IDs.ToList();

                if (_ids.IndexOf(_mapID) == -1)
                    _ids.Add(_mapID);
            }
            else
                _ids.Add(_mapID);

            _file = File.Create($"{Application.persistentDataPath}/LocationsList.dat");
            _formatter.Serialize(_file, new Data.LocationsList { IDs = _ids.ToArray() });
            _file.Close();
        }
    }
}

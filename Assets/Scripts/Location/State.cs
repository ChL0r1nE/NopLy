using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public record LocationState : Slots
    {
        public LocationState(bool[] isAlive, Slot[] slots) : base(slots)
        {
            IsAlive = isAlive;

            if (IsClean())
                SlotsSerialize(slots);
        }

        public bool[] IsAlive;
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

        [SerializeField] private int _mapID;
        private bool _isClean;

        private readonly Serialize _serialize = new();

        private void OnEnable()
        {
            if (_serialize.ExistSave($"Location{_mapID}"))
            {
                Data.LocationState state = _serialize.LoadSave<Data.LocationState>($"Location{_mapID}");
                _isClean = state.IsClean();
                _alives = state.IsAlive;

                if (_isClean)
                    Destroy(this);

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

            _serialize.CreateSave($"Location{_mapID}", new Data.LocationState(_alives, _repairMaterials));

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
    }
}

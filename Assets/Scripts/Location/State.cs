using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public record Enemy
    {
        public Enemy(bool[] alive)
        {
            Alives = alive;
        }

        public bool[] Alives;
    }
}

namespace Location
{
    public class State : MonoBehaviour, Enemy.IEnemyLeft
    {
        public void EnemyLeft(int number) => _enemyRecord.Alives[number] = false;

        [Serializable]
        private record EnemyRecord
        {
            public GameObject EnemyPrefab;
            public Vector3 Position;
        }

        [SerializeField] private EnemyRecord[] _enemys;

        private Data.Enemy _enemyRecord;
        [SerializeField] private int _mapID;

        private readonly Serialize _serialize = new();

        private void OnEnable()
        {
            if (_serialize.LoadSave<object>($"Location{_mapID}") is Data.Enemy)
                _enemyRecord = _serialize.LoadSave<Data.Enemy>($"Location{_mapID}");
            else
                Destroy(this);
        }

        private void Start()
        {
            for (int i = 0; i < _enemys.Length; i++)
                if (_enemyRecord.Alives[i])
                    Instantiate(_enemys[i].EnemyPrefab, _enemys[i].Position, Quaternion.identity).GetComponentInChildren<Enemy.Health>().SetSpawn(this, i);
        }

        private void OnDisable()
        {
            foreach (bool b in _enemyRecord.Alives)
                if (b)
                {
                    _serialize.CreateSave($"Location{_mapID}", _enemyRecord);
                    return;
                }

            _serialize.CreateSave($"Location{_mapID}", new Data.Production(0, 0, false));
        }
    }
}

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine;

[Serializable]
public record MapEnemyAliveData
{
    public bool[] IsAlive;
}

public class MapEnemySpawn : MonoBehaviour, Enemy.IEnemyLeft
{
    public void EnemyLeft(int number) => _alives[number] = false;

    [Serializable]
    private record EnemyRecord
    {
        public GameObject EnemyPrefab;
        public Vector3 Position;
    }

    [SerializeField] private EnemyRecord[] _enemys;
    private bool[] _alives;

    [SerializeField] private int _mapID;
    private BinaryFormatter _formatter = new();
    private FileStream _file;

    private void OnEnable()
    {
        if (File.Exists($"{Application.persistentDataPath}/Map{_mapID}.dat"))
        {
            _file = File.Open($"{Application.persistentDataPath}/Map{_mapID}.dat", FileMode.Open);
            _alives = (_formatter.Deserialize(_file) as MapEnemyAliveData).IsAlive;
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
                Instantiate(_enemys[i].EnemyPrefab, _enemys[i].Position, Quaternion.identity).AddComponent<Enemy.DisableSignal>().SetSpawn(this, i);
    }

    private void OnDisable()
    {
        _file = File.Create($"{Application.persistentDataPath}/Map{_mapID}.dat");
        _formatter.Serialize(_file, new MapEnemyAliveData { IsAlive = _alives });
        _file.Close();
    }
}

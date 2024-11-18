using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine;

namespace Map
{
    public class LocationEnemy : AbstractLocation
    {
        private class RepairMaterialsClass
        {
            public Slot[] Slots;
        }

        private Lazy<RepairMaterialsClass> _repairMaterials = new();

        [SerializeField] private string _damage;
        private BinaryFormatter _formatter = new();
        private FileStream _file;
        private Data.LocationState _state;
        private bool _isFile;

        private void Start()
        {
            _isFile = File.Exists($"{Application.persistentDataPath}/Location{_mapID}.dat");

            if (!_isFile) return;

            _file = File.Open($"{Application.persistentDataPath}/Location{_mapID}.dat", FileMode.Open);
            _state = _formatter.Deserialize(_file) as Data.LocationState;
            _file.Close();

            if (!_state.IsClean() || _state.IsWork) return;

            _repairMaterials.Value.Slots = new Slot[_state.ItemRecords.Length];

            for (int i = 0; i < _state.ItemRecords.Length; i++)
                _repairMaterials.Value.Slots[i] = new(ItemDictionary.Instance.GetInfo(_state.ItemRecords[i].ID), _state.ItemRecords[i].Count);
        }

        protected override void OnDown()
        {
            base.OnDown();

            if (!_isFile || !_state.IsClean())
                _mapLocation.SetEnemyMenu(_damage);
            else if (!_state.IsWork)
                _mapLocation.SetCityMenu(_repairMaterials.Value.Slots);
            else
                _mapLocation.SetProductMenu("Prod");
        }
    }
}

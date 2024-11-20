using UnityEngine;
using System;

namespace Map
{
    public class Enemy : AbstractLocation
    {
        private class RepairMaterialsClass
        {
            public Slot[] Slots;
        }

        private Lazy<RepairMaterialsClass> _repairMaterials = new();

        [SerializeField] private string _damage;
        private Data.LocationState _state;
        private bool _isFile;

        private readonly Serialize _serialize = new();

        private void OnEnable()
        {
            _isFile = _serialize.ExistSave($"Location{_mapID}");

            if (!_isFile)
            {
                if (TryGetComponent(out Production production))
                    Destroy(production);

                return;
            }

            _state = _serialize.LoadSave<Data.LocationState>($"Location{_mapID}");

            if (_state.IsWork)
            {
                Destroy(this);
                return;
            }
            else if (TryGetComponent(out Production production))
                Destroy(production);

            if (!_state.IsClean()) return;

            _repairMaterials.Value.Slots = new Slot[_state.ItemRecords.Length];

            for (int i = 0; i < _state.ItemRecords.Length; i++)
                _repairMaterials.Value.Slots[i] = new(ItemDictionary.Instance.GetInfo(_state.ItemRecords[i].ID), _state.ItemRecords[i].Count);
        }

        protected override void OnDown()
        {
            base.OnDown();

            if (!_isFile || !_state.IsClean())
                _mapLocation.SetEnemyMenu(_damage);
            else
                _mapLocation.SetCityMenu(_repairMaterials.Value.Slots);
        }
    }
}

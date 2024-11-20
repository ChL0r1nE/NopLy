using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

namespace Interact
{
    public class TownStorage : Storage
    {
        private List<int> _ids = new();

        private BinaryFormatter _formatter = new();
        private FileStream _file;
        private SlotsSerialize _slotsSerialize = new();
        private Data.LocationState _locationState;

        protected override void StartStorage()
        {
            if (!File.Exists($"{Application.persistentDataPath}/Location{_saveID}.dat"))
            {
                bool[] b = { false };
                _locationState = new(b, Slots);
                return;
            }

            _file = File.Open($"{Application.persistentDataPath}/Location{_saveID}.dat", FileMode.Open);
            _locationState = _formatter.Deserialize(_file) as Data.LocationState;
            _file.Close();

            _slotsSerialize.DeserializeData(Slots, _locationState.ItemRecords);
        }

        protected override void OnDisableStorage()
        {
            _file = File.Create($"{Application.persistentDataPath}/Location{_saveID}.dat");
            _formatter.Serialize(_file, _locationState with { ItemRecords = _slotsSerialize.ItemRecords(Slots), IsWork = true });
            _file.Close();

            if (File.Exists($"{Application.persistentDataPath}/LocationsID.dat"))
            {
                _file = File.Open($"{Application.persistentDataPath}/LocationsID.dat", FileMode.Open);
                _ids = (_formatter.Deserialize(_file) as Data.IDArray).IDs.ToList();
                _file.Close();

                if (_ids.IndexOf(_saveID) == -1)
                    _ids.Add(_saveID);
            }
            else
                _ids.Add(_saveID);

            _file = File.Create($"{Application.persistentDataPath}/LocationsID.dat");
            _formatter.Serialize(_file, new Data.IDArray { IDs = _ids.ToArray() });
            _file.Close();
        }
    }
}

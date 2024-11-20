using System.Collections.Generic;
using System.Linq;

namespace Interact
{
    public class TownStorage : Storage
    {
        private List<int> _ids = new();

        private Data.LocationState _locationState;

        private readonly Serialize _serialize = new();

        protected override void StartStorage()
        {
            if (!_serialize.ExistSave($"Location{_saveID}"))
            {
                bool[] b = { false };
                _locationState = new(b, Slots);
                return;
            }

            _locationState = _serialize.LoadSave<Data.LocationState>($"Location{_saveID}");
            _serialize.Records2Slots(_locationState.ItemRecords, Slots);
        }

        protected override void OnDisableStorage()
        {
            _serialize.CreateSave($"Location{_saveID}", _locationState with { ItemRecords = _serialize.Slots2Record(Slots), IsWork = true });

            if (_serialize.ExistSave("LocationsID"))
            {
                _ids = _serialize.LoadSave<Data.IDArray>("LocationsID").IDs.ToList();

                if (_ids.IndexOf(_saveID) == -1)
                    _ids.Add(_saveID);
            }
            else
                _ids.Add(_saveID);

            _serialize.CreateSave("LocationsID", new Data.IDArray { IDs = _ids.ToArray() });
        }
    }
}

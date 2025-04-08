using System.Collections.Generic;
using System.Linq;

namespace Interact
{
    public class TownStorage : Storage
    {
        private List<int> _ids = new();

        private Data.Slots _locationState;

        private readonly Serialize _serialize = new();

        protected override void StartStorage()
        {
            _locationState = _serialize.LoadSave<Data.Slots>($"Location{_saveID}");
            _serialize.Records2Slots(_locationState.ItemRecords, Slots);
        }

        protected override void OnDisableStorage()
        {
            _serialize.CreateSave($"Location{_saveID}", new Data.Slots(Slots));

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

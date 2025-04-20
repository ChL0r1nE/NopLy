using System.Collections.Generic;
using System.Linq;

namespace Map
{
    public class RepairSquad : AbstractUnit
    {
        private Data.RepairSquad _repairSquadData;

        private readonly Serialize _serialize = new();

        private void OnDisable() => _serialize.CreateSave($"RepairSquad{_repairSquadData.ID}", _repairSquadData);

        private void Update() => transform.position = _startPosition + _deltaPosition * _repairSquadData.Progress;

        private void FixedUpdate()
        {
            _progressWay += .01f;
            _repairSquadData.Progress = _progressWay / _lengthWay;

            if (_repairSquadData.Progress < 1f) return;

            LocationDictionary.Instance.GetTransform(_repairSquadData.TargetID).GetComponent<Production>().Repair();

            List<int> IDs = _serialize.LoadSave<Data.IDArray>($"RepairSquadsID").IDs.ToList();
            IDs.Remove(_repairSquadData.ID);

            if (IDs.Count == 0)
                _serialize.DeleteSave("RepairSquadsID");
            else
                _serialize.CreateSave("RepairSquadsID", new Data.IDArray { IDs = IDs.ToArray() });

            _serialize.DeleteSave($"RepairSquad{_repairSquadData.ID}");
            Destroy(gameObject);
        }

        public void SetData(Data.RepairSquad repairSquad)
        {
            _repairSquadData = repairSquad;

            _startPosition = LocationDictionary.Instance.GetTransform(_repairSquadData.StartID).position;
            _deltaPosition = LocationDictionary.Instance.GetTransform(_repairSquadData.TargetID).position;
            SetWay(_repairSquadData.Progress);
        }
    }
}

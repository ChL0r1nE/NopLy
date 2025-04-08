using UnityEngine;

namespace Map
{
    public class RepairSquad : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        private Data.RepairSquad _repairSquadData;
        private Vector3 _startPosition;
        private Vector3 _deltaPosition;
        private float _progressWay;
        private float _lengthWay;

        private readonly Serialize _serialize = new();

        private void OnDisable() => _serialize.CreateSave($"Caravan{_repairSquadData.ID}", _repairSquadData);

        private void Update() => transform.position = _startPosition + _deltaPosition * _repairSquadData.Progress;

        private void FixedUpdate()
        {
            _progressWay += .01f;
            _repairSquadData.Progress = _progressWay / _lengthWay;

            if (_repairSquadData.Progress < 1f) return;

            LocationDictionary.Instance.GetTransform(_repairSquadData.TargetID).GetComponent<Production>().Repair();
            Destroy(gameObject);
        }

        public void SetData(Data.RepairSquad repairSquad)
        {
            _repairSquadData = repairSquad;

            _startPosition = LocationDictionary.Instance.GetTransform(_repairSquadData.StartID).position;
            _lineRenderer.SetPosition(0, _startPosition);

            _deltaPosition = LocationDictionary.Instance.GetTransform(_repairSquadData.TargetID).position;
            _lineRenderer.SetPosition(1, _deltaPosition);
            _deltaPosition -= _startPosition;

            _lengthWay = _deltaPosition.magnitude;
            _progressWay = _lengthWay * _repairSquadData.Progress;
            transform.position = _startPosition + _deltaPosition * _repairSquadData.Progress;
        }
    }
}

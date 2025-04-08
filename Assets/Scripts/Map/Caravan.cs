using UnityEngine;

namespace Map
{
    public class Caravan : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        private Data.Caravan _caravanData;
        private Vector3 _startPosition;
        private Vector3 _deltaPosition;
        private float _progressWay;
        private float _lengthWay;

        private readonly Serialize _serialize = new();

        private void OnDisable() => _serialize.CreateSave($"Caravan{_caravanData.ID}", _caravanData);

        private void Update() => transform.position = _startPosition + _deltaPosition * _caravanData.Progress;

        private void FixedUpdate()
        {
            _progressWay += _caravanData.ToBack ? -.01f : .01f;
            _caravanData.Progress = _progressWay / _lengthWay;

            if (_caravanData.Progress > 0f && _caravanData.Progress < 1f) return;

            _caravanData.ToBack = !_caravanData.ToBack;

            if(_caravanData.MoveType == Data.MoveType.Replace)
            {
                _caravanData.StartID = _caravanData.TargetID;
                _caravanData.TargetID = -1;
                Destroy(gameObject);
                return;
            }

            if (_caravanData.ToBack)
                _caravanData.ItemCount = LocationDictionary.Instance.GetTransform(_caravanData.TargetID).GetComponent<Production>().GetCargo(ref _caravanData.ItemID, 100);
            else
                LocationDictionary.Instance.GetTransform(_caravanData.StartID).GetComponent<AbstractConnectableLocation>().UnitInteract(_caravanData.ItemID, _caravanData.ItemCount);
        }

        public void SetData(Data.Caravan caravan)
        {
            _caravanData = caravan;

            _startPosition = LocationDictionary.Instance.GetTransform(_caravanData.StartID).position;
            _lineRenderer.SetPosition(0, _startPosition);

            _deltaPosition = LocationDictionary.Instance.GetTransform(_caravanData.TargetID).position;
            _lineRenderer.SetPosition(1, _deltaPosition);
            _deltaPosition -= _startPosition;

            if (_caravanData.ToBack)
            {
                _startPosition += _deltaPosition;
                _deltaPosition *= -1;
            }

            _lengthWay = _deltaPosition.magnitude;
            _progressWay = _lengthWay * _caravanData.Progress;
            transform.position = _startPosition + _deltaPosition * _caravanData.Progress;
        }
    }
}

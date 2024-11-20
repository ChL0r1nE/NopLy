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

        private void FixedUpdate()
        {
            _progressWay += .01f;
            _caravanData.Progress = _progressWay / _lengthWay;
            transform.position = _startPosition + _deltaPosition * _caravanData.Progress;

            if (_caravanData.Progress >= 1f)
            {
                if (_caravanData.IsRepair)
                {
                    Data.LocationState state = _serialize.LoadSave<Data.LocationState>($"Location{_caravanData.TargetID}");
                    _serialize.CreateSave($"Location{_caravanData.TargetID}", state with { IsWork = true });

                    _caravanData.TargetID = -1;
                    Destroy(gameObject);
                    return;
                }

                if (_caravanData.IsForward && _caravanData.ItemCount != 0)
                    LocationDictionary.Instance.GetTransform(_caravanData.TargetID).GetComponent<AbstractConnectableLocation>().SetCargo(_caravanData.ItemID, _caravanData.ItemCount);
                else
                    _caravanData.ItemCount = LocationDictionary.Instance.GetTransform(_caravanData.StartID).GetComponent<Production>().GetCargo(100);

                _startPosition += _deltaPosition;
                _deltaPosition *= -1;

                _progressWay = 0;
                _caravanData.Progress = 0f;
                _caravanData.IsForward = !_caravanData.IsForward;
            }
        }

        public void SetData(Data.Caravan caravan)
        {
            _caravanData = caravan;

            if (!caravan.IsRepair && caravan.Progress == 0)
            {
                _caravanData.ItemID = LocationDictionary.Instance.GetTransform(caravan.StartID).GetComponent<Production>().ProductItem.ID;
                _caravanData.ItemCount = LocationDictionary.Instance.GetTransform(caravan.StartID).GetComponent<Production>().GetCargo(100); //100 is capacity
            }

            Vector3 position = LocationDictionary.Instance.GetTransform(_caravanData.StartID).position;
            _lineRenderer.SetPosition(0, position);
            _startPosition = position;

            position = LocationDictionary.Instance.GetTransform(_caravanData.TargetID).position;
            _lineRenderer.SetPosition(1, position);
            _deltaPosition = position;

            _deltaPosition -= _startPosition;

            if (!caravan.IsForward)
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

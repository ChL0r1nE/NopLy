using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

namespace Map
{
    public class Caravan : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        private BinaryFormatter _formatter = new();
        private FileStream _file;
        private Data.Caravan _caravanData;
        private Vector3 _startPosition;
        private Vector3 _deltaPosition;
        private float _progressWay;
        private float _lengthWay;
        private int _caravanID;
        private int _targetID;

        private void OnDisable()
        {
            _file = File.Create($"{Application.persistentDataPath}/Caravan{_caravanID}.dat");
            _formatter.Serialize(_file, _caravanData);
            _file.Close();
        }

        private void FixedUpdate()
        {
            _progressWay += .01f;
            _caravanData.Progress = _progressWay / _lengthWay;
            transform.position = _startPosition + _deltaPosition * _caravanData.Progress;

            if (_caravanData.Progress >= 1f)
            {
                if (_caravanData.IsRepair)
                {
                    _caravanData.TargetID = -1;

                    _file = File.Open($"{Application.persistentDataPath}/Location{_targetID}.dat", FileMode.Open);
                    Data.LocationState state = _formatter.Deserialize(_file) as Data.LocationState;
                    _file.Close();

                    _file = File.Create($"{Application.persistentDataPath}/Location{_targetID}.dat");
                    _formatter.Serialize(_file, state with { IsWork = true });
                    _file.Close();

                    Destroy(gameObject);
                }
            }
        }

        public void SetData(Data.Caravan caravan)
        {
            _caravanID = caravan.ID;
            _caravanData = caravan;
            _targetID = _caravanData.TargetID;

            Vector3 position = LocationDictionary.Instance.GetTransform(_caravanData.StartID).position;
            _lineRenderer.SetPosition(0, position);
            _startPosition = position;

            position = LocationDictionary.Instance.GetTransform(_caravanData.TargetID).position;
            _lineRenderer.SetPosition(1, position);
            _deltaPosition = position;

            _deltaPosition -= _startPosition;
            _lengthWay = _deltaPosition.magnitude;
            _progressWay = _lengthWay * _caravanData.Progress;
            transform.position = _startPosition + _deltaPosition * _caravanData.Progress;
        }
    }
}

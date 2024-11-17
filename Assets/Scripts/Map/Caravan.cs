using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

namespace Map
{
    public class Caravan : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        private BinaryFormatter _formatter = new();
        private FileStream _file;
        private Vector3 _startPosition;
        private Vector3 _deltaPosition;
        private float _lengthWay;
        private float _progressWay;
        private int _caravanID;
        private int _targetID;

        private void FixedUpdate()
        {
            _progressWay += .01f;
            transform.position = _startPosition + _deltaPosition * (_progressWay / _lengthWay);

            if (_progressWay / _lengthWay >= 1f)
            {
                File.Delete($"{Application.persistentDataPath}/Caravan{_caravanID}.dat");

                _file = File.Open($"{Application.persistentDataPath}/CaravansID.dat", FileMode.Open);
                List<int> ids = (_formatter.Deserialize(_file) as Data.IDArray).IDs.ToList();
                _file.Close();

                ids.Remove(_caravanID);

                _file = File.Create($"{Application.persistentDataPath}/CaravansID.dat");
                _formatter.Serialize(_file, new Data.IDArray { IDs = ids.ToArray() });
                _file.Close();

                _file = File.Open($"{Application.persistentDataPath}/Location{_targetID}.dat", FileMode.Open);
                Data.LocationState state = _formatter.Deserialize(_file) as Data.LocationState;
                _file.Close();

                _file = File.Create($"{Application.persistentDataPath}/Location{_targetID}.dat");
                _formatter.Serialize(_file, new Data.LocationState(null, state, true));
                _file.Close();

                Destroy(gameObject);
            }
        }

        public void SetData(int id)
        {
            _file = File.Open($"{Application.persistentDataPath}/Caravan{id}.dat", FileMode.Open);
            Data.Caravan caravan = _formatter.Deserialize(_file) as Data.Caravan;
            _file.Close();

            _caravanID = id;
            _targetID = caravan.TargetID;

            foreach (LocationDictionary.LocationClass location in LocationDictionary.Instance.Locations)
            {
                if (location.ID == caravan.StartID)
                {
                    _lineRenderer.SetPosition(0, location.Transform.position);
                    _startPosition = location.Transform.position;
                }
                else if (location.ID == caravan.TargetID)
                {
                    _lineRenderer.SetPosition(1, location.Transform.position);
                    _deltaPosition = location.Transform.position;
                }
            }

            _deltaPosition -= _startPosition;
            _lengthWay = _deltaPosition.magnitude;
            _progressWay = _lengthWay * caravan.Progress;
            transform.position = _startPosition + _deltaPosition * caravan.Progress;
        }
    }
}

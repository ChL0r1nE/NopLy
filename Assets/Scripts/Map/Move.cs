using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Map
{
    public class Move : MonoBehaviour
    {
        [System.Serializable]
        private record PositionData
        {
            public PositionData(Vector3 position)
            {
                X = position.x;
                Z = position.z;
            }

            public float X;
            public float Z;
        }

        private readonly BinaryFormatter _formatter = new();

        private Vector3 _targetPosition;
        private FileStream _file;
        private string _targetName;
        private bool _isMoving = false;

        private void Awake()
        {
            if (!File.Exists($"{Application.persistentDataPath}/MapPosition.dat")) return;

            _file = File.Open($"{Application.persistentDataPath}/MapPosition.dat", FileMode.Open);
            PositionData positionData = (PositionData)_formatter.Deserialize(_file);
            _file.Close();

            _targetPosition.x = positionData.X;
            _targetPosition.z = positionData.Z;
            transform.position = _targetPosition;
        }

        private void FixedUpdate()
        {
            if (!_isMoving) return;

            if (Vector3.Distance(transform.position, _targetPosition) >= .1f)
                transform.Translate(Vector3.forward * .1f);
            else
            {
                _file = File.Create($"{Application.persistentDataPath}//MapPosition.dat");
                _formatter.Serialize(_file, new PositionData(transform.position));
                _file.Close();

                SceneManager.LoadScene(_targetName);
            }
        }

        public void SetTargetPosition(Vector3 target, string locationName)
        {
            _isMoving = true;
            _targetPosition = target;
            _targetName = locationName;
            transform.LookAt(_targetPosition);
        }
    }
}

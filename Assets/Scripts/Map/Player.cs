using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public record PositionOnMap
    {
        public PositionOnMap(Vector3 position)
        {
            X = position.x;
            Z = position.z;
        }

        public float X;
        public float Z;
    }
}

namespace Map
{
    public class Player : MonoBehaviour
    {
        public void Exit() => _isInside = false;

        private readonly BinaryFormatter _formatter = new();

        [SerializeField] private UI.MapLocation _locationMenu;
        private Vector3 _targetPosition;
        private FileStream _file;
        private string _targetName;
        private bool _isInside = false;
        private bool _isMoving = false;

        private void Awake()
        {
            if (!File.Exists($"{Application.persistentDataPath}/MapPosition.dat")) return;

            _file = File.Open($"{Application.persistentDataPath}/MapPosition.dat", FileMode.Open);
            Data.PositionOnMap positionData = (Data.PositionOnMap)_formatter.Deserialize(_file);
            _file.Close();

            transform.position = new(positionData.X, 0, positionData.Z);
        }

        private void FixedUpdate()
        {
            if (_isInside) return;

            if (_isMoving && Vector3.Distance(transform.position, _targetPosition) > .1f)
                transform.Translate(Vector3.forward * .1f);
            else if (_isMoving)
            {
                _isInside = true;
                _isMoving = false;
                _locationMenu.Open();
            }
        }

        public void SetTargetPosition(Vector3 position, Sprite sprite, string locationName)
        {
            if (_isInside) return;

            _isMoving = true;
            _targetName = locationName;
            _targetPosition = position;
            transform.LookAt(_targetPosition);
            _locationMenu.SetLocationImage(sprite, _targetName);
        }

        public void LoadLocation()
        {
            _file = File.Create($"{Application.persistentDataPath}/MapPosition.dat");
            _formatter.Serialize(_file, new Data.PositionOnMap(transform.position));
            _file.Close();

            SceneManager.LoadScene(_targetName);
        }
    }
}

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

        [SerializeField] private UI.MapLocation _locationMenu;
        private Vector3 _targetPosition;
        private string _targetName;
        private bool _isInside = false;
        private bool _isMoving = false;

        private readonly Serialize _serialize = new();

        private void Awake()
        {
            if (!_serialize.ExistSave("MapPosition")) return;

            Data.PositionOnMap positionData = _serialize.LoadSave<Data.PositionOnMap>("MapPosition");
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
            _serialize.CreateSave("MapPosition", new Data.PositionOnMap(transform.position));
            SceneManager.LoadScene(_targetName);
        }
    }
}

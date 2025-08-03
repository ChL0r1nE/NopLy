using UnityEngine.SceneManagement;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public record Position
    {
        public Position(Vector3 position)
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
        public static Player Static;
        public static bool IsChoiced = true;

        public void Exit() => _isInside = false;

        private AbstractLocation _abstractLocation;
        private Vector3 _targetPosition;
        private string _targetName;
        private bool _isInside = false;
        private bool _isMove = false;

        private readonly Serialize _serialize = new();

        private void Awake()
        {
            Static = this;

            if (!_serialize.ExistSave("MapPosition")) return; //MakeBaseSafe

            Data.Position positionData = _serialize.LoadSave<Data.Position>("MapPosition");
            transform.position = new(positionData.X, 0, positionData.Z);
        }

        private void FixedUpdate()
        {
            if (_isInside) return;

            if (_isMove)
            {
                if (Vector3.Distance(transform.position, _targetPosition) > .1f)
                    transform.Translate(Vector3.forward * .1f);
                else
                {
                    _isMove = false;
                    _isInside = true;
                    _abstractLocation.ShowMenu();
                }
            }
        }

        public void SetTargetPosition(AbstractLocation abstractLocation, Vector3 position, string name)
        {
            if (_isInside) return;

            _isMove = true;
            _targetName = name;
            _targetPosition = position;
            _abstractLocation = abstractLocation;
            transform.LookAt(_targetPosition);
        }

        public void LoadLocation()
        {
            _serialize.CreateSave("MapPosition", new Data.Position(transform.position));
            SceneManager.LoadScene(_targetName);
        }
    }
}

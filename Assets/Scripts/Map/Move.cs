using UnityEngine.SceneManagement;
using UnityEngine;

namespace Map
{
    public class Move : MonoBehaviour //X_X
    {
        public static Move StaticMove;

        private Vector3 _targetPosition;
        private string _targetName;
        private bool _isMoving = false;

        private void Start() => StaticMove = this;

        private void FixedUpdate()
        {
            if (!_isMoving) return;

            transform.LookAt(_targetPosition);

            if (Vector3.Distance(transform.position, _targetPosition) >= .1f)
                transform.Translate(Vector3.forward * .1f);
            else
                SceneManager.LoadScene(_targetName);
        }

        public void SetTargetPosition(Vector3 target, string locationName)
        {
            _isMoving = true;
            _targetPosition = target;
            _targetName = locationName;
        }
    }
}

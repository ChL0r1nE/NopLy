using UnityEngine;

namespace Map
{
    public class AbstractUnit : MonoBehaviour
    {
        [SerializeField] protected LineRenderer _lineRenderer;
        protected Vector3 _startPosition;
        protected Vector3 _deltaPosition;
        protected float _progressWay;
        protected float _lengthWay;

        protected void SetWay(float progress)
        {
            _lineRenderer.SetPosition(0, _startPosition);
            _lineRenderer.SetPosition(1, _deltaPosition);

            _deltaPosition -= _startPosition;
            _lengthWay = _deltaPosition.magnitude;
            _progressWay = _lengthWay * progress;
            transform.position = _startPosition + _deltaPosition * progress;
        }
    }
}

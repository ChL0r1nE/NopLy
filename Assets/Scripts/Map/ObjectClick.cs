using UnityEngine;

namespace Map
{
    public class ObjectClick : MonoBehaviour
    {
        [SerializeField] private Move _move;
        [SerializeField] private Vector3 _position;
        [SerializeField] private string _locationName;

        private void OnMouseDown() => _move.SetTargetPosition(_position, _locationName);
    }
}

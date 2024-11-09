using UnityEngine;

namespace Map
{
    public class ObjectClick : MonoBehaviour
    {
        [SerializeField] private Vector3 _position;
        [SerializeField] private string _locationName;

        private void OnMouseDown() => Move.StaticMove.SetTargetPosition(_position, _locationName);
    }
}

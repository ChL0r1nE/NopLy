using UnityEngine;

namespace Map
{
    public class ObjectClick : MonoBehaviour
    {
        [SerializeField] private Move _move;
        [SerializeField] private string _locationName;

        private void OnMouseDown() => _move.SetTargetPosition(transform.position, _locationName);
    }
}

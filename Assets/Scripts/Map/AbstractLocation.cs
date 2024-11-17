using UnityEngine;

namespace Map
{
    public abstract class AbstractLocation : MonoBehaviour
    {
        [SerializeField] protected UI.MapLocation _mapLocation;
        [SerializeField] protected Player _move;
        [SerializeField] protected int _mapID;

        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _name;

        private void OnMouseDown() => OnDown();

        protected virtual void OnDown() => _move.SetTargetPosition(transform.position, _sprite, _name);
    }
}

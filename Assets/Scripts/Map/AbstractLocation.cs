using UnityEngine;

namespace Map
{
    public abstract class AbstractLocation : MonoBehaviour
    {
        public static bool IsPlayerMoveMode = true;

        [SerializeField] protected UI.MapLocation _mapLocation;
        [SerializeField] protected UI.MercenariesList _mercenariesList;
        [SerializeField] protected Player _move;
        [SerializeField] protected int _mapID;

        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _name;

        private void OnMouseDown()
        {
            if (IsPlayerMoveMode)
                OnDown();
            else
                _mercenariesList.SetLocationID(transform, _mapID);
        }

        protected virtual void OnDown() => _move.SetTargetPosition(transform.position, _sprite, _name);
    }
}

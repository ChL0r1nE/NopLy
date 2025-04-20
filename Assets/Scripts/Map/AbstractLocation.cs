using UnityEngine;

namespace Map
{
    public abstract class AbstractLocation : MonoBehaviour
    {
        [SerializeField] protected UI.MapLocation _mapLocation;
        [SerializeField] protected int _mapID;

        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _name;

        private void OnMouseDown()
        {
            if (Player.IsChoiced)
                Player.Static.SetTargetPosition(this, transform.position, _name);
            else
                UI.MercenariesList.Static.SetLocationID(_mapID);
        }

        public virtual void ShowMenu()
        {
            _mapLocation.Open();
            _mapLocation.SetLocationHead(_sprite, _name);
        }
    }
}

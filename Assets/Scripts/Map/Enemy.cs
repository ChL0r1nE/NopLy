using UnityEngine;

namespace Map
{
    public class Enemy : AbstractLocation
    {
        [SerializeField] private string _damage;

        private readonly Serialize _serialize = new();

        private void OnEnable()
        {
            if (_serialize.LoadSave<object>($"Location{_mapID}") is Data.Enemy) return;

            gameObject.GetComponent<Production>().SetMain();
            Destroy(this);
        }

        public override void ShowMenu()
        {
            base.ShowMenu();
            _mapLocation.SetEnemyMenu(_damage);
        }
    }
}

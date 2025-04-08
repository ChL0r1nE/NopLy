using UnityEngine;

namespace Map
{
    public class Enemy : AbstractLocation
    {
        [SerializeField] private string _damage;

        private readonly Serialize _serialize = new();

        private void OnEnable()
        {
            if (_serialize.LoadSave<object>($"Location{_mapID}") is Data.Production)
                Destroy(this);
            else if (gameObject.TryGetComponent(out Production production))
                Destroy(production);
        }

        protected override void OnDown()
        {
            base.OnDown();
            _mapLocation.SetEnemyMenu(_damage);
        }
    }
}

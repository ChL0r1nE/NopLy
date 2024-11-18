using UnityEngine;

namespace Map
{
	public class LocationVillage : AbstractConnectableLocation
	{
        [SerializeField] private string _product;

        private void Start() => SetTargetsID();

        protected override void OnDown()
        {
            base.OnDown();
            _mapLocation.SetVillageMenu(_product);
        }

        protected override void SetTargetsID()
        {
            Debug.Log("Sets");
        }
    }
}

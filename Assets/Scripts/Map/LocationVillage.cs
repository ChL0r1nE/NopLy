using UnityEngine;

namespace Map
{
	public class LocationVillage : AbstractLocation
	{
        [SerializeField] private string _product;

        protected override void OnDown()
        {
            base.OnDown();
            _mapLocation.SetVillageMenu(_product);
        }
    }
}

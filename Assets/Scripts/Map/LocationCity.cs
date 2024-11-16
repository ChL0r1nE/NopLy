using UnityEngine;

namespace Map
{
    public class LocationCity : AbstractLocation
    {
        public Slot[] Slots = new Slot[10];

        [SerializeField] private SlotsSerialize _slotsSerialize;
        [SerializeField] private int _storageID;

        private void Start() => _slotsSerialize.DeserializeData(Slots, $"Storage{_storageID}");

        protected override void OnDown()
        {
            base.OnDown();
            _mapLocation.SetCityMenu(Slots);
        }
    }
}

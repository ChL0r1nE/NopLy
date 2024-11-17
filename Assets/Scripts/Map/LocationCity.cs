namespace Map
{
    public class LocationCity : AbstractLocation
    {
        public Slot[] Slots = new Slot[10];

        private void Start() => new SlotsSerialize().DeserializeData(Slots, $"Storage{_mapID}");

        protected override void OnDown()
        {
            base.OnDown();
            _mapLocation.SetCityMenu(Slots);
        }
    }
}

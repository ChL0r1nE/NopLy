namespace Map
{
    public class Production : AbstractConnectableLocation
    {
        public override void SetCargo(int id, int count) => ItemCount += count;

        public Info.Item Item;
        public int ItemCount;
        public Info.Item ProductItem;
        public int ProductItemCount;

        private int _productCount;

        private readonly Serialize _serialize = new();

        private void OnEnable() => TickMachine.OnTick += OnTick;

        private void OnDisable() => TickMachine.OnTick -= OnTick;

        private void Start() => SetTargetsID();

        protected override void OnDown()
        {
            base.OnDown();
            Slot[] slots = { new Slot(ProductItem, ProductItemCount) };
            _mapLocation.SetCityMenu(slots);
        }

        protected override void SetTargetsID()
        {
            if (!_serialize.ExistSave("LocationsID")) return;

            int[] ids = _serialize.LoadSave<Data.IDArray>("LocationsID").IDs;

            foreach (int id in ids)
            {
                if (LocationDictionary.Instance.GetTransform(id).TryGetComponent(out Production production) && (production.Item.ID == ProductItem.ID))
                    TargetsID.Add(id);
                else if (LocationDictionary.Instance.GetTransform(id).TryGetComponent(out Storage _))
                    TargetsID.Add(id);
            }
        }

        public int GetCargo(int count)
        {
            int startCount = ProductItemCount;

            ProductItemCount -= count;

            if (ProductItemCount < 0)
                ProductItemCount = 0;

            return startCount - ProductItemCount;
        }

        private void OnTick()
        {
            if (Item == null)
                ProductItemCount += 10;
            else
            {
                _productCount = ItemCount > 10 ? 10 : ItemCount;
                ItemCount -= _productCount;
                ProductItemCount += _productCount / 2;
            }
        }
    }
}

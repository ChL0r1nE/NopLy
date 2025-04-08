using System;

namespace Data
{
    [Serializable]
    public record Production
    {
        public Production(int baseProductCount, int resultProductCount, bool isWork)
        {
            BaseProductCount = baseProductCount;
            ResultProductCount = resultProductCount;
            IsWork = isWork;
        }

        public bool IsWork;
        public int BaseProductCount;
        public int ResultProductCount;
    }
}

namespace Map
{
    public class Production : AbstractConnectableLocation
    {
        public static Action<int> UpdateTarget;

        public override void UnitInteract(int id, int count) => _production.BaseProductCount += count;

        public void Repair()
        {
            _production.IsWork = true;
            UpdateTarget?.Invoke(_mapID);
        }

        public Info.Item BaseItem;
        public Info.Item ProductItem;

        private Data.Production _production;

        private int _productCount;

        private readonly Serialize _serialize = new();

        private void OnEnable() => TickMachine.OnTick += OnTick;

        private void OnDisable()
        {
            TickMachine.OnTick -= OnTick;
            //createsave
        }

        private void Start()
        {
            if (_serialize.LoadSave<object>($"Location{_mapID}") is Data.Production)
            {
                UpdateTargetsID();
                _production = _serialize.LoadSave<Data.Production>($"Location{_mapID}");
            }
            else
                Destroy(this);
        }

        protected override void OnDown()
        {
            base.OnDown();
            Slot[] slots = { new Slot(ProductItem, _production.ResultProductCount) };
            _mapLocation.SetCityMenu(slots);
        }

        protected override void UpdateTargetsID()
        {
            int[] ids = _serialize.LoadSave<Data.IDArray>("LocationsID").IDs;

            foreach (int id in ids) //rework
            {
                if (LocationDictionary.Instance.GetTransform(id).TryGetComponent(out Production production) && production.BaseItem && production.BaseItem.ID == ProductItem.ID)
                    CaravanTargetsID.Add(id);
                else if (LocationDictionary.Instance.GetTransform(id).TryGetComponent(out Town _))
                    CaravanTargetsID.Add(id);
            }
        }

        public int GetCargo(ref int itemID, int count)
        {
            itemID = ProductItem.ID;
            int startCount = _production.ResultProductCount;

            _production.ResultProductCount -= count;

            if (_production.ResultProductCount < 0)
                _production.ResultProductCount = 0;

            return startCount - _production.ResultProductCount;
        }

        private void OnTick()
        {
            if (!_production.IsWork) return;

            if (BaseItem == null)
                _production.ResultProductCount += 10;
            else
            {
                _productCount = _production.BaseProductCount > 10 ? 10 : _production.BaseProductCount;
                _production.BaseProductCount -= _productCount;
                _production.ResultProductCount += _productCount / 2;
            }
        }
    }
}

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
        public static Action<int> AddWorkTarget;

        public override void UnitInteract(ref int id, ref int count)
        {
            if (id == 0)
            {
                int productCount = Math.Min(16, _production.ResultProductCount);
                _production.ResultProductCount -= productCount;

                id = ResultItem.ID;
                count = productCount;
                return;
            }

            _production.BaseProductCount += count;
            count = 0;
            id = 0;
        }

        public void Repair()
        {
            _production.IsWork = true;
            AddWorkTarget?.Invoke(_mapID);
        }

        public Info.Item BaseItem;
        public Info.Item ResultItem;

        [UnityEngine.SerializeField] private bool _isMain;
        private Data.Production _production;
        private int _productCount;

        private readonly Serialize _serialize = new();

        public bool IsWork() => _production.IsWork;

        public void SetMain() => _isMain = true;

        private void OnEnable()
        {
            if (!(_serialize.LoadSave<object>($"Location{_mapID}") is Data.Production))
            {
                UnityEngine.Debug.Log("Delete");
                Destroy(this);
                return;
            }

            TickMachine.OnTick += OnTick;
            AddWorkTarget += UpdateWorkTarget;
            _production = _serialize.LoadSave<Data.Production>($"Location{_mapID}");
        }

        private void OnDisable()
        {
            TickMachine.OnTick -= OnTick;
            AddWorkTarget -= UpdateWorkTarget;

            if (_isMain)
                _serialize.CreateSave($"Location{_mapID}", _production);
        }

        private void Start() => UpdateTargetsID();

        public override void ShowMenu()
        {
            base.ShowMenu();

            if (BaseItem)
                _mapLocation.SetProductionMenu(BaseItem.Sprite, _production.BaseProductCount, ResultItem.Sprite, _production.ResultProductCount);
            else
                _mapLocation.SetProductionMenu(ResultItem.Sprite, _production.ResultProductCount);

        }

        protected override void UpdateTargetsID()
        {
            int[] ids = _serialize.LoadSave<Data.IDArray>("LocationsID").IDs;

            foreach (int id in ids)
            {
                UnityEngine.Debug.Log($"Check {id}");
                if (LocationDictionary.Instance.GetTransform(id).TryGetComponent(out Production production)) //see Deleted on Mill
                    if (production.BaseItem && production.BaseItem.ID == ResultItem.ID)
                        CaravanTargetsID.Add(id);

                if (LocationDictionary.Instance.GetTransform(id).TryGetComponent(out Town _))
                    CaravanTargetsID.Add(id);
            }
        }

        private void UpdateWorkTarget(int id)
        {
            if (LocationDictionary.Instance.GetTransform(id).TryGetComponent(out Production production) && production.BaseItem && production.BaseItem.ID == ResultItem.ID)
                CaravanTargetsID.Add(id);
        }

        private void OnTick()
        {
            if (!_production.IsWork) return;

            if (BaseItem == null)
                _production.ResultProductCount += 4;
            else
            {
                _productCount = Math.Min(10, _production.BaseProductCount);
                _production.BaseProductCount -= _productCount;
                _production.ResultProductCount += _productCount / 2;
            }
        }
    }
}

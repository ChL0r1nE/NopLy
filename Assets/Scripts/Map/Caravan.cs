using UI;

namespace Data
{
    public enum MoveType
    {
        Main,
        Replace
    }

    [System.Serializable]
    public record RepairSquad
    {
        public RepairSquad(int id, int startID, int targetID = -1)
        {
            ID = id;
            StartID = startID;
            TargetID = targetID;
        }

        public int ID;
        public float Progress;
        public int TargetID;
        public int StartID;
    }

    [System.Serializable]
    public record Caravan : RepairSquad
    {
        public Caravan(int id, int startID, int targetID = -1, MoveType moveType = MoveType.Main) : base(id, startID, targetID)
        {
            ID = id;
            Progress = 0;
            TargetID = targetID;
            StartID = startID;
            ToBack = false;
            MoveType = moveType;
        }

        public MoveType MoveType;
        public int ItemID;
        public int ItemCount;
        public bool ToBack;
    }
}

namespace Map
{
    public class Caravan : AbstractUnit
    {
        private Data.Caravan _caravanData;

        private readonly Serialize _serialize = new();

        private void OnDisable() => _serialize.CreateSave($"Caravan{_caravanData.ID}", _caravanData);

        private void Update() => transform.position = _startPosition + _deltaPosition * _caravanData.Progress;

        private void FixedUpdate()
        {
            _progressWay += _caravanData.ToBack ? -.01f : .01f;
            _caravanData.Progress = _progressWay / _lengthWay;

            if (_caravanData.Progress > 0f && _caravanData.Progress < 1f) return;

            if (_caravanData.MoveType == Data.MoveType.Replace)
            {
                MercenariesList.Static.AddPanel(_caravanData.ID, _caravanData.StartID, UnitType.Caravan);
                _caravanData.StartID = _caravanData.TargetID;
                _caravanData.TargetID = -1;
                Destroy(gameObject);
                return;
            }

            _caravanData.ToBack = !_caravanData.ToBack;
            LocationDictionary.Instance.GetTransform(_caravanData.ToBack ? _caravanData.TargetID : _caravanData.StartID).GetComponent<AbstractConnectableLocation>().UnitInteract(ref _caravanData.ItemID, ref _caravanData.ItemCount);
        }

        public void SetData(Data.Caravan caravan)
        {
            _caravanData = caravan;

            if(_caravanData.Progress == 0f && !_caravanData.ToBack)
                LocationDictionary.Instance.GetTransform(_caravanData.StartID).GetComponent<AbstractConnectableLocation>().UnitInteract(ref _caravanData.ItemID, ref _caravanData.ItemCount);

            _startPosition = LocationDictionary.Instance.GetTransform(_caravanData.StartID).position;
            _deltaPosition = LocationDictionary.Instance.GetTransform(_caravanData.TargetID).position;
            SetWay(_caravanData.Progress);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public enum UnitType
    {
        Caravan,
        RepairSquad
    }

    public struct UnitInfo
    {
        public UnitInfo(int id, int startID, UnitType type, RectTransform transform)
        {
            ID = id;
            StartID = startID;
            Type = type;
            RectTransform = transform;
        }

        public RectTransform RectTransform;
        public UnitType Type;
        public int StartID;
        public int ID;
    }

    public class MercenariesList : MonoBehaviour
    {
        private List<UnitInfo> _unitsInfo = new();
        private List<int> _targetsID = new();

        [SerializeField] private RectTransform _choisePanelTransform;
        [SerializeField] private Map.CaravanSpawn _caravanSpawn;
        [SerializeField] private Button _unitButtonPrefab;
        private Vector2 _buttonOffset = new(0, 105);
        private Data.MoveType _moveType;
        private int _selectUnitNumber;
        private bool _isChoiceType = false;

        private readonly Serialize _serialize = new();

        private void Start()
        {
            if (!_serialize.ExistSave("CaravansID")) return;

            int[] ids = _serialize.LoadSave<Data.IDArray>("CaravansID").IDs;
            int i = 0;

            foreach (int id in ids)
            {
                Data.Caravan caravan = _serialize.LoadSave<Data.Caravan>($"Caravan{id}");

                if (caravan.TargetID != -1) continue;

                Button button = Instantiate(_unitButtonPrefab, transform);
                button.SetButtonInfo(this, caravan.ID, caravan.StartID, UnitType.Caravan);
                _unitsInfo.Add(new(id, caravan.StartID, UnitType.Caravan, button.GetComponent<RectTransform>()));
                _unitsInfo[^1].RectTransform.anchoredPosition = _buttonOffset * i++;
            }

            if (!_serialize.ExistSave("RepairSquadsID")) return;

            ids = _serialize.LoadSave<Data.IDArray>("RepairSquadsID").IDs;

            foreach (int id in ids)
            {
                Data.RepairSquad repairSquad = _serialize.LoadSave<Data.RepairSquad>($"RepairSquad{id}");

                if (repairSquad.TargetID != -1) continue;

                Button button = Instantiate(_unitButtonPrefab, transform);
                button.SetButtonInfo(this, repairSquad.ID, repairSquad.StartID, UnitType.RepairSquad);
                _unitsInfo.Add(new(id, repairSquad.StartID, UnitType.RepairSquad, button.GetComponent<RectTransform>()));
                _unitsInfo[^1].RectTransform.anchoredPosition = _buttonOffset * i++;
            }
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Q)) return;

            _isChoiceType = false;
            Map.AbstractLocation.IsPlayerMoveMode = true;
        }

        public void SetMoveType(bool isMain)
        {
            _isChoiceType = true;
            _moveType = isMain ? Data.MoveType.Main : Data.MoveType.Replace;
        }

        public void CaravanButtonDown(int id)
        {
            _isChoiceType = false;

            for (int i = 0; i < _unitsInfo.Count; i++)
                if (_unitsInfo[i].ID == id)
                    _selectUnitNumber = i;

            Map.AbstractLocation.IsPlayerMoveMode = false;
            _choisePanelTransform.anchoredPosition = new Vector2(275, 105 * _selectUnitNumber); //new vec

            Transform transform = LocationDictionary.Instance.GetTransform(_unitsInfo[_selectUnitNumber].StartID);

            if (_unitsInfo[_selectUnitNumber].Type == UnitType.Caravan)
                _targetsID = transform.GetComponent<Map.AbstractConnectableLocation>().CaravanTargetsID;
            else if (_unitsInfo[_selectUnitNumber].Type == UnitType.RepairSquad)
                _targetsID = transform.GetComponent<Map.Town>().RepairTargetsID;

            foreach (int targetID in _targetsID)
                LocationDictionary.Instance.GetTransform(targetID).Translate(Vector3.up);
        }

        public void SetLocationID(int mapID)
        {
            if (!_isChoiceType || _targetsID.IndexOf(mapID) == -1) return;

            if (_unitsInfo[_selectUnitNumber].Type == UnitType.Caravan)
                _caravanSpawn.SpawnCaravan(new(_unitsInfo[_selectUnitNumber].ID, _unitsInfo[_selectUnitNumber].StartID, mapID, _moveType));
            if (_unitsInfo[_selectUnitNumber].Type == UnitType.RepairSquad)
                _caravanSpawn.SpawnRepairSquad(new(_unitsInfo[_selectUnitNumber].ID, _unitsInfo[_selectUnitNumber].StartID, mapID));

            Map.AbstractLocation.IsPlayerMoveMode = true;
            Destroy(_unitsInfo[_selectUnitNumber].RectTransform.gameObject);
            _unitsInfo.RemoveAt(_selectUnitNumber);

            for (int i = _selectUnitNumber; i < _unitsInfo.Count; i++)
                _unitsInfo[i].RectTransform.anchoredPosition -= _buttonOffset;
        }
    }
}

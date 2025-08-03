using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Interact
{
    public class Caravaner : AbstractInteract
    {
        public int LocationID;

        private UI.Caravan _caravanUI;
        private bool _isOpen = false;

        private readonly Serialize _serialize = new();

        private void OnTriggerExit()
        {
            _isOpen = false;
            _caravanUI.SetOpen(false);
        }

        private void Start()
        {
            _caravanUI = FindObjectOfType<UI.Caravan>();
            _caravanUI.SetCaravaner(this);
        }

        public override void Interact()
        {
            _isOpen = !_isOpen;
            _caravanUI.SetOpen(_isOpen);
        }

        public void AddUnit(UI.UnitType unitType)
        {
            string unitName = unitType == UI.UnitType.Caravan ? "Caravan" : "RepairSquad";
            List<int> IDs = new();

            if (_serialize.ExistSave($"{unitName}sID"))
                IDs = _serialize.LoadSave<Data.IDArray>($"{unitName}sID").IDs.ToList();

            int id;

            do
                id = Random.Range(0, 100000);
            while (_serialize.ExistSave($"{unitName}{id}"));

            IDs.Add(id);

            _serialize.CreateSave($"{unitName}sID", new Data.IDArray { IDs = IDs.ToArray() });
            _serialize.CreateSave($"{unitName}{id}", unitType == UI.UnitType.Caravan ? new Data.Caravan(id, LocationID) : new Data.RepairSquad(id, LocationID));
        }
    }
}

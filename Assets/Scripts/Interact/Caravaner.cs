using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public record Caravan
    {
        public Caravan(int startID, int targetID, int id, bool isRepair = false)
        {
            Progress = 0;
            TargetID = targetID;
            StartID = startID;
            ID = id;
            IsRepair = isRepair;
            IsForward = true;
        }

        public float Progress;
        public int ItemCount;
        public int TargetID;
        public int StartID;
        public int ItemID;
        public int ID;
        public bool IsForward;
        public bool IsRepair;
    }
}

namespace Interact
{
    public class Caravaner : AbstractInteract
    {
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

        public void AddCaravan()
        {
            List<int> caravansID = new();

            if (_serialize.ExistSave("CaravansID"))
                caravansID = _serialize.LoadSave<Data.IDArray>("CaravansID").IDs.ToList();

            int id;

            do
                id = Random.Range(0, 100000);
            while (_serialize.ExistSave($"Caravan{id}"));

            caravansID.Add(id);

            _serialize.CreateSave("CaravansID", new Data.IDArray { IDs = caravansID.ToArray() });
            _serialize.CreateSave($"Caravan{id}", new Data.Caravan(-1, -1, id));
        }
    }
}

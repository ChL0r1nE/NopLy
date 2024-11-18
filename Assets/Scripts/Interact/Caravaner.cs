using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public record Caravan : Slots
    {
        public Caravan(Slot[] slots, int startID, int targetID, int id, bool isRepair = false) : base(slots)
        {
            SlotsSerialize(slots);
            Progress = 0;
            StartID = startID;
            ID = id;
            TargetID = targetID;
            IsRepair = isRepair;
        }

        public float Progress;
        public int ID;
        public int StartID;
        public int TargetID;
        public bool IsRepair;
    }
}

namespace Interact
{
    public class Caravaner : AbstractInteract
    {
        private BinaryFormatter _formatter = new();
        private FileStream _file;
        private UI.Caravan _caravanUI;
        private bool _isOpen = false;

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

            if (File.Exists($"{Application.persistentDataPath}/CaravansID.dat"))
            {
                _file = File.Open($"{Application.persistentDataPath}/CaravansID.dat", FileMode.Open);
                caravansID = (_formatter.Deserialize(_file) as Data.IDArray).IDs.ToList();
                _file.Close();
            }

            int id;

            do
                id = Random.Range(0, 100000);
            while (File.Exists($"{Application.persistentDataPath}/Caravan{id}.dat"));

            caravansID.Add(id);

            _file = File.Create($"{Application.persistentDataPath}/CaravansID.dat");
            _formatter.Serialize(_file, new Data.IDArray { IDs = caravansID.ToArray() });
            _file.Close();

            _file = File.Create($"{Application.persistentDataPath}/Caravan{id}.dat");
            _formatter.Serialize(_file, new Data.Caravan(null, -1, -1, id));
            _file.Close();
        }
    }
}

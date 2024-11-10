using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

[System.Serializable]
public class QuestClass
{
    public Slot[] Items;

    public Slot Revard;
    public string Name;
    public string Task;
    public string AfterGive;
    public int ID;
}

[System.Serializable]
public record QuestRecord
{
    public QuestRecord(QuestClass questClass, bool isComplete)
    {
        Name = questClass.Name;
        Task = questClass.Task;
        ID = questClass.ID;
        IsComplete = isComplete;
    }

    public string Name;
    public string Task;
    public int ID;
    public bool IsComplete;
}

[System.Serializable]
public record ActiveQuestIDsRecord
{
    public int[] IDs;
}

namespace Interact
{
    public class QuestGive : AbstractInteract
    {
        private List<int> ids = new();

        private BinaryFormatter _formatter = new();
        public QuestClass Quest;
        public bool IsGive = false;
        public bool IsComplete = false;

        private UI.QuestMenu _questUI;
        private FileStream _file;

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Player"))
                _questUI.SwitchMenu(this, "", false);
        }

        private void OnDisable()
        {
            if (!IsGive && !IsComplete) return;

            _file = File.Create($"{Application.persistentDataPath}/Quest{Quest.ID}.dat");
            _formatter.Serialize(_file, new QuestRecord(Quest, IsComplete));
            _file.Close();

            if (File.Exists($"{Application.persistentDataPath}/ActiveQuests.dat"))
            {
                _file = File.Open($"{Application.persistentDataPath}/ActiveQuests.dat", FileMode.Open);
                ActiveQuestIDsRecord data = (ActiveQuestIDsRecord)_formatter.Deserialize(_file);
                _file.Close();

                ids = data.IDs.ToList();

                if (IsComplete)
                    ids.Remove(Quest.ID);
                else
                    ids.Add(Quest.ID);
            }
            else if (!IsComplete)
                ids.Add(Quest.ID);

            _file = File.Create($"{Application.persistentDataPath}/ActiveQuests.dat");
            _formatter.Serialize(_file, new ActiveQuestIDsRecord { IDs = ids.ToArray() });
            _file.Close();
        }

        private void Start()
        {
            if (File.Exists($"{Application.persistentDataPath}/Quest{Quest.ID}.dat"))
            {
                _file = File.Open($"{Application.persistentDataPath}/Quest{Quest.ID}.dat", FileMode.Open);
                QuestRecord data = (QuestRecord)_formatter.Deserialize(_file);
                _file.Close();

                IsGive = true;
                IsComplete = data.IsComplete;
            }

            _questUI = UI.QuestMenu.StaticQuest;
        }

        public override void Interact()
        {
            if (!IsComplete)
                _questUI.SwitchMenu(this, IsGive ? Quest.AfterGive : Quest.Task);
        }

        public void CompleteQuest() => IsComplete = true;

        public void GiveQuest() => IsGive = true;
    }
}

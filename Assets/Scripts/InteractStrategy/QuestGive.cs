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
public record QuestData
{
    public QuestData(QuestClass questClass, bool isComplete)
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
public record ActiveQuestsIDData
{
    public int[] IDs;
}

namespace Interact
{
    public class QuestGive : AbstractInteract, ITalk
    {
        private List<int> ids = new();

        public QuestClass Quest;

        private BinaryFormatter _formatter = new();
        private FileStream _file;
        private UI.TalkMenu _talkMenu;
        private UI.QuestList _questList;
        private bool _isGive = false;
        private bool _isComplete = false;

        private void OnDisable()
        {
            if (!_isGive && !_isComplete) return;

            _file = File.Create($"{Application.persistentDataPath}/Quest{Quest.ID}.dat");
            _formatter.Serialize(_file, new QuestData(Quest, _isComplete));
            _file.Close();

            if (File.Exists($"{Application.persistentDataPath}/ActiveQuests.dat"))
            {
                _file = File.Open($"{Application.persistentDataPath}/ActiveQuests.dat", FileMode.Open);
                ActiveQuestsIDData data = (ActiveQuestsIDData)_formatter.Deserialize(_file);
                _file.Close();

                ids = data.IDs.ToList();

                if (_isComplete)
                    ids.Remove(Quest.ID);
                else
                    ids.Add(Quest.ID);
            }
            else if (!_isComplete)
                ids.Add(Quest.ID);

            _file = File.Create($"{Application.persistentDataPath}/ActiveQuests.dat");
            _formatter.Serialize(_file, new ActiveQuestsIDData { IDs = ids.ToArray() });
            _file.Close();
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Player"))
                _talkMenu.SwitchMenu(this, "", false);
        }

        private void Start()
        {
            if (File.Exists($"{Application.persistentDataPath}/Quest{Quest.ID}.dat"))
            {
                _file = File.Open($"{Application.persistentDataPath}/Quest{Quest.ID}.dat", FileMode.Open);
                QuestData data = (QuestData)_formatter.Deserialize(_file);
                _file.Close();

                _isGive = true;
                _isComplete = data.IsComplete;
            }

            _questList = FindObjectOfType<UI.QuestList>();
            _talkMenu = UI.TalkMenu.StaticTalk;
        }

        public override void Interact()
        {
            if (!_isComplete)
                _talkMenu.SwitchMenu(this, _isGive ? Quest.AfterGive : Quest.Task);
        }

        public void Talk()
        {
            if (_isGive && FindObjectOfType<PlayerComponent.Inventory>().DeleteSlots(Quest.Items))
            {
                _isComplete= true;
                FindObjectOfType<PlayerComponent.Inventory>().AddItem(ref Quest.Revard);
                _questList.RemoveQuestLabel(Quest.ID);
            }
            else if (!_isGive)
            {
                _isGive = true;
                _questList.AddQuestLabel(Quest.Name, Quest.Task, Quest.ID);
            }
        }
    }
}

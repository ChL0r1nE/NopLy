using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public record IDArray
    {
        public int[] IDs;
    }

    [System.Serializable]
    public record Quest
    {
        public Quest(Interact.QuestClass questClass, bool isComplete)
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
}

namespace Interact
{
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

    public class QuestGive : AbstractInteract, ITalk
    {
        private List<int> ids = new();

        public QuestClass Quest;

        private UI.Talk _talkUI;
        private UI.QuestList _questList;
        private bool _isGive = false;
        private bool _isComplete = false;

        private readonly Serialize _serialize = new();

        private void OnDisable()
        {
            if (!_isGive && !_isComplete) return;

            _serialize.CreateSave($"Quest{Quest.ID}", new Data.Quest(Quest, _isComplete));

            if (_serialize.ExistSave("QuestsID"))
            {
                Data.IDArray idArray = _serialize.LoadSave<Data.IDArray>("QuestsID");
                ids = idArray.IDs.ToList();

                if (_isComplete)
                    ids.Remove(Quest.ID);
                else
                    ids.Add(Quest.ID);
            }
            else if (!_isComplete)
                ids.Add(Quest.ID);

            _serialize.CreateSave("QuestsID", new Data.IDArray { IDs = ids.ToArray() });
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Player"))
                _talkUI.SwitchMenu(this, "", false);
        }

        private void Start()
        {
            if (_serialize.ExistSave($"Quest{Quest.ID}"))
            {
                _isComplete = _serialize.LoadSave<Data.Quest>($"Quest{Quest.ID}").IsComplete; //Destroy if Complete
                _isGive = true;
            }

            _questList = FindObjectOfType<UI.QuestList>();
            _talkUI = UI.Talk.StaticTalk;
        }

        public override void Interact()
        {
            if (!_isComplete)
                _talkUI.SwitchMenu(this, _isGive ? Quest.AfterGive : Quest.Task);
        }

        public void Talk()
        {
            if (_isGive && FindObjectOfType<PlayerComponent.Inventory>().DeleteSlots(Quest.Items))
            {
                _isComplete = true;
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

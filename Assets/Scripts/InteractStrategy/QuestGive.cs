using UnityEngine;
using System;

[Serializable]
public class Quest
{
    public Slot QuestItem;

    public string QuestName;
    public string QuestTask;
    public int TargetValue;
}

namespace Interact
{
    public class QuestGive : AbstractInteract
    {
        [SerializeField] private Quest _quest;

        private UI.QuestMenu _questUI;
        private bool _isGive;

        private void Start() => _questUI = UI.QuestMenu.StaticUIQuest;

        public Quest GetQuestToGive()
        {
            _isGive = true;
            return _quest;
        }

        public override void Interact()
        {
            if (!_isGive)
                _questUI.SwitchMenu(this);
        }
    }
}

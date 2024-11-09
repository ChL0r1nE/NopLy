using System;

[Serializable]
public class Quest
{
    public Slot[] QuestItems;
    public Slot QuestRevard;

    public string QuestName;
    public string QuestTask;
    public string AfterGive;
}

namespace Interact
{
    public class QuestGive : AbstractInteract
    {
        public bool IsGive;

        public Quest Quest;

        private UI.QuestMenu _questUI;

        private void Start() => _questUI = UI.QuestMenu.StaticUIQuest;

        public override void Interact() => _questUI.SwitchMenu(this, IsGive);
    }
}

using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class QuestMenu : MonoBehaviour
    {
        public static QuestMenu StaticUIQuest;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Text _text;
        private Interact.QuestGive _questGive;
        private Quest _quest;
        private Vector2 _position = new(0, -1000);
        private bool _isOpen;

        private void Awake() => StaticUIQuest = this;

        private void Update()
        {
            if (_rectTransform.anchoredPosition.y == (_isOpen ? 0 : -1000)) return;

            _position.y = Mathf.MoveTowards(_rectTransform.anchoredPosition.y, _isOpen ? 0 : -1000, Time.deltaTime * 5000);
            _rectTransform.anchoredPosition = _position;
        }

        public void SwitchMenu(Interact.QuestGive questQive, bool isGive)
        {
            _isOpen = !_isOpen;

            if (_isOpen)
            {
                _questGive = questQive;
                _quest = _questGive.Quest;
                _text.text = isGive ? _quest.AfterGive : _quest.QuestTask;
            }
        }

        public void Choice(int choice)
        {
            _isOpen = false;
            if (choice == 0) return;

            if (_questGive.IsGive && FindObjectOfType<PlayerComponent.Inventory>().DeleteRecipe(_quest.QuestItems))
                FindObjectOfType<PlayerComponent.Inventory>().AddItem(_quest.QuestRevard, out _);
            else if(!_questGive.IsGive)
            {
                StaticQuestList.Quests.Add(_quest);
                _questGive.IsGive = true;
            }
        }
    }
}
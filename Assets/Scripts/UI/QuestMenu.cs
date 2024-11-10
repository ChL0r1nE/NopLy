using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class QuestMenu : MonoBehaviour
    {
        public static QuestMenu StaticQuest;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Text _text;
        [SerializeField] private QuestList _questList;
        private Interact.QuestGive _questGive;
        private QuestClass _quest;
        private Vector2 _position = new(0, -1000);
        private bool _isOpen;

        private void Awake() => StaticQuest = this;

        private void Update()
        {
            if (_rectTransform.anchoredPosition.y == (_isOpen ? 0 : -1000)) return;

            _position.y = Mathf.MoveTowards(_rectTransform.anchoredPosition.y, _isOpen ? 0 : -1000, Time.deltaTime * 5000);
            _rectTransform.anchoredPosition = _position;
        }

        public void SwitchMenu(Interact.QuestGive questQive, string text, bool baseOpen = true)
        {
            _isOpen = !_isOpen && baseOpen;

            if (_isOpen)
            {
                _questGive = questQive;
                _quest = _questGive.Quest;
                _text.text = text;
            }
        }

        public void Choice(int choice)
        {
            _isOpen = false;
            if (choice == 0) return;

            if (_questGive.IsGive && FindObjectOfType<PlayerComponent.Inventory>().DeleteRecipe(_quest.Items))
            {
                FindObjectOfType<PlayerComponent.Inventory>().AddItem(_quest.Revard, out _);
                _questList.RemoveQuestLabel(_quest.ID);
                _questGive.CompleteQuest();
            }
            else if (!_questGive.IsGive)
            {
                _questGive.GiveQuest();
                _questList.AddQuestLabel(_quest.Name, _quest.Task);
            }
        }
    }
}
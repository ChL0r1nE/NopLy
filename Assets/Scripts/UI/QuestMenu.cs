using UnityEngine;

namespace UI
{
    public class QuestMenu : MonoBehaviour
    {
        public static QuestMenu StaticUIQuest;

        [SerializeField] private RectTransform _rectTransform;
        private Interact.QuestGive _questGive;
        private Vector2 _position = new(0, -1000);
        private bool _isOpen;

        private void Awake() => StaticUIQuest = this;

        private void Update()
        {
            if (_rectTransform.anchoredPosition.y == (_isOpen ? 0 : -1000)) return;

            _position.y = Mathf.MoveTowards(_rectTransform.anchoredPosition.y, _isOpen ? 0 : -1000, Time.deltaTime * 5000);
            _rectTransform.anchoredPosition = _position;
        }

        public void SwitchMenu(Interact.QuestGive questQive)
        {
            _isOpen = !_isOpen;

            if (_isOpen)
                _questGive = questQive;
        }

        public void Choice(int choice)
        {
            if (choice != 0)
                StaticQuestList.Quests.Add(_questGive.GetQuestToGive());

            _isOpen = false;
        }
    }
}
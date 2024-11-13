using UnityEngine.UI;
using UnityEngine;
using Interact;

namespace UI
{
    public class TalkMenu : MonoBehaviour
    {
        public static TalkMenu StaticTalk;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Text _text;
        [SerializeField] private QuestList _questList;
        private ITalk _iTalk;
        private Vector2 _position = new(0, -1000);
        private bool _isOpen;

        private void Awake() => StaticTalk = this;

        private void Update()
        {
            if (_rectTransform.anchoredPosition.y == (_isOpen ? 0 : -1000)) return;

            _position.y = Mathf.MoveTowards(_rectTransform.anchoredPosition.y, _isOpen ? 0 : -1000, Time.deltaTime * 5000);
            _rectTransform.anchoredPosition = _position;
        }

        public void SwitchMenu(ITalk iTalk, string text, bool baseOpen = true)
        {
            _isOpen = !_isOpen && baseOpen;

            if (!_isOpen) return;

            _iTalk = iTalk;
            _text.text = text;
        }

        public void Choice(bool choice)
        {
            _isOpen = false;

            if (choice)
                _iTalk.Talk();
        }
    }
}
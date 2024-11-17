using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class Caravan : MonoBehaviour
    {
        public void AddCaravan(int i) => _caravaner.AddCaravan(i);

        public void Close() => _isOpen = false;

        [SerializeField] private RectTransform _iconPrefab;
        private RectTransform _rectTransform;
        private Interact.Caravaner _caravaner;
        private Vector2 _iconPosition = new(-160, -80);
        private Vector2 _rectPosition = new(0, -1000);
        private bool _isOpen = false;

        private void Start() => _rectTransform = GetComponent<RectTransform>();

        private void Update()
        {
            _rectPosition.y = Mathf.MoveTowards(_rectPosition.y, _isOpen ? 0 : -1000, Time.deltaTime * 5000);
            _rectTransform.anchoredPosition = _rectPosition;
        }

        public void SetCaravaner(Interact.Caravaner caravaner, List<Slot[]> slots)
        {
            _isOpen = true;
            _caravaner = caravaner;

            foreach (Slot[] slotsLine in slots)
            {
                _iconPosition.x += 80;
                _iconPosition.y = -80;

                foreach (Slot slot in slotsLine)
                {
                    _iconPosition.y += 80;

                    RectTransform iconRect = Instantiate(_iconPrefab, transform);
                    iconRect.GetComponent<RectTransform>().anchoredPosition = _iconPosition;
                    iconRect.transform.GetChild(0).GetComponent<Image>().sprite = slot.Item.Sprite;
                    iconRect.transform.GetChild(1).GetComponent<Text>().text = slot.Count != 1 ? $"{slot.Count}" : "";
                }
            }
        }
    }
}

using UnityEngine;

namespace UI
{
    public class Caravan : MonoBehaviour
    {
        public void AddCaravan() => _caravaner.AddUnit(UnitType.Caravan);

        public void AddRepairSquad() => _caravaner.AddUnit(UnitType.RepairSquad);

        public void SetCaravaner(Interact.Caravaner caravaner) => _caravaner = caravaner;

        public void SetOpen(bool open) => _isOpen = open;

        private RectTransform _rectTransform;
        private Interact.Caravaner _caravaner;
        private Vector2 _rectPosition = new(0, -1000);
        private bool _isOpen = false;

        private void Start() => _rectTransform = GetComponent<RectTransform>();

        private void Update()
        {
            _rectPosition.y = Mathf.MoveTowards(_rectPosition.y, _isOpen ? 0 : -1000, Time.deltaTime * 5000);
            _rectTransform.anchoredPosition = _rectPosition;
        }
    }
}

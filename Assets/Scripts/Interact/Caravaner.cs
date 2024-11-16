using UnityEngine;

namespace Interact
{
    public class Caravaner : AbstractInteract
    {
        [SerializeField] private int _mapID;
        private UI.Caravan _caravanUI;
        private bool _isOpen = false;

        private void Start()
        {
            _caravanUI = FindObjectOfType<UI.Caravan>();
            _caravanUI.SetCaravanMenu(_mapID);
        }

        public override void Interact()
        {
            _isOpen = !_isOpen;
            _caravanUI.SetOpen(_isOpen);
        }
    }
}

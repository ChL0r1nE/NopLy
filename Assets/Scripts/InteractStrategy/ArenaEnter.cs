using UnityEngine;

namespace Interact
{
    public class ArenaEnter : AbstractInteract, ITalk
    {
        public void SetClose() => _isClose = true;

        private UI.TalkMenu _talkMenu;
        [SerializeField] private ArenaEnemySpawn _arenaSpawn;
        [SerializeField] private Slot _revard;
        [SerializeField, TextArea] string _message;
        [SerializeField, TextArea] string _afterFightMessage;
        private bool _isClose = false;

        private void Start() => _talkMenu = UI.TalkMenu.StaticTalk;

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Player"))
                _talkMenu.SwitchMenu(this, "", false);
        }

        public override void Interact() => _talkMenu.SwitchMenu(this, _isClose ? _afterFightMessage : _message);

        public void Talk()
        {
            if (_isClose)
            {
                _isClose = false;
                Slot slot = new(_revard.Item, _revard.Count);
                FindObjectOfType<PlayerComponent.Inventory>().AddItem(ref slot);
            }
            else
                _arenaSpawn.Open();
        }
    }
}

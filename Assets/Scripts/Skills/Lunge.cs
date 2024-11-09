using UnityEngine;

namespace Skill
{
    public class Lunge : AbstractSkill
    {
        [SerializeField] private PlayerComponent.Move _playerMove;

        public override void Execute() => _playerMove.AddImpulse();
    }
}

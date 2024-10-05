using UnityEngine;

public class LungeSkill : SkillInfo
{
    [SerializeField] private PlayerMoveScript _playerMoveScript;

    public override void Execute() => _playerMoveScript.AddImpulse();
}

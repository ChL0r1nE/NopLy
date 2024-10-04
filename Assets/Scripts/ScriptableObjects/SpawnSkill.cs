using UnityEngine;

public class SpawnSkill : SkillInfo
{
    [SerializeField] private GameObject _gameObject;

    public override void Execute()
    {
        Instantiate(_gameObject, transform.position, Quaternion.identity);
    }
}

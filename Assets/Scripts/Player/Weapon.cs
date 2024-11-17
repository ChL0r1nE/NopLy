using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class Weapon : MonoBehaviour
    {
        public Info.Weapon GetInfoWeapon() => _weapon == _defaultMainWeapon ? null : _weapon;

        public void SetWeaponSlot(WeaponSlot weaponSlot) => SetWeapon(weaponSlot != null ? weaponSlot.Item as Info.Weapon : _defaultMainWeapon, weaponSlot != null ? weaponSlot.Endurance : int.MaxValue);

        [System.Serializable]
        public class SkillClass
        {
#if UNITY_EDITOR
            public string SkillName;
#endif

            public Skill.AbstractSkill[] SkillComponents;
            public string[] AnimTriggers;

            [SerializeField] public string _name;
            public Sprite Sprite;
            public Info.WeaponType WeaponType;
            public int WeaponEndurance;
            public int ReloadTime;

            public void Execute()
            {
                foreach (Skill.AbstractSkill skill in SkillComponents)
                    skill.Execute();
            }
        }

        public int Endurance { get; private set; }

        private List<int> _activeSkillsID = new();

        [SerializeField] private SkillClass[] _skills;
        private float[] _skillReset;

        [SerializeField] private Animator _animator;
        [SerializeField] private MeshFilter _weaponMesh;
        [SerializeField] private Move _playerMove;
        [SerializeField] private Attack _playerAttack;
        [SerializeField] private EnemyTarget _playerEnemyTarget;
        [SerializeField] private Info.Weapon _defaultMainWeapon;
        private UI.Ammunition _ammunitionUI;
        private UI.SkillList _skillList;
        private Info.Weapon _weapon;
        private int _skillNumber;

        private void Update()
        {
            if (_activeSkillsID.Count > 0)
            {
                for (int i = 0; i < _activeSkillsID.Count; i++)
                    if (_skillReset[i] < 1)
                        _skillReset[i] += Time.deltaTime / _skills[_activeSkillsID[i]].ReloadTime;

                _skillList.UpdateReload(_skillReset);
            }

            _skillNumber = Input.GetKeyDown(KeyCode.Q) ? 0 : Input.GetKeyDown(KeyCode.F) ? 1 : -1;

            if (_skillNumber != -1 && _skillNumber < _activeSkillsID.Count && _skillReset[_skillNumber] >= 1f)
            {
                _skillReset[_skillNumber] = 0f;
                _skillNumber = _activeSkillsID[_skillNumber];

                foreach (string name in _skills[_skillNumber].AnimTriggers)
                    _animator.SetTrigger(name);

                WeaponUse(_skills[_skillNumber].WeaponEndurance);
                _skills[_skillNumber].Execute();

                return;
            }

            if (!Input.GetKeyDown(KeyCode.Mouse0)) return;

            if (_playerEnemyTarget.EnemyTransform)
                RotateToTransforn(_playerEnemyTarget.EnemyTransform.position);
            else
                _animator.SetTrigger("Strike");
        }

        public void Inizilize()
        {
            _skillList = FindObjectOfType<UI.SkillList>();
            _ammunitionUI = FindObjectOfType<UI.Ammunition>();
            _ammunitionUI.SetPlayerWeapon(this);
        }

        public void WeaponUse(int use)
        {
            Endurance -= use;
            _ammunitionUI.SetEndurance((float)Endurance / _weapon.MaxEndurance);

            if (Endurance < 0f)
                SetWeapon(_defaultMainWeapon, int.MaxValue);
        }

        public void RotateToTransforn(Vector3 position)
        {
            position = transform.position - position;
            float angle = Mathf.Atan(position.x / position.z) * 57;

            if (position.z > 0)
                angle += 180;

            _playerMove.SetTargetRotation(Mathf.Repeat(angle, 359));
        }

        private void SetWeapon(Info.Weapon weapon, int endurance)
        {
            if (!_weapon || _weapon.WeaponType != weapon.WeaponType)
            {
                _animator.SetTrigger($"{weapon.WeaponType}");

                _activeSkillsID.Clear();

                for (int i = 0; i < _skills.Length; i++)
                    if (_skills[i].WeaponType == weapon.WeaponType)
                        _activeSkillsID.Add(i);

                _skillReset = new float[_activeSkillsID.Count];
                Sprite[] skillSprites = new Sprite[_activeSkillsID.Count];

                for (int i = 0; i < _activeSkillsID.Count; i++)
                    skillSprites[i] = _skills[_activeSkillsID[i]].Sprite;

                _skillList.SetSkills(skillSprites);
            }

            _weapon = weapon;
            Endurance = endurance;
            _weaponMesh.mesh = _weapon.WeaponMesh;
            _playerAttack.WeaponDamage = _weapon.Damage;

            _ammunitionUI.SetWeaponImage(_weapon);
            _ammunitionUI.SetEndurance((float)Endurance / _weapon.MaxEndurance);
        }
    }
}

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace PlayerComponent
{
    public class Weapon : MonoBehaviour
    {
        [System.Serializable]
        public class SkillClass
        {
#if UNITY_EDITOR
            public string SkillName;
#endif
            public bool SkillReady => _resetTimer >= 1f;

            public void SetImage(Image image)
            {
                _skillImage = image;

                if (!_skillImage)
                    _resetTimer = 0;
            }

            public Skill.AbstractSkill[] SkillComponents;
            public string[] AnimTriggers;

            public Sprite Sprite;
            public Info.WeaponType WeaponType;
            public int WeaponEndurance;

            [SerializeField] private int _reloadTime;
            [SerializeField] public string _name;
            private Image _skillImage;
            private float _resetTimer = 0;

            public void FrameUpdate()
            {
                if (!_skillImage) return;

                _resetTimer += Time.deltaTime / _reloadTime;
                _skillImage.fillAmount = _resetTimer;
            }

            public void Execute()
            {
                foreach (Skill.AbstractSkill skill in SkillComponents)
                    skill.Execute();

                _resetTimer = 0;
            }
        }

        private List<int> _activeSkillsID = new();

        [SerializeField] private Image[] _skillImages;
        [SerializeField] private SkillClass[] _skills;

        public int WeaponEndurance { get; private set; }

        [SerializeField] private UI.Ammunition _ammunitionUI;
        [SerializeField] private Image _weaponImage;
        [SerializeField] private Image _weaponEnduranceImage;
        [SerializeField] private Animator _animator;
        [SerializeField] private MeshFilter _weaponMesh;
        [SerializeField] private Move _playerMove;
        [SerializeField] private Attack _playerAttack;
        [SerializeField] private EnemyTarget _playerEnemyTarget;
        [SerializeField] private Info.Weapon _defaultMainWeapon;
        private Image _skillImage;
        private Info.Weapon _weapon;
        private Vector2 _targetImagePos;
        private float _imageMoveTime = 0;
        private int _nowSkills;
        private int _skillNumber;

        public Info.Weapon GetInfoWeapon() => _weapon == _defaultMainWeapon ? null : _weapon;

        public void SetWeaponSlot(WeaponSlot weaponSlot) => SetWeapon(weaponSlot != null ? weaponSlot.Item as Info.Weapon : _defaultMainWeapon, weaponSlot != null ? weaponSlot.Endurance : int.MaxValue);

        private void Start()
        {
            _weapon = _defaultMainWeapon;
            SetWeapon(_defaultMainWeapon, int.MaxValue);
        }

        private void Update()
        {
            if (_imageMoveTime < 1)
            {
                _imageMoveTime += Time.deltaTime * 3;

                for (int i = 0; i < _skillImages.Length; i++)
                {
                    _targetImagePos.y = Mathf.Lerp(0, 10 + 100 * (i + 1), _imageMoveTime);
                    _skillImages[i].GetComponent<RectTransform>().anchoredPosition = _targetImagePos;
                }
            }

            foreach (SkillClass skill in _skills)
                skill.FrameUpdate();

            _skillNumber = Input.GetKeyDown(KeyCode.Q) ? 0 : Input.GetKeyDown(KeyCode.F) ? 1 : -1;

            if (_skillNumber != -1 && _skillNumber < _activeSkillsID.Count && _skills[_activeSkillsID[_skillNumber]].SkillReady)
            {
                foreach (string name in _skills[_activeSkillsID[_skillNumber]].AnimTriggers)
                    _animator.SetTrigger(name);

                WeaponUse(_skills[_activeSkillsID[_skillNumber]].WeaponEndurance);
                _skills[_activeSkillsID[_skillNumber]].Execute();

                return;
            }

            if (!Input.GetKeyDown(KeyCode.Mouse0)) return;

            if (_playerEnemyTarget.EnemyTransform)
                RotateToTransforn(_playerEnemyTarget.EnemyTransform.position);
            else
                _animator.SetTrigger("Strike");
        }

        public void WeaponUse(int use)
        {
            WeaponEndurance -= use;
            _weaponEnduranceImage.fillAmount = (float)WeaponEndurance / _weapon.MaxEndurance;
            _weaponEnduranceImage.color = Color.green * ((float)WeaponEndurance / _weapon.MaxEndurance) + Color.red * (1 - (float)WeaponEndurance / _weapon.MaxEndurance);

            if (WeaponEndurance < 0)
                SetWeapon(_defaultMainWeapon, int.MaxValue);
        }

        public void RotateToTransforn(Vector3 position)
        {
            position = transform.position - position;
            float angle = Mathf.Atan(position.x / position.z) * 57;

            if (position.z > 0)
                angle += 180;

            angle = Mathf.Repeat(angle, 359);
            _playerMove.SetTargetRotation(angle);
        }

        private void SetWeapon(Info.Weapon weapon, int endurance)
        {
            if (_weapon.WeaponType != weapon.WeaponType)
            {
                _nowSkills = 0;
                _imageMoveTime = 0;
                _animator.SetTrigger($"{weapon.WeaponType}");

                _activeSkillsID.Clear();

                for (int i = 0; i < _skills.Length; i++)
                {
                    if (_skills[i].WeaponType != weapon.WeaponType)
                    {
                        _skills[i].SetImage(null);
                        continue;
                    }

                    _activeSkillsID.Add(i);
                    _skills[i].SetImage(_skillImages[_nowSkills]);

                    _skillImage = _skillImages[_nowSkills];
                    _skillImage.gameObject.SetActive(true);
                    _skillImage.GetComponent<Image>().sprite = _skills[i].Sprite;
                    _skillImage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    _skillImage = _skillImages[_nowSkills++].transform.GetChild(0).GetComponent<Image>();
                    _skillImage.sprite = _skills[i].Sprite;
                    _skills[i].SetImage(_skillImage);
                }

                for (int i = _nowSkills; i < _skillImages.Length; i++)
                    _skillImages[i].gameObject.SetActive(false);
            }

            _weapon = weapon;
            WeaponEndurance = endurance;
            _weaponMesh.mesh = _weapon.WeaponMesh;
            _weaponImage.sprite = _weapon.Sprite;
            _playerAttack.WeaponDamage = _weapon.Damage;

            WeaponUse(0);
            _ammunitionUI.UpdateWeaponImage();
        }
    }
}

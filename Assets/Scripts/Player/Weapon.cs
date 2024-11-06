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
                _skillImage = image ? image.transform.GetChild(0).GetComponent<Image>() : null;

                if (!_skillImage)
                    _resetTimer = 0;
            }

            public Skill.AbstractSkill[] SkillComponents;
            public string[] AnimTriggers;

            public Sprite Sprite;
            public WeaponType WeaponType;

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
                foreach (Skill.AbstractSkill skillInfo in SkillComponents)
                    skillInfo.Execute();

                _resetTimer = 0;
            }
        }

        public WeaponInfo WeaponInfo
        {
            get => _weaponInfo != _defaultMainWeapon ? _weaponInfo : null;
            set => SetWeapon(value ? value : _defaultMainWeapon);
        }

        private WeaponInfo _weaponInfo;

        private List<int> _activeSkillsID = new();

        [SerializeField] private Image[] _skillImages;
        [SerializeField] private SkillClass[] _skills;

        [SerializeField] private InventoryUI.Ammunition _inventoryUI;
        [SerializeField] private Image _weaponImage;
        [SerializeField] private Animator _animator;
        [SerializeField] private MeshFilter _weaponMesh;
        [SerializeField] private Move _playerMove;
        [SerializeField] private Attack _playerAttack;
        [SerializeField] private EnemyTarget _playerEnemyTarget;
        [SerializeField] private WeaponInfo _defaultMainWeapon;
        private Vector2 _targetImagePos;
        private int _nowSkills;
        private int _skillNumber;
        private float _imageMoveTime = 0;

        private void Start()
        {
            _weaponInfo = _defaultMainWeapon;
            SetWeapon(_weaponInfo);
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

                _skills[_activeSkillsID[_skillNumber]].Execute();

                return;
            }

            if (!Input.GetKeyDown(KeyCode.Mouse0)) return;

            if (_playerEnemyTarget.EnemyTransform)
                RotateToTransforn(_playerEnemyTarget.EnemyTransform.position);
            else
                _animator.SetTrigger("Strike");
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

        private void SetWeapon(WeaponInfo info)
        {
            if (_weaponInfo.WeaponType != info.WeaponType)
            {
                _nowSkills = 0;
                _imageMoveTime = 0;
                _animator.SetTrigger($"{info.WeaponType}");

                _activeSkillsID.Clear();

                for (int i = 0; i < _skills.Length; i++)
                {
                    if (_skills[i].WeaponType != info.WeaponType)
                    {
                        _skills[i].SetImage(null);
                        continue;
                    }

                    _activeSkillsID.Add(i);
                    _skills[i].SetImage(_skillImages[_nowSkills]);

                    _skillImages[_nowSkills].gameObject.SetActive(true);
                    _skillImages[_nowSkills].GetComponent<Image>().sprite = _skills[i].Sprite;
                    _skillImages[_nowSkills].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    _skillImages[_nowSkills++].transform.GetChild(0).GetComponent<Image>().sprite = _skills[i].Sprite;
                }

                for (int i = _nowSkills; i < _skillImages.Length; i++)
                    _skillImages[i].gameObject.SetActive(false);
            }

            _weaponInfo = info;
            _weaponMesh.mesh = _weaponInfo.WeaponMesh;
            _weaponImage.sprite = _weaponInfo.Sprite;
            _playerAttack.WeaponDamage = _weaponInfo.Damage;

            _inventoryUI.UpdateMenu(null);
        }
    }
}

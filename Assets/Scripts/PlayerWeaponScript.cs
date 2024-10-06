using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerWeaponScript : MonoBehaviour
{
    [System.Serializable]
    public class Skill
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

        public SkillInfo[] SkillComponents;
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
            if (_resetTimer < 1f) return;

            foreach (SkillInfo skillInfo in SkillComponents)
                skillInfo.Execute();

            _resetTimer = 0;
        }
    }

    public WeaponInfo WeaponInfo;

    private List<int> _activeSkillsID = new();

    [SerializeField] private Image[] _skillImages;
    [SerializeField] private Skill[] _skills;

    [SerializeField] private Image _weaponImage;
    [SerializeField] private Animator _animator;
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private PlayerMoveScript _playerMoveScript;
    [SerializeField] private PlayerAttackScript _playerAttackScript;
    [SerializeField] private PlayerEnemyTargetScript _playerEnemyTargetScript;
    private Vector2 _targetImagePos;
    private int _nowSkills;
    private int _skillNumber;
    private float _imageMoveTime = 0;

    private void Start() => SetWeapon(WeaponInfo);

    private void Update()
    {
        if (_imageMoveTime < 1)
        {
            _imageMoveTime += Time.deltaTime * 3;

            for (int i = 0; i < _skillImages.Length; i++)
            {
                _targetImagePos.y = Mathf.Lerp(0, 10 + 100 * (i + 1), _imageMoveTime);
                _skillImages[i].transform.parent.GetComponent<RectTransform>().anchoredPosition = _targetImagePos;
            }
        }

        foreach (Skill skill in _skills)
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

        if (_playerEnemyTargetScript.EnemyTransform)
            RotateToTransforn(_playerEnemyTargetScript.EnemyTransform.position);
        else
            _animator.SetTrigger("Strike");
    }

    public void RotateToTransforn(Vector3 position)
    {
        position = transform.position - position;

        float Angle = Mathf.Atan(position.x / position.z) * 57;

        if (position.z > 0)
            Angle += 180;

        _playerMoveScript.SetTargetRotation(Angle);
    }

    public void SetWeapon(WeaponInfo info)
    {
        if (WeaponInfo.WeaponType != info.WeaponType)
        {
            _nowSkills = -1;
            _imageMoveTime = 0;
            _animator.SetTrigger(info.WeaponType.ToString());

            _activeSkillsID.Clear();

            for (int i = 0; i < _skills.Length; i++)
            {
                if (_skills[i].WeaponType != info.WeaponType)
                {
                    _skills[i].SetImage(null);
                    continue;
                }

                _nowSkills++;
                _activeSkillsID.Add(i);

                _skills[i].SetImage(_skillImages[_nowSkills]);

                _skillImages[_nowSkills].transform.parent.gameObject.SetActive(true);
                _skillImages[_nowSkills].transform.parent.GetComponent<Image>().sprite = _skills[i].Sprite;
                _skillImages[_nowSkills].transform.parent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                _skillImages[_nowSkills].sprite = _skills[i].Sprite;
            }

            for (int i = _nowSkills + 1; i < +_skillImages.Length; i++)
                _skillImages[i].transform.parent.gameObject.SetActive(false);
        }

        WeaponInfo = info;

        _meshFilter.mesh = WeaponInfo.WeaponMesh;
        _weaponImage.sprite = WeaponInfo.Sprite;
        _playerAttackScript.SetWeaponDamage(WeaponInfo.Damage);
    }
}

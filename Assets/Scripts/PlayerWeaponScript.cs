using UnityEngine.UI;
using UnityEngine;

public class PlayerWeaponScript : MonoBehaviour
{
    [System.Serializable]
    public class Skill
    {
        public void SetImage(Image image) => _skillImage = image;

        public bool SkillReady => _resetTimer > 1;

        public SkillInfo[] SkillComponents;
        public string[] AnimTriggers;

        public Sprite Sprite;
        public WeaponType WeaponType;

        [SerializeField] private int _reloadTime;
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
            if (_resetTimer < 1) return;

            foreach (SkillInfo skillInfo in SkillComponents)
                skillInfo.Execute();

            _resetTimer = 0;
        }
    }

    public WeaponInfo WeaponInfo;

    [SerializeField] private Image[] _skillImages;
    [SerializeField] private Skill[] _skills; //AddArrayOfAktiveSkills

    [SerializeField] private Image _weaponImage;
    [SerializeField] private Animator _animator;
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private PlayerMoveScript _playerMoveScript;
    [SerializeField] private PlayerAttackScript _playerAttackScript;
    [SerializeField] private PlayerEnemyTargetScript _playerEnemyTargetScript;
    private int _nowSkills;
    private float _imageMoveTime = 0;

    private void Start() => SetWeapon(WeaponInfo);

    private void Update()
    {
        if (_imageMoveTime < 1)
        {
            _imageMoveTime += Time.deltaTime;

            for (int i = 0; i < _skillImages.Length; i++) //new Vector x_x
                _skillImages[i].transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, 150 + Mathf.Lerp(0, 150 * (i + 1), _imageMoveTime * 3));
        }

        foreach (Skill skill in _skills)
            skill.FrameUpdate();

        if (WeaponInfo.WeaponType == WeaponType.Shaft) //TypeLock
        {
            int skillNumber = Input.GetKeyDown(KeyCode.Q) ? 0 : Input.GetKeyDown(KeyCode.E) ? 1 : -1;

            if (skillNumber != -1 && _skills[skillNumber].SkillReady)
            {
                foreach (string name in _skills[skillNumber].AnimTriggers)
                    _animator.SetTrigger(name);

                _skills[skillNumber].Execute();

                return;
            }
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

    public void SetWeapon(WeaponInfo info) //ReFactor
    {
        if (WeaponInfo.WeaponType != info.WeaponType)
        {
            _nowSkills = -1;
            _imageMoveTime = 0;
            _animator.SetTrigger(info.WeaponType.ToString());

            foreach (Image image in _skillImages)
                image.transform.parent.gameObject.SetActive(false);

            foreach (Skill skill in _skills)
            {
                if (skill.WeaponType != info.WeaponType)
                {
                    skill.SetImage(null);
                    continue;
                }

                _nowSkills++;
                skill.SetImage(_skillImages[_nowSkills]);

                _skillImages[_nowSkills].transform.parent.gameObject.SetActive(true);
                _skillImages[_nowSkills].transform.parent.GetComponent<Image>().sprite = skill.Sprite;
                _skillImages[_nowSkills].sprite = skill.Sprite;
            }
        }

        WeaponInfo = info;

        _meshFilter.mesh = WeaponInfo.WeaponMesh;
        _weaponImage.sprite = WeaponInfo.Sprite;
        _playerAttackScript.SetWeaponDamage(WeaponInfo.Damage);
    }
}

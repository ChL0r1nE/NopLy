using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace PlayerComponent
{
    public class Buff : MonoBehaviour
    {
        public static Buff StaticBuff;

        public record BuffClass
        {
            public BuffClass(BuffInfo info)
            {
                BuffInfo = info;
                Length = info.Length;
            }

            public BuffInfo BuffInfo;
            public float Length;
        }

        private List<RectTransform> _buffRectTransforms = new();
        private List<BuffClass> _buffs = new();

        [SerializeField] private GameObject _iconPrefab;
        [SerializeField] private Transform _buffIconParentTransform;
        [SerializeField] private Armor PlayerArmor;
        [SerializeField] private Health _playerHealth;
        private Vector2 _buffIconOffset = new(45, 0);

        private void Start() => StaticBuff = this;

        private void FixedUpdate()
        {
            for (int i = 0; i < _buffs.Count; i++)
            {
                if (_buffs[i].Length == -1) continue;

                _buffs[i].Length -= Time.fixedDeltaTime;

                if (_buffs[i].Length > 0) continue;

                Destroy(_buffRectTransforms[i].gameObject);

                _buffs.RemoveAt(i);
                _buffRectTransforms.RemoveAt(i);

                for (int j = i; j < _buffs.Count; j++)
                    _buffRectTransforms[j].anchoredPosition = _buffIconOffset * j;
            }
        }

        public void AddBuff(BuffInfo info)
        {
            if (info.Type == BuffType.Heal)
            {
                _playerHealth.HealthValue += info.Modifier;
                return;
            }

            for (int i = 0; i < _buffs.Count; i++)
            {
                if (_buffs[i].BuffInfo.Name != info.Name) continue;

                _buffs[i].Length = info.Length;
                return;
            }

            int buffNumder = _buffs.Count;
            _buffs.Add(new BuffClass(info));

            _buffRectTransforms.Add(Instantiate(_iconPrefab, _buffIconParentTransform).GetComponent<RectTransform>());
            _buffRectTransforms[buffNumder].GetComponent<Image>().sprite = _buffs[buffNumder].BuffInfo.Sprite;
            _buffRectTransforms[buffNumder].anchoredPosition = _buffIconOffset * buffNumder;

            switch (_buffs[buffNumder].BuffInfo.Type)
            {
                case BuffType.Armor:
                    PlayerArmor.ArmorBuffModifier += _buffs[buffNumder].BuffInfo.Modifier;
                    break;
            }
        }

        public void DeleteBuff(BuffInfo info)
        {
            for (int i = 0; i < _buffs.Count; i++)
            {
                if (_buffs[i].BuffInfo.Name != info.Name) continue;

                switch (_buffs[i].BuffInfo.Type)
                {
                    case BuffType.Armor:
                        PlayerArmor.ArmorBuffModifier -= _buffs[i].BuffInfo.Modifier;
                        break;
                }

                Destroy(_buffRectTransforms[i].gameObject);

                _buffs.RemoveAt(i);
                _buffRectTransforms.RemoveAt(i);

                for (int j = i; j < _buffs.Count; j++)
                    _buffRectTransforms[j].anchoredPosition = _buffIconOffset * j;

                return;
            }
        }
    }
}
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
            public BuffClass(Info.Buff buff)
            {
                Buff = buff;
                Timer = buff.Length;
            }

            public Info.Buff Buff;
            public float Timer;
        }

        private List<RectTransform> _buffRectTransforms = new();
        private List<BuffClass> _buffs = new();

        [SerializeField] private RectTransform _iconPrefab;
        [SerializeField] private Transform _buffIconParentTransform;
        [SerializeField] private Armor PlayerArmor;
        [SerializeField] private Health _playerHealth;
        private Vector2 _buffIconOffset = new(45, 0);
        private float _timer = 0;

        private void Start() => StaticBuff = this;

        private void FixedUpdate()
        {
            _timer += Time.fixedDeltaTime;

            if (_timer < 1) return;

            for (int i = 0; i < _buffs.Count; i++)
            {
                if (_buffs[i].Timer == -1) continue;

                _buffs[i].Timer -= _timer;

                if (_buffs[i].Timer > 0) continue;

                Destroy(_buffRectTransforms[i].gameObject);
                _buffRectTransforms.RemoveAt(i);
                _buffs.RemoveAt(i);

                for (int j = i; j < _buffs.Count; j++)
                    _buffRectTransforms[j].anchoredPosition = _buffIconOffset * j;
            }

            _timer = 0;
        }

        public void AddBuff(Info.Buff buff)
        {
            if (buff.Type == Info.BuffType.Heal)
            {
                _playerHealth.HealthChange(buff.Modifier);
                return;
            }

            for (int i = 0; i < _buffs.Count; i++)
            {
                if (_buffs[i].Buff.Name != buff.Name) continue;

                _buffs[i].Timer = buff.Length;
                return;
            }

            int buffNumder = _buffs.Count;
            _buffs.Add(new BuffClass(buff));

            _buffRectTransforms.Add(Instantiate(_iconPrefab, _buffIconParentTransform));
            _buffRectTransforms[buffNumder].GetComponent<Image>().sprite = _buffs[buffNumder].Buff.Sprite;
            _buffRectTransforms[buffNumder].anchoredPosition = _buffIconOffset * buffNumder;

            switch (_buffs[buffNumder].Buff.Type)
            {
                case Info.BuffType.Armor:
                    PlayerArmor.ArmorBuffModifier += _buffs[buffNumder].Buff.Modifier;
                    break;
            }
        }

        public void DeleteBuff(Info.Buff buff)
        {
            for (int i = 0; i < _buffs.Count; i++)
            {
                if (_buffs[i].Buff.Name != buff.Name) continue;

                switch (_buffs[i].Buff.Type)
                {
                    case Info.BuffType.Armor:
                        PlayerArmor.ArmorBuffModifier -= _buffs[i].Buff.Modifier;
                        break;
                }

                Destroy(_buffRectTransforms[i].gameObject);
                _buffRectTransforms.RemoveAt(i);
                _buffs.RemoveAt(i);

                for (int j = i; j < _buffs.Count; j++)
                    _buffRectTransforms[j].anchoredPosition = _buffIconOffset * j;

                return;
            }
        }
    }
}
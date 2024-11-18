using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class Buffs : MonoBehaviour
    {
        public static Buffs StaticBuff;

        public record BuffRecord
        {
            public BuffRecord(Info.Buff buff)
            {
                Buff = buff;
                Timer = buff.Length;
            }

            public Info.Buff Buff;
            public float Timer;
        }

        private List<BuffRecord> _buffs = new();

        [SerializeField] private Armor PlayerArmor;
        [SerializeField] private Health _playerHealth;
        private UI.BuffList _buffsList;
        private float _timer = 0;

        private void Start()
        {
            StaticBuff = this;
            _buffsList = FindObjectOfType<UI.BuffList>();
            FindObjectOfType<UI.Ammunition>().SetPlayerBuff(this);
        }

        private void FixedUpdate()
        {
            _timer += Time.fixedDeltaTime;

            if (_timer < 1) return;

            for (int i = 0; i < _buffs.Count; i++)
            {
                if (_buffs[i].Timer == -1) continue;

                _buffs[i].Timer -= _timer;

                if (_buffs[i].Timer > 0) continue;

                _buffs.RemoveAt(i);
                _buffsList.RemoveBuff(i);
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

            foreach (BuffRecord buffClass in _buffs)
            {
                if (buffClass.Buff.Name != buff.Name) continue;

                buffClass.Timer = buff.Length;
                return;
            }

            _buffs.Add(new BuffRecord(buff));
            _buffsList.AddBuff(_buffs[^1].Buff.Sprite);

            switch (_buffs[^1].Buff.Type)
            {
                case Info.BuffType.Armor:
                    PlayerArmor.ArmorBuffModifier += _buffs[^1].Buff.Modifier;
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

                _buffs.RemoveAt(i);
                _buffsList.RemoveBuff(i);
                return;
            }
        }
    }
}
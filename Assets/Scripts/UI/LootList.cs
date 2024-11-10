using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class LootList : MonoBehaviour
    {
        record LootLabel
        {
            public LootLabel(RectTransform rectTransform)
            {
                RectTransform = rectTransform;
                GameObject = RectTransform.gameObject;
            }

            public GameObject GameObject;
            public RectTransform RectTransform;
            public float Timer = 0f;
        }

        private List<LootLabel> _lootLabels = new();

        [SerializeField] private RectTransform _lootLabel;
        private Vector2 _labelPosition;

        private void Update()
        {
            for (int i = 0; i < _lootLabels.Count; i++)
            {
                _lootLabels[i].Timer += Time.deltaTime;

                _labelPosition.y = Mathf.SmoothStep(_lootLabels[i].RectTransform.anchoredPosition.y, 80f * i, Time.deltaTime * 25f);
                _lootLabels[i].RectTransform.anchoredPosition = _labelPosition;

                if (_lootLabels[i].Timer > 2f)
                {
                    Destroy(_lootLabels[i].GameObject);
                    _lootLabels.RemoveAt(i);
                }
            }
        }

        public void AddLootLabel(Info.Item info, int count)
        {
            _lootLabels.Add(new(Instantiate(_lootLabel, transform.position, Quaternion.identity, transform)));

            _lootLabels[_lootLabels.Count - 1].GameObject.transform.GetChild(1).GetComponent<Image>().sprite = info.Sprite;
            _lootLabels[_lootLabels.Count - 1].GameObject.GetComponentInChildren<Text>().text = info.Name + (count != 1 ? $" ({count})" : "");
        }
    }
}

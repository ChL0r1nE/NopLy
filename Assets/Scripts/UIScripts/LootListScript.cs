using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LootListScript : MonoBehaviour
{
    record LootLabel
    {
        public LootLabel(GameObject gameObject)
        {
            GameObject = gameObject;
            RectTransform = GameObject.GetComponent<RectTransform>();
        }

        public GameObject GameObject;
        public RectTransform RectTransform;
        public float Timer = 0;
    }

    private List<LootLabel> _lootLabels = new();

    [SerializeField] private GameObject _lootLabel;
    private Vector2 _labelPosition;

    private void Update()
    {
        for (int i = 0; i < _lootLabels.Count; i++)
        {
            _lootLabels[i].Timer += Time.deltaTime;

            _labelPosition.y = Mathf.SmoothStep(_lootLabels[i].RectTransform.anchoredPosition.y, 80 * i, Time.deltaTime * 25);
            _lootLabels[i].RectTransform.anchoredPosition = _labelPosition;

            if (_lootLabels[i].Timer > 2f)
            {
                Destroy(_lootLabels[i].GameObject);
                _lootLabels.RemoveAt(i);
            }
        }
    }

    public void AddLootLabel(ItemInfo info, int count)
    {
        _lootLabels.Add(new LootLabel(Instantiate(_lootLabel, transform.position, Quaternion.identity, transform)));

        _lootLabels[_lootLabels.Count - 1].GameObject.transform.GetChild(1).GetComponent<Image>().sprite = info.Sprite;
        _lootLabels[_lootLabels.Count - 1].GameObject.GetComponentInChildren<Text>().text = info.Name + (count != 1 ? " x " + count.ToString() : "");
    }
}

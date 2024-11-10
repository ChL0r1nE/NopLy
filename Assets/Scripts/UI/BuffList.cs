using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class BuffList : MonoBehaviour
    {
        [SerializeField] private RectTransform _buffImagePrefab;
        private List<RectTransform> _buffImages = new();
        private Vector2 _offset = new(45, 0);
        private int _buffCount = 0;

        public void AddBuff(Sprite sprite)
        {
            _buffImages.Add(Instantiate(_buffImagePrefab, transform));
            _buffImages[^1].anchoredPosition = _offset * _buffCount++;
            _buffImages[^1].GetComponent<Image>().sprite = sprite;
        }

        public void RemoveBuff(int number)
        {
            Destroy(_buffImages[number].gameObject);
            _buffImages.RemoveAt(number);
            _buffCount--;

            for(int i = Mathf.Clamp(--number, 0, int.MaxValue); i < _buffImages.Count; i++)
                _buffImages[i].anchoredPosition = _offset * i;
        }
    }
}

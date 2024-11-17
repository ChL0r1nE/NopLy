using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class BuffList : MonoBehaviour
    {
        private List<RectTransform> _buffImages = new();

        [SerializeField] private RectTransform _buffImagePrefab;
        private readonly Vector2 _offset = new(45, 0);
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

            for(int i = number == 0 ? 0 : --number ; i < _buffImages.Count; i++)
                _buffImages[i].anchoredPosition = _offset * i;
        }
    }
}

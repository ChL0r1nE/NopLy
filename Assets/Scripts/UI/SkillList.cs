using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class SkillList : MonoBehaviour
    {
        [SerializeField] private Image[] _imagesBack;
        [SerializeField] private Image[] _imagesForward;

        private Vector2 _imagePosition;
        private float _moveTimer;
        private int _skillCount;

        private void Update()
        {
            if (_moveTimer > .34f) return;

            _moveTimer += Time.deltaTime;

            for (int i = 0; i < _skillCount; i++)
            {
                _imagePosition.y = Mathf.Lerp(0, 110 + 100 * i, _moveTimer * 3);
                _imagesBack[i].rectTransform.anchoredPosition = _imagePosition;
            }
        }

        public void SetSkills(int count, Sprite[] sprites)
        {
            _moveTimer = 0f;
            _skillCount = count;

            for (int i = 0; i < _imagesBack.Length; i++)
            {
                _imagesBack[i].gameObject.SetActive(i < count);

                if (i >= count) continue;

                _imagesForward[i].sprite = sprites[i];
                _imagesBack[i].sprite = sprites[i];
            }
        }

        public void UpdateReload(float[] timers)
        {
            for (int i = 0; i < _skillCount; i++)
                _imagesForward[i].fillAmount = timers[i];
        }
    }
}

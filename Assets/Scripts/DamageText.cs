using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Billboard _billboard;
    private Color _textColor = new(255f, 69f, 69f, 1f);
    private Vector3 _position = new(0f, 2.5f, 0f);
    private int _damageStack;
    private float _textDelay;
    private bool _isWork = false;

    private void Update()
    {
        if (!_isWork) return;

        _textDelay += Time.deltaTime;

        if (_textDelay > 1f)
        {
            _position.y = Mathf.Lerp(2.5f, 4f, _textDelay - 1f);
            _rectTransform.anchoredPosition = _position;

            _textColor.a = Mathf.Lerp(_textColor.a, 0f, _textDelay - 1f);
            _text.color = _textColor;

            if (_textColor.a == 0f)
            {
                _damageStack = 0;
                _isWork = false;
                _billboard.EnableFollowing = false;
            }
        }
    }

    public void ResetTextDelay(int damage)
    {
        _isWork = true;
        _billboard.EnableFollowing = true;

        _damageStack += damage;
        _text.text = $"- {_damageStack}";
        _textDelay = 0f;

        _position.y = 2.5f;
        _rectTransform.anchoredPosition = _position;

        _textColor.a = 1f;
        _text.color = _textColor;
    }
}

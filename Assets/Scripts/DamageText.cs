using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Billboard _billboard;
    [SerializeField] private float _defaultY;
    private Color _textColor = new(255f, 69f, 69f, 1f);
    private Vector3 _position;
    private int _damageStack;
    private float _textDelay;
    private bool _isWork = false;

    private void Update()
    {
        if (!_isWork) return;

        _textDelay += Time.deltaTime;

        if (_textDelay >= 2f)
        {
            _damageStack = 0;
            _isWork = false;
            _billboard.EnableFollowing = false;
        }
        else if (_textDelay > 1f)
        {
            _position.y = Mathf.Lerp(_defaultY, _defaultY + 1.5f, _textDelay - 1f);
            _rectTransform.anchoredPosition = _position;

            _textColor.a = Mathf.Lerp(_textColor.a, 0f, _textDelay - 1f);
            _text.color = _textColor;
        }
    }

    public void ResetTextDelay(int damage)
    {
        _textDelay = 0f;
        _isWork = true;
        _billboard.EnableFollowing = true;

        _damageStack += damage;
        _text.text = $"- {_damageStack}";

        _position.y = _defaultY;
        _rectTransform.anchoredPosition = _position;

        _textColor.a = 1f;
        _text.color = _textColor;
    }
}

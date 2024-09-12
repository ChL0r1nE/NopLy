using UnityEngine;
using TMPro;

public class DamageTextScript : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private RectTransform _rectTransform;
    private Color _textColor = new(255, 69, 69, 1);
    private Vector3 _position = new(0, 2.5f, 0);
    private int _damageStack;
    private float _textDelay;
    private bool _isWork = false;

    private void Update()
    {
        if (!_isWork) return;

        _textDelay += Time.deltaTime;

        if (_textDelay > 1)
        {
            _position.y = Mathf.Lerp(2.5f, 4f, _textDelay - 1);
            _rectTransform.anchoredPosition = _position;

            _textColor.a = Mathf.Lerp(_textColor.a, 0, _textDelay - 1f);
            _text.color = _textColor;

            if (_textColor.a == 0)
            {
                _damageStack = 0;
                _isWork = false;
            }
        }
    }

    public void ResetTextDelay(int damage)
    {
        _isWork = true;

        _damageStack += damage;
        _text.text = "-" + _damageStack.ToString();
        _textDelay = 0;

        _position.y = 2.5f;
        _rectTransform.anchoredPosition = _position;

        _textColor.a = 1;
        _text.color = _textColor;
    }
}

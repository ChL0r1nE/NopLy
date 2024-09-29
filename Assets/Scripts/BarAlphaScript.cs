using UnityEngine;

public class BarAlphaScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _targetSpriteRenderer;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Color _color = new(255, 255, 255, 0);

    void Update()
    {
        if (_targetSpriteRenderer.color.a == _spriteRenderer.color.a) return;

        _color.a = _targetSpriteRenderer.color.a;
        _spriteRenderer.color = _color;
    }
}

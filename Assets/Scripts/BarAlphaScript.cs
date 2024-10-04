using UnityEngine;

public class BarAlphaScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _targetSpriteRenderer;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    void Update()
    {
        if (_targetSpriteRenderer.color.a != _spriteRenderer.color.a)
            _spriteRenderer.color = _targetSpriteRenderer.color;
    }
}

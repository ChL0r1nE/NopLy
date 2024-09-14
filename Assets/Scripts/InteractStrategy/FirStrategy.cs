using UnityEngine;

public class FirStrategy : Strategy
{
    [SerializeField] private SpriteRenderer _renderer;
    private int _health = 3;

    public override void Interact()
    {
        _renderer.size = new Vector2(--_health * 0.09f, 0.04f);
    }
}

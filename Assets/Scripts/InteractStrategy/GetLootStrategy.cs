using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TargetActivateScript))]
public class GetLootStrategy : Strategy
{
    [SerializeField] private GameObject Loot;
    [SerializeField] private SpriteRenderer _barRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private TargetActivateScript _targetActivateScript;
    [SerializeField] private int _health = 1;
    [SerializeField] private bool _destroyObjectAfter;
    private Vector2 _barScale = new(0, 0.04f);
    private bool _isDestroy = false;

    public override void Interact()
    {
        if (_isDestroy) return;

        _health--;

        if (_barRenderer)
        {
            _barScale.x = _health * 0.09f;
            _barRenderer.size = _barScale;
        }

        if (_health > 0) return;

        _isDestroy = true;
        _animator.SetTrigger("Destroy");
        _targetActivateScript.SetOffActive();

        Instantiate(Loot, transform.position, Quaternion.identity);

        if (_destroyObjectAfter)
            Destroy(gameObject, 2f);
    }
}

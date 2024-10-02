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

    public override void Interact()
    {
        _health--;

        if (_health < 0) return;

        if (_barRenderer)
        {
            _barScale.x = _health * 0.09f;
            _barRenderer.size = _barScale;
        }

        if (_health > 0) return;

        _animator.SetTrigger("Collect");
        _targetActivateScript.SetOffActive();

        Instantiate(Loot, transform.position, Quaternion.identity);

        if (_destroyObjectAfter)
            Destroy(gameObject, 2f);
    }
}

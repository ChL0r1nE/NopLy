using UnityEngine;

[RequireComponent(typeof(TargetActivateScript))]
public class GetLootStrategy : Strategy
{
    public GameObject Loot;
    public GameObject Billboard;

    [SerializeField] private TargetActivateScript _targetActivateScript;
    [SerializeField] private bool _destroyObjectAfter;
    private Animator _animator;
    private bool _isDestroy = false;

    private void Start()
    {
        _targetActivateScript = GetComponent<TargetActivateScript>();
        _animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        if (_isDestroy) return;

        _isDestroy = true;
        _targetActivateScript.SetCanBeActive(false);
        _animator.SetTrigger("Collect");

        Instantiate(Loot, transform.position, Quaternion.identity);

        if (_destroyObjectAfter)
            Destroy(gameObject, 2f);
    }
}

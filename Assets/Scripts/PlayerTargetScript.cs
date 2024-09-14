using UnityEngine;

public class PlayerTargetScript : MonoBehaviour
{
    private TargetActivateScript _target;
    private Strategy _targetStrategy;

    private void OnTriggerEnter(Collider col)
    {
        if (_target)
            _target.TargetDisable();

        _target = col.GetComponent<TargetActivateScript>();
        _target.TargetEnable(out _targetStrategy);
    }

    private void OnTriggerExit(Collider col)
    {
        _targetStrategy = null;
        col.GetComponent<TargetActivateScript>().TargetDisable();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _targetStrategy)
            _targetStrategy.Interact();
    }

    public void SetStrategy(Strategy strategy) => _targetStrategy = strategy;
}

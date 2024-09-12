using UnityEngine;

public class PlayerTargetScript : MonoBehaviour
{
    private TargetActivateScript _target;
    private Strategy _targetStrategy;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Interact"))
        {
            if (_target != null)
                _target.TargetDisable();

            _target = col.gameObject.GetComponent<TargetActivateScript>();
            _target.TargetEnable(out _targetStrategy);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Interact"))
        {
            _targetStrategy = null;
            col.gameObject.GetComponent<TargetActivateScript>().TargetDisable();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _targetStrategy != null)
            _targetStrategy.Interact();
    }

    public void SetStrategy(Strategy strategy) => _targetStrategy = strategy;
}

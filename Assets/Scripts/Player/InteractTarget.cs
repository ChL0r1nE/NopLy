using UnityEngine;

namespace PlayerComponent
{
    public class InteractTarget : MonoBehaviour
    {
        private TargetActivate _target;
        private Interact.AbstractInteract _targetStrategy;

        private void OnTriggerEnter(Collider col)
        {
            _target?.SetTragetValues(0f, 0f);

            _target = col.GetComponent<TargetActivate>();
            _target.TargetEnable(out _targetStrategy);
        }

        private void OnTriggerExit(Collider col)
        {
            _targetStrategy = null;
            _target.SetTragetValues(0f, 0f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                _targetStrategy?.Interact();
        }
    }
}

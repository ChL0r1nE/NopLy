using UnityEngine;

namespace PlayerComponent
{
    public class InteractTarget : MonoBehaviour
    {
        private Interact.AbstractInteract _targetStrategy;
        private TargetActivate _target;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                _targetStrategy?.Interact();
        }

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
    }
}

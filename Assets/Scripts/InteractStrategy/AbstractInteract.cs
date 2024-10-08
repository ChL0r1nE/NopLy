using UnityEngine;

namespace Interact
{
    [RequireComponent(typeof(TargetActivate))]
    public abstract class AbstractInteract : MonoBehaviour
    {
        public abstract void Interact();
    }
}
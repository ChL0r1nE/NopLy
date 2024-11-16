using UnityEngine;

namespace Interact
{
    public interface ITalk
    {
        public void Talk();
    }

    [RequireComponent(typeof(TargetActivate))]
    public abstract class AbstractInteract : MonoBehaviour
    {
        public abstract void Interact();
    }
}
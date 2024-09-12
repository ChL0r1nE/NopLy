using UnityEngine;

[RequireComponent(typeof(TargetActivateScript))]
public abstract class Strategy : MonoBehaviour
{
    public virtual void Interact() { }
}
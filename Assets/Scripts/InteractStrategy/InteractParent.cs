using UnityEngine;

[RequireComponent(typeof(TargetActivateScript))]
public abstract class Strategy : MonoBehaviour
{
    public abstract void Interact();
}
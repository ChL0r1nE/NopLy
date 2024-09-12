using UnityEngine;

public class CameraFollowingScript : MonoBehaviour
{
    public Transform PlayerTransform;
    public Vector3 CameraPositionOffset;

    private void Update() => transform.position = PlayerTransform.position + CameraPositionOffset;
}

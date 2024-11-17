using UnityEngine;

public class CameraSphere : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    private Vector3 _lookVector = new();

    private void Update()
    {
        if (!Input.GetMouseButton(0)) return;

        _lookVector.y += Input.GetAxis("Mouse X") * _sensitivity;
        _lookVector.z = Mathf.Clamp(_lookVector.z - Input.GetAxis("Mouse Y") * _sensitivity, 5, 65);

        transform.rotation = Quaternion.Euler(_lookVector);
    }
}
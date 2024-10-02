using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    private Transform _cam;

    private void Start() => _cam = Camera.main.transform;

    private void Update() => transform.LookAt(transform.position + _cam.forward); //Khm
}

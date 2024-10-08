using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool EnableFollowing = false;

    private Transform _cam;

    private void Start()
    {
        _cam = Camera.main.transform;
        transform.LookAt(transform.position + _cam.forward);
    }

    private void Update()
    {
        if (EnableFollowing)
            transform.LookAt(transform.position + _cam.forward);
    }
}

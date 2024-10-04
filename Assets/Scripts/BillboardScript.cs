using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    public void SetFollowing(bool b) => _enableFollowing = b;

    private Transform _cam;
    private bool _enableFollowing = false;

    private void Start()
    {
        _cam = Camera.main.transform;
        transform.LookAt(transform.position + _cam.forward);
    }

    private void Update()
    {
        if (_enableFollowing)
            transform.LookAt(transform.position + _cam.forward);
    }
}

using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private Vector3 CameraPositionOffset;
    private Vector3 _cameraWorldRotation = new(60f, -45f, 0f);
    private Vector3 _toPlayerPosition;
    private Vector3 _toPlayerRotation;
    private float _time;
    private bool _toPlayer = false;

    public void SwitchToPlayer()
    {
        _toPlayer = !_toPlayer;

        if (_toPlayer)
        {
            _time = 0f;

            _toPlayerPosition = PlayerTransform.position + PlayerTransform.forward * 3f + PlayerTransform.up * 1.5f;

            _toPlayerRotation.x = 28f;
            _toPlayerRotation.y = Mathf.Repeat(PlayerTransform.eulerAngles.y - 221f, 359f);
            //_toPlayerRotation.y = PlayerTransform.eulerAngles.y; CanBeFirstPerson
        }
        else
            transform.rotation = Quaternion.Euler(_cameraWorldRotation);
    }

    private void Update()
    {
        if (_toPlayer)
        {
            _time += Time.deltaTime;

            if (_time > 1f) return;

            transform.position = Vector3.Lerp(transform.position, _toPlayerPosition, _time / 3f);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.eulerAngles, _toPlayerRotation, _time / 3f));
        }
        else
            transform.position = PlayerTransform.position + CameraPositionOffset;
    }
}

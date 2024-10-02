using UnityEngine;

public class CameraFollowingScript : MonoBehaviour
{
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private Vector3 CameraPositionOffset;
    private Vector3 _cameraWorldRotation = new(60, -45, 0);
    private Vector3 _toPlayerPosition;
    private Vector3 _toPlayerRotation;
    private float _time;
    private bool _toPlayer = false;

    public void SwitchToPlayer()
    {
        _toPlayer = !_toPlayer;

        if (_toPlayer)
        {
            _time = 0;

            _toPlayerPosition = PlayerTransform.position + PlayerTransform.right * -0.3f + PlayerTransform.forward * 2.2f + PlayerTransform.up * 2.75f;

            _toPlayerRotation.x = 28;
            _toPlayerRotation.y = Mathf.Repeat(PlayerTransform.eulerAngles.y - 221, 359);
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

            transform.position = Vector3.Lerp(transform.position, _toPlayerPosition, _time / 3);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.eulerAngles, _toPlayerRotation, _time / 3));
        }
        else
            transform.position = PlayerTransform.position + CameraPositionOffset;
    }
}

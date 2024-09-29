using UnityEngine;

public class CameraFollowingScript : MonoBehaviour
{
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private Vector3 CameraPositionOffset;
    private Vector3 _toPlayerPosition;
    private Vector3 _toPlayerRotation;
    private bool _toPlayer = false;

    public void SwitchToPlayer()
    {
        _toPlayer = !_toPlayer;

        if (_toPlayer)
        {
            _toPlayerPosition = PlayerTransform.position + PlayerTransform.right * -0.3f + PlayerTransform.forward * 2.2f + PlayerTransform.up * 2.75f;

            _toPlayerRotation.x = 28;
            _toPlayerRotation.y = Mathf.Repeat(PlayerTransform.eulerAngles.y - 221, 359);
        }
        else
            transform.rotation = Quaternion.Euler(new Vector3(60, -45, 0));
    }

    private void Update()
    {
        if (_toPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, _toPlayerPosition, Time.deltaTime * 25);
            transform.rotation = Quaternion.Euler(Vector3.MoveTowards(transform.eulerAngles, _toPlayerRotation, Time.deltaTime * 500));
        }
        else
            transform.position = PlayerTransform.position + CameraPositionOffset;
    }
}

using UnityEngine;

public class BuffZone : MonoBehaviour
{
    [SerializeField] private GameObject _afterLoot;
    [SerializeField] private Material _zoneMaterial;
    [SerializeField] private BuffInfo _buff;
    [SerializeField] private int _workTime;
    private PlayerComponent.Buff _playerBuff;
    private float _timer = 0f;
    private bool _isPlayer;

    private void OnEnable()
    {
        _playerBuff = PlayerComponent.Buff.StaticBuff;
        _zoneMaterial.SetVector("_CirclePosition", new Vector4(transform.position.x, 0f, transform.position.z, 0f));
    }

    private void OnDisable()
    {
        _zoneMaterial.SetVector("_CirclePosition", new Vector4(0f, 100f, 0f, 0f));
        Instantiate(_afterLoot, transform.position, Quaternion.Euler(new Vector3(0f, 0f, 100f)));

        if (_isPlayer)
            _playerBuff.DeleteBuff(_buff);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        _isPlayer = true;
        _playerBuff.AddBuff(_buff);
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        _isPlayer = false;
        _playerBuff.DeleteBuff(_buff);
    }

    private void Update()
    {
        if (_timer > 1f)
            Destroy(gameObject);

        _timer += Time.deltaTime / _workTime;
        _zoneMaterial.SetFloat("_Angle", Mathf.Lerp(0f, 23f, _timer));
    }
}

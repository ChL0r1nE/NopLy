using UnityEngine;

public class ArmorBuffZoneScript : MonoBehaviour
{
    [SerializeField] private Material _zoneMaterial;
    [SerializeField] private Sprite _buffSprite;
    [SerializeField] private int _workTime;
    private PlayerArmorScript _playerArmorScript;
    private BuffListScript _buffListScript;
    private float _timer = 0;
    private bool _isPlayer;

    private void OnEnable()
    {
        _buffListScript = FindObjectOfType<BuffListScript>();
        _playerArmorScript = FindObjectOfType<PlayerArmorScript>();
        _zoneMaterial.SetVector("_CirclePosition", new Vector4(transform.position.x, 0, transform.position.z, 0));
    }

    private void OnDisable()
    {
        _zoneMaterial.SetVector("_CirclePosition", new Vector4(0, 100, 0, 0));

        if (!_isPlayer) return;

        _playerArmorScript.ArmorBuffModifier -= 0.5f;
        _buffListScript.DeleteIcon(_buffSprite);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        _isPlayer = true;
        _playerArmorScript.ArmorBuffModifier += 0.5f;
        _buffListScript.AddIcon(_buffSprite);
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        _isPlayer = false;
        _playerArmorScript.ArmorBuffModifier -= 0.5f;
        _buffListScript.DeleteIcon(_buffSprite);
    }

    private void Update()
    {
        if (_timer > 1)
            Destroy(gameObject);

        _timer += Time.deltaTime / _workTime;
        _zoneMaterial.SetFloat("_Angle", Mathf.Lerp(0, 23, _timer));
    }
}

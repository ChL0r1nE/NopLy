using UnityEngine;

[RequireComponent(typeof(OutlineScript))]
public class TargetActivateScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] BillboardRendereres;

    [SerializeField] private Strategy Strategy;
    private OutlineScript _outlineScript;
    private Color _billboardColor = new(255, 255, 255, 0);
    private float _widthTarget;
    private float _alphaTarget;
    private float _time = 2;

    public void TargetDisable() => TargetEvent(0f, 0f);

    public void SetOffActive()
    {
        TargetEvent(0f, 0f);
        Destroy(GetComponent<BoxCollider>());
    }

    public void TargetEnable(out Strategy strategy)
    {
        strategy = Strategy;
        TargetEvent(5f, 1f);
    }

    private void Start() => _outlineScript = GetComponent<OutlineScript>();

    private void Update()
    {
        if (_time > 1) return;

        _time += Time.deltaTime;

        _outlineScript.OutlineWidth = Mathf.Lerp(_outlineScript.OutlineWidth, _widthTarget, _time);

        _billboardColor.a = Mathf.Lerp(_billboardColor.a, _alphaTarget, _time);

        foreach (SpriteRenderer billboardRenderer in BillboardRendereres)
            billboardRenderer.color = _billboardColor;
    }

    private void TargetEvent(float outlineWidth, float alphaTarget)
    {
        _time = 0;
        _widthTarget = outlineWidth;
        _alphaTarget = alphaTarget;
    }
}

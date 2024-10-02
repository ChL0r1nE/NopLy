using UnityEngine;

[RequireComponent(typeof(OutlineScript))]
public class TargetActivateScript : MonoBehaviour
{
    public SpriteRenderer BillboardRenderer;
    public Strategy Strategy;

    private OutlineScript _outlineScript;
    private Color _billboardColor = new(255, 255, 255, 0);
    private float _alphaTarget;

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
        if (BillboardRenderer.color.a == _alphaTarget) return;

        _billboardColor.a = Mathf.MoveTowards(_billboardColor.a, _alphaTarget, Time.deltaTime * 2);
        BillboardRenderer.color = _billboardColor;
    }

    private void TargetEvent(float outlineWidth, float alphaTarget)
    {
        _outlineScript.OutlineWidth = outlineWidth;
        _alphaTarget = alphaTarget;
    }
}

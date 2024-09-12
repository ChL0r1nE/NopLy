using UnityEngine;

[RequireComponent(typeof(OutlineScript))]
public class TargetActivateScript : MonoBehaviour
{
    public SpriteRenderer BillboardRenderer;
    public Strategy Strategy;

    private OutlineScript _outlineScript;
    private Color _billboardColor = new(255, 255, 255, 0);
    private float _alphaTarget;
    private bool _canBeActive = true;

    public void SetCanBeActive(bool canBeActive)
    {
        _canBeActive = canBeActive;
        _outlineScript.OutlineWidth = 0f;
        _alphaTarget = 0f;
    }

    public void TargetEnable(out Strategy strategy)
    {
        strategy = Strategy;

        if (_canBeActive)
            TargetEvent(5f, 1f);
    }

    public void TargetDisable() => TargetEvent(0f, 0f);

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

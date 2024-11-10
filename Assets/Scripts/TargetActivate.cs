using UnityEngine;

[RequireComponent(typeof(Outline))]
public class TargetActivate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] BillboardRendereres;

    [SerializeField] private Interact.AbstractInteract Strategy;
    private Outline _outline;
    private Color _billboardColor = new(255f, 255f, 255f, 0f);
    private float _widthTarget;
    private float _alphaTarget;
    private float _time = 1f;

    private void Start() => _outline = GetComponent<Outline>();

    private void Update()
    {
        if (_time > 1) return;

        _time += Time.deltaTime;
        _outline.OutlineWidth = Mathf.Lerp(_outline.OutlineWidth, _widthTarget, _time);
        _billboardColor.a = Mathf.Lerp(_billboardColor.a, _alphaTarget, _time);

        foreach (SpriteRenderer billboardRenderer in BillboardRendereres)
            billboardRenderer.color = _billboardColor;
    }

    public void SetOffActive()
    {
        SetTragetValues(0f, 0f);
        Destroy(GetComponent<SphereCollider>());
    }

    public void TargetEnable(out Interact.AbstractInteract strategy)
    {
        SetTragetValues(5f, 1f);
        strategy = Strategy;
    }

    public void SetTragetValues(float outlineWidth, float alphaTarget)
    {
        _time = 0f;
        _widthTarget = outlineWidth;
        _alphaTarget = alphaTarget;
    }
}

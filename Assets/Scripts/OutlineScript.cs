using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class OutlineScript : MonoBehaviour
{
    private readonly static HashSet<Mesh> registeredMeshes = new();

    public enum Mode
    {
        OutlineAll,
        OutlineVisible,
        OutlineHidden,
        OutlineAndSilhouette,
        SilhouetteOnly
    }

    public Mode OutlineMode
    {
        get { return _outlineMode; }
        set
        {
            _outlineMode = value;
            UpdateMaterialProperties();
        }
    }

    public Color OutlineColor
    {
        get { return _outlineColor; }
        set
        {
            _outlineColor = value;
            UpdateMaterialProperties();
        }
    }

    public float OutlineWidth
    {
        get { return _outlineWidth; }
        set
        {
            _outlineWidth = value;
            UpdateMaterialProperties();
        }
    }

    private List<Material> _materials = new();

    [SerializeField] private Renderer[] _renderers;

    [SerializeField] private Color _outlineColor = Color.white;
    [SerializeField] private Mode _outlineMode = Mode.OutlineVisible;
    [SerializeField] private float _outlineWidth = 0f;
    private Renderer _renderer;
    private Material _outlineMaskMaterial;
    private Material _outlineFillMaterial;
    private Vector3 _smoothNormal;

    private void Awake()
    {
        _outlineMaskMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineMask"));
        _outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineFill"));

        _outlineMaskMaterial.name = "OutlineMask (Instance)";
        _outlineFillMaterial.name = "OutlineFill (Instance)";

        LoadSmoothNormals();
        UpdateMaterialProperties();
    }

    private void OnEnable()
    {
        foreach (Renderer renderer in _renderers)
        {
            var materials = renderer.sharedMaterials.ToList();

            materials.Add(_outlineMaskMaterial);
            materials.Add(_outlineFillMaterial);

            renderer.materials = materials.ToArray();
        }
    }

    private void OnDisable()
    {
        foreach (Renderer renderer in _renderers)
        {
            _materials = renderer.sharedMaterials.ToList();

            _materials.Remove(_outlineMaskMaterial);
            _materials.Remove(_outlineFillMaterial);

            renderer.materials = _materials.ToArray();
        }
    }

    private void OnDestroy()
    {
        Destroy(_outlineMaskMaterial);
        Destroy(_outlineFillMaterial);
    }

    private void LoadSmoothNormals()
    {
        foreach (MeshFilter meshFilter in GetComponentsInChildren<MeshFilter>())
        {
            if (!registeredMeshes.Add(meshFilter.sharedMesh))
                continue;

            List<Vector3> smoothNormals = SmoothNormals(meshFilter.sharedMesh);

            meshFilter.sharedMesh.SetUVs(3, smoothNormals);

            _renderer = meshFilter.GetComponent<Renderer>();

            if (_renderer)
                CombineSubmeshes(meshFilter.sharedMesh, _renderer.sharedMaterials.Length);
        }

        foreach (SkinnedMeshRenderer skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (!registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
                continue;

            skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
            CombineSubmeshes(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials.Length);
        }
    }

    private List<Vector3> SmoothNormals(Mesh mesh)
    {
        var groups = mesh.vertices.Select((vertex, index) =>
            new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

        List<Vector3> smoothNormals = new(mesh.normals);

        foreach (var group in groups)
        {
            if (group.Count() == 1)
                continue;

            _smoothNormal = Vector3.zero;

            foreach (var pair in group)
                _smoothNormal += smoothNormals[pair.Value];

            _smoothNormal.Normalize();

            foreach (var pair in group)
                smoothNormals[pair.Value] = _smoothNormal;
        }

        return smoothNormals;
    }

    private void CombineSubmeshes(Mesh mesh, int lenght)
    {
        if (mesh.subMeshCount == 1 || mesh.subMeshCount > lenght) return;

        mesh.subMeshCount++;
        mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
    }

    private void UpdateMaterialProperties()
    {
        _outlineFillMaterial.SetColor("_OutlineColor", _outlineColor);

        _outlineFillMaterial.SetFloat
            ("_OutlineWidth", _outlineMode == Mode.SilhouetteOnly ? 0 : _outlineWidth);

        switch (_outlineMode)
        {
            case Mode.OutlineAll:
                _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                break;

            case Mode.OutlineVisible:
                _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                break;

            case Mode.OutlineHidden:
                _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                break;

            case Mode.OutlineAndSilhouette:
                _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                break;

            case Mode.SilhouetteOnly:
                _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                break;
        }
    }
}

using UnityEngine;

public class UpdateMeshCollider : MonoBehaviour
{
    MeshCollider meshCollider;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh bakedMesh;

    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        bakedMesh = new Mesh();
    }

    void Update()
    {
        skinnedMeshRenderer.BakeMesh(bakedMesh);
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = bakedMesh;
    }

}

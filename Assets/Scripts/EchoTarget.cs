using UnityEngine;

public class EchoTarget : MonoBehaviour
{
    public int pointsPerMesh = 400;

    public Color echoColor = new Color(0.1f, 0.8f, 1f, 1f);
    public float pointSize = 0.04f;
    public ParticleSystem echoParticles;
    public void CreateEchoSnapshot()
    {
        if (echoParticles == null)
        {
            Debug.LogError("Echo particles not assigned.", this);
            return;
        }
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter filter in meshFilters)
        {
            // Process each mesh filter to create echo points
            if (filter.sharedMesh == null)
                continue;

            SampleMesh(filter.sharedMesh, filter.transform);
        }

        SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
        {
            Mesh bakedMesh = new Mesh();

            //Capture the current movement.
            renderer.BakeMesh(bakedMesh);

            SampleMesh(bakedMesh, renderer.transform);

            Destroy(bakedMesh);

        }
    }

    void SampleMesh(Mesh mesh, Transform meshTransform)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;// Get the triangles of the mesh, can be changed to quads if needed.

        if (triangles.Length < 3) return;
        int triangleCount = triangles.Length / 3;

        for (int i = 0; i < pointsPerMesh; i++)
        {
            // Process each point in the mesh to create echo points
            int triangleIndex = Random.Range(0, triangleCount) * 3;
            Vector3 a = vertices[triangles[triangleIndex]];
            Vector3 b = vertices[triangles[triangleIndex + 1]];
            Vector3 c = vertices[triangles[triangleIndex + 2]];

            // Generate a random point inside the triangle using barycentric coordinates
            float r1 = Mathf.Sqrt(Random.value);
            float r2 = Random.value;

            Vector3 localPoint = (1 - r1) * a + (r1 * (1 - r2)) * b + (r1 * r2) * c;

            // Freeze the point in world space
            Vector3 worldPoint = meshTransform.TransformPoint(localPoint);
            ParticleSystem.EmitParams emit = new ParticleSystem.EmitParams();

            emit.position = worldPoint;
            emit.startColor = echoColor;
            emit.startSize = pointSize;
            echoParticles.Emit(emit, 1);
        }
    }
}

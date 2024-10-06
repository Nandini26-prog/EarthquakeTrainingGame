using UnityEngine;

public class WallDeformation : MonoBehaviour
{
    public GameObject[] walls; // Array to hold multiple wall objects
    public float forceAmount = 10f; // Amount of force applied for deformation
    public float deformationRadius = 0.5f; // Radius around the point of deformation

    void Start()
    {
        // Optionally, initialize any specific setup needed for multiple walls
    }

    public void ApplyForce(Vector3 pointOfImpact)
    {
        foreach (GameObject wall in walls)
        {
            MeshFilter wallMeshFilter = wall.GetComponent<MeshFilter>();
            if (wallMeshFilter != null)
            {
                Mesh wallMesh = wallMeshFilter.mesh;
                Vector3[] vertices = wallMesh.vertices;

                // Loop through all the vertices of the wall
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 worldPos = wall.transform.TransformPoint(vertices[i]);

                    // Check if the vertex is within the radius of the impact point
                    if (Vector3.Distance(worldPos, pointOfImpact) < deformationRadius)
                    {
                        // Calculate the force direction and apply it to the vertex
                        Vector3 forceDirection = (worldPos - pointOfImpact).normalized;
                        vertices[i] += forceDirection * forceAmount * Time.deltaTime;
                    }
                }

                // Update the mesh with the deformed vertices
                wallMesh.vertices = vertices;
                wallMesh.RecalculateNormals();
                wallMeshFilter.mesh = wallMesh;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Trigger deformation when the wall is hit
        ApplyForce(collision.contacts[0].point);
    }
}

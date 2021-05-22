using UnityEngine;
public class PrismGenerator : MonoBehaviour
{

    public GameObject myPrefab;
    public Material material;
    //public int limit=3;
    
    [Header("Size")]
    public float radius = 0.2f;
    public float height = 0.8f;
    public float nextHeightDif = 0.8f;
    private MeshCollider meshCollider;
   // private GameObject[] prisms;
    //vertice gibt die Anzahl der Ecken der Polygone der Grundflächen des Zylinders an.
    public int verticeCount = 7;

    void Start()
    {
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[verticeCount*2];
        for(int i = 0; i<verticeCount; i++)
        {
            vertices[i] = new Vector3(Mathf.Sin(2 * Mathf.PI/(verticeCount) * i) * radius, -height/2, Mathf.Cos(2 * Mathf.PI/(verticeCount) * i) * radius);
            vertices[i+ (verticeCount)] = new Vector3(Mathf.Sin(2*Mathf.PI/(verticeCount) * i) * radius, height/2,Mathf.Cos(2 * Mathf.PI/(verticeCount) * i) * radius);
        }
        mesh.vertices = vertices;
        int[] triangles = new int[(((verticeCount-1)*4)*3)];
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < verticeCount - 2; j++)
            {
                triangles[j*3 + (verticeCount -2) *3 * i] = (verticeCount) * i;
                triangles[j*3+1 + (verticeCount -2) * i * 3] = (verticeCount - 1) * i +j+2;
                triangles[j*3+2 + (verticeCount -2) * i * 3] = (verticeCount + 1) * i +j+1;
            }
        }
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < verticeCount; j++)
            {
                triangles[3 * (j + (i+2) * verticeCount)-12] = j;
                triangles[3 * (j + (i + 2) * verticeCount)-11] = (j + 1) % verticeCount + i * verticeCount;
                triangles[3 * (j + (i + 2) * verticeCount)-10] = verticeCount + (j + 1 - i) % verticeCount;
            }
        }
        mesh.triangles = triangles;
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
        GetComponent<MeshRenderer>().material = material;
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
using UnityEngine;
public class MeshGenerator: MonoBehaviour
{

    //public GameObject myPrefab;
    public Material material;
    //public int limit=3;
    [Header("Size")]
    public float width = 1f;
    public float height = 1f;
    public float length = 1f;
    public float nextHeightDif = 0.4f;
   // public float scaling = 0.3f;

    private MeshCollider meshCollider;
    //private GameObject[] wedges;
    //private GameObject[] cylinders;

    void Start()
    {
        //width *= scaling;
        //height *= scaling;
        //length *= scaling;
        //MeshCollider meshCollider = new MeshCollider();
        //wedges = new GameObject[limit];
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[6];
        vertices[0] = new Vector3(-width/2, -height/2, -length/2);
        vertices[1] = new Vector3(width/2, -height / 2, -length / 2);
        vertices[2] = new Vector3(width/2, -height / 2, length/2);
        vertices[3] = new Vector3(-width/2, -height/2, length/2);
        vertices[4] = new Vector3(width/2, height/2, -length/2);
        vertices[5] = new Vector3(width/2, height/2, length/2);
        mesh.vertices = vertices;
        mesh.triangles = new int[]{0, 1, 2, 0, 2, 3, 0, 3, 5, 0, 5, 4, 4, 1, 0, 3, 2, 5, 1, 5, 2, 1, 4, 5};
        //meshCollider.sharedMesh.SetVertices(vertices);
        //meshCollider.sharedMesh = mesh;
        //meshCollider.inflateMesh = true;
        //meshCollider.convex = true;

        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
        GetComponent<MeshRenderer>().material = material;
        GetComponent<MeshFilter>().mesh = mesh;
    }
/*    public void generateWedge()
    {
        for (int i=0; i<=limit-1; i++)
        if(wedges[i] == null)
        {
                wedges[i] = Instantiate(myPrefab, new Vector3(2.06f, 0.01f + (nextHeightDif * i), (1f)), Quaternion.identity);
                break;
        }
    }*/
}
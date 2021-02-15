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
        Debug.Log("PrismGenerator Start");
        //MeshCollider meshCollider = new MeshCollider();
        //prisms = new GameObject[limit];
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[verticeCount*2];
        for(int i = 0; i<verticeCount; i++)
        {
            //Debug.Log(i);
            vertices[i] = new Vector3(Mathf.Sin(2 * Mathf.PI/(verticeCount) * i) * radius, -height/2, Mathf.Cos(2 * Mathf.PI/(verticeCount) * i) * radius);
            //Debug.Log(vertices[i]);
            //Debug.Log(i);
            vertices[i+ (verticeCount)] = new Vector3(Mathf.Sin(2*Mathf.PI/(verticeCount) * i) * radius, height/2,Mathf.Cos(2 * Mathf.PI/(verticeCount) * i) * radius);
           // Debug.Log(vertices[i]);
        }
        //Debug
       /* for(int i = 0; i< vertices.Length; i++)
        {
            Debug.Log(vertices[i]);
        }*/
        mesh.vertices = vertices;
        //Debug.Log("Vertices created.");

        int[] triangles = new int[(((verticeCount-1)*4)*3)];
        //Debug.Log("Creating triangles");
        //string triangleDebug = "Testing triangles: ";
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < verticeCount - 2; j++)
            {
                triangles[j*3 + (verticeCount -2) *3 * i] = (verticeCount) * i;
                //triangleDebug += "Adding at Index "+ (j * 3 + (verticeCount - 2) * 3 * i)+" vertice with number "+((verticeCount) * i)+".\n";
                triangles[j*3+1 + (verticeCount -2) * i * 3] = (verticeCount - 1) * i +j+2;
                //triangleDebug += "Adding at Index " + (j * 3 + 1 + (verticeCount - 2) * i * 3) + " vertice with number " + ((verticeCount - 1) * i + j + 2) + ".\n";
                triangles[j*3+2 + (verticeCount -2) * i * 3] = (verticeCount + 1) * i +j+1;
                //triangleDebug += "Adding at Index " + (j * 3 + 2 + (verticeCount - 2) * i * 3) + " vertice with number " + ((verticeCount + 1) * i + j + 1) + ".\n";
            }
        }
        /*/Debug
        Debug.Log(triangleDebug);
        triangleDebug = "";
        for (int i = 0; i < triangles.Length; i++)
        {
            triangleDebug += (triangles[i] + ", ");
            if (((i+1) % 3) == 0)
            {
                triangleDebug += "\n";
            }
        }
        Debug.Log(triangleDebug);

        //end Debug 
        triangleDebug = "";*/
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < verticeCount; j++)
            {
                triangles[3 * (j + (i+2) * verticeCount)-12] = j;
                //triangleDebug += "Adding at Index " + (3*(j + (i + 2) * verticeCount) - 12) + " vertice with number " + j + ".\n";
                triangles[3 * (j + (i + 2) * verticeCount)-11] = (j + 1) % verticeCount + i * verticeCount;
                //triangleDebug += "Adding at Index " + (3*(j + (i + 2) * verticeCount) - 11) + " vertice with number " + ((j + 1) % verticeCount + i * verticeCount) + ".\n";
                triangles[3 * (j + (i + 2) * verticeCount)-10] = verticeCount + (j + 1 - i) % verticeCount;
                //triangleDebug += "Adding at Index " + (3 * (j + (i + 2) * verticeCount) - 10) + " vertice with number " + (verticeCount + (j + 1 - i) % verticeCount) + ".\n";
            }
        }
        //Debug.Log(triangleDebug);
        /*triangleDebug = "";
        for(int i = 0; i < triangles.Length; i++)
        {
            triangleDebug += (triangles[i]+", ");
            if (((i+1) % 3) == 0)
            {
                triangleDebug += "\n";
            }
        }
        Debug.Log(triangleDebug);*/

        /*for(int i=0;i< triangles.Length; i=i+9)
        {
            for(int j = 0; k < 2;k++)
            {
                for (int k = 0; k < (vertice - 4) * 3; k++)
                {
                    triangles[i + j + k] = k;
                }
            }

            triangles[i] = i;
            triangles[i + 1] = i+1;
            triangles[i + 2] = i + 2;
            triangles[i + 3] = (vertice / 2);
            triangles[i + 5] = i+ (vertice / 2);
            triangles[i + 4] = i + (vertice / 2)+1;
            triangles[i + 6] = i;
            triangles[i + 7] = i+ (vertice / 2);
            triangles[i + 8] = i+1;
            triangles[i + 9] = i;
            triangles[i + 11] = i+ (vertice / 2);
            triangles[i + 10] = i+ (vertice / 2)+1;
        }*/
        mesh.triangles = triangles;
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;



        /*
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(width, 0, 0);
        vertices[2] = new Vector3(width, 0, length);
        vertices[3] = new Vector3(0, 0, length);
        vertices[4] = new Vector3(width, height, 0);
        vertices[5] = new Vector3(width, height, length);
        mesh.vertices = vertices;
        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 5, 0, 5, 4, 4, 1, 0, 3, 2, 5, 1, 5, 2, 1, 4, 5 };
        //meshCollider.sharedMesh.SetVertices(vertices);
        meshCollider.sharedMesh = mesh;
        //meshCollider.inflateMesh = true;
        meshCollider.convex = true;*/


        GetComponent<MeshRenderer>().material = material;
        GetComponent<MeshFilter>().mesh = mesh;
        //Debug.Log("Prefab is ready");
    }
   /* public void generatePrism()
    {
        for (int i = 0; i <= limit - 1; i++)
            if (prisms[i] == null)
            {
                prisms[i] = Instantiate(myPrefab, new Vector3(2.06f, 0.01f + (nextHeightDif * i), (1f)), Quaternion.identity);
                break;
            }
    }*/
}
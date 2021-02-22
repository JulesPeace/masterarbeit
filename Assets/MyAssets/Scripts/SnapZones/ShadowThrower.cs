using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShadowThrower : MonoBehaviour
{
    public GameObject vorderansicht_plane;
    public GameObject seitenansicht_plane;
    public GameObject aufsicht_plane;
    public GameObject shadowProjection;
    private GameObject projectionAufsicht;
    private GameObject projectionSeitenansicht;
    private GameObject projectionVorderansicht;
    public Material correctColor;
    public Material incorrectColor;
    public GameObject TestProjectionRemoveMeLater;
    private static Vector3[] RectangleVertices = new Vector3[] { new Vector3(-0.5f, 0f, -0.5f), new Vector3(0.5f, 0f, -0.5f), new Vector3(0.5f, 0f, 0.5f), new Vector3(-0.5f, 0f, 0.5f) };

    // Start is called before the first frame update
    void Start()
    {
 /*       try
        {
            Debug.Log("Projektion von " + this.name + " beginnt.");
            project(TestProjectionRemoveMeLater);
            Debug.Log("Projektion von " + this.name + " beendet.");
        }
        catch(Exception e)
        {
            Debug.Log("TestProjectionRemoveMeLater von "+this.name+" ist leer! msg=" + e.Message);
        }*/
    }

    public void project(GameObject interactable)
    {
        try
        {
            QuestDebugLogic.instance.log(interactable.tag);
        }
        catch
        {
            QuestDebugLogic.instance.log("Keine Übergabe für project()");
        }
        projectAufsicht(interactable);
        projectSeitenansicht(interactable);
        projectVorderansicht(interactable);
    }

    void projectAufsicht(GameObject interactable)
    {
        this.projectionAufsicht = Instantiate(shadowProjection, aufsicht_plane.transform, false);
        this.projectionAufsicht.transform.localPosition = new Vector3(this.transform.localPosition.x * 2 - 4f, 0.025f, this.transform.localPosition.z * 2 - 4f);
        this.projectionAufsicht.transform.localScale = new Vector3(2, 2, 2);
        this.projectionAufsicht.GetComponent<MeshRenderer>().material = correctColor;
        Mesh mesh = new Mesh();
        try
        {
            //Debug.Log("Anfang des try-Blocks");
            //Debug.Log("Interactable ist " + interactable.name);
            if (interactable.name.Contains("Cube"))
            {
                //QuestDebugLogic.instance.log("Cube snap erkannt");
                rectangleMesh(mesh);
                //mesh.triangles = new int[] { 0, 3, 2, 0, 2, 1 };
            }
            else
            if (interactable.name.Contains("Wedge"))
            {
                //Debug.Log("Wedge snap erkannt: Bereite Ecken vor:");
                //QuestDebugLogic.instance.log("Wedge snap erkannt");
                //Debug.Log("Wedge snap erkannt");
                //TODO Komplizierte interactable.transform.rotation analyse
                string msg = "";
                int x = (int)interactable.transform.rotation.eulerAngles.x;
                //msg += "x=" + x;
                int y = (int)interactable.transform.rotation.eulerAngles.y;
                //msg += ", y=" + y;
                int z = (int)interactable.transform.rotation.eulerAngles.z;
                //msg += ", z=" + z;
                //msg += interactable.transform.rotation.eulerAngles.ToString();
                //QuestDebugLogic.instance.log(msg);
                //mesh.triangles = new int[] { 0, 1, 2 };
                if (x == 90)
                {
                    while (y != 0)
                    {
                        y += 90;
                        z += 90;
                        y %= 360;
                        z %= 360;
                        //Debug.Log("x="+x+", y="+y+",z="+z+".");
                    }
                    triangleMesh(mesh, (z + 90) % 360);
                }
                    /*if (x == 90)
                    {
                        int d = z - y;
                        if (d == 0)
                        {
                            mesh.vertices = triangleVertices(0);
                        } else
                        if (d == 270 || d==-90)
                        {
                            mesh.vertices = triangleVertices(90);
                        } else
                        if (d == 180||d==-180)
                        {
                            mesh.vertices = triangleVertices(180);
                        } else
                        if (d == -270||d==90)
                        {
                            mesh.vertices = triangleVertices(270);
                        }
                    }*/
                else if (x == 270)
                {
                    while (y != 0)
                    {
                        y -= 90;
                        z += 90;
                        z %= 360;
                    }
                    triangleMesh(mesh, (-z + 180 + 360) % 360);                                                                                                                                                                         
                }
                else
                {
                    rectangleMesh(mesh);
                    //mesh.triangles = new int[] { 0, 3, 2, 0, 2, 1 };
                }
            }
            else
            if (interactable.name.Contains("Prism"))
            {
                QuestDebugLogic.instance.log("Prism snap erkannt");
                mesh.vertices = circleVertices(0.5f);
                int[] triangles = new int[54];
                for (int i = 0; i < 18; i++)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = 18 - i;
                    triangles[i * 3 + 2] = 19 - i;
                }
                mesh.triangles = triangles;
            }
            else
            {
                QuestDebugLogic.instance.log("Weder Cube, noch Prism oder Wedge erkannt!");
                Debug.Log("Weder Cube, noch Prism oder Wedge erkannt!");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message + "projection() ohne interactable");
            QuestDebugLogic.instance.log(e.Message+ ":projection() ohne interactable");
        }
        this.projectionAufsicht.GetComponent<MeshFilter>().mesh = mesh;
    }

    void projectSeitenansicht(GameObject interactable)
    {
        this.projectionSeitenansicht = Instantiate(shadowProjection, seitenansicht_plane.transform, false);
        this.projectionSeitenansicht.transform.localPosition = new Vector3(this.transform.localPosition.y * 2 -4f, 0.025f, -this.transform.localPosition.x * 2 + 4f);
        this.projectionSeitenansicht.transform.localScale = new Vector3(2, 2, 2);
        this.projectionSeitenansicht.GetComponent<MeshRenderer>().material = correctColor;
        Mesh mesh = new Mesh();
        try
        {
            if (interactable.name.Contains("Cube"))
            {
                rectangleMesh(mesh);
                //mesh.triangles = new int[] { 0, 3, 2, 0, 2, 1 };
            }
            else
            if (interactable.name.Contains("Wedge"))
            {
                //TODO Komplizierte interactable.transform.rotation analyse
                int x = (int)interactable.transform.rotation.eulerAngles.x;
                int y = (int)interactable.transform.rotation.eulerAngles.y;
                int z = (int)interactable.transform.rotation.eulerAngles.z;
                if ((x == 0 || x == 180) && (y == 0 || y == 180))
                {
                    if (y == 180)
                    {
                        x += 180;
                        x %= 360;
                        y += 180;
                        y %= 360;
                        z += 180;
                        z %= 360;
                    }
                    if (x != 0)
                    {
                        z = (-z + 90) % 360;
                        x -= 180;
                    }
                    triangleMesh(mesh, (z+360)%360);
                }
                else
                {
                    rectangleMesh(mesh);
                }

                //mesh.triangles = new int[] { 0, 1, 2 };
            }
            else
            if (interactable.name.Contains("Prism"))
            {
                rectangleMesh(mesh);
            }
            else
            {
                Debug.Log("Weder Cube, noch Prism oder Wedge erkannt!");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log("projectionSeitenansicht() ohne interactable");
            QuestDebugLogic.instance.log("projection() ohne interactable. " +e.Message);
        }
        this.projectionSeitenansicht.GetComponent<MeshFilter>().mesh = mesh;
    }

    void projectVorderansicht(GameObject interactable)
    {
        this.projectionVorderansicht = Instantiate(shadowProjection, vorderansicht_plane.transform, false);
        this.projectionVorderansicht.transform.localPosition = new Vector3(this.transform.localPosition.y * 2 - 4f, 0.025f, this.transform.localPosition.z * 2 - 4f);
        this.projectionVorderansicht.transform.localScale = new Vector3(2, 2, 2);
        this.projectionVorderansicht.GetComponent<MeshRenderer>().material = correctColor;
        Mesh mesh = new Mesh();
        try
        {
            if (interactable.name.Contains("Cube"))
            {
                rectangleMesh(mesh);
                //mesh.triangles = new int[] { 0, 3, 2, 0, 2, 1 };
            }
            else
            if (interactable.name.Contains("Wedge"))
            {
                //TODO Komplizierte interactable.transform.rotation analyse
                int x = (int)interactable.transform.rotation.eulerAngles.x;
                int y = (int)interactable.transform.rotation.eulerAngles.y;
                int z = (int)interactable.transform.rotation.eulerAngles.z;
                if ((x == 0 || x == 180) && (y == 90 || y == 270))
                {
                    if (y == 270)
                    {
                        x += 180;
                        x %= 360;
                        y += 180;
                        y %= 360;
                        z += 180;
                        z %= 360;
                    }
                    if (x != 0)
                    {
                        z = (-z + 90) % 360;
                        x -= 180;
                    }
                    triangleMesh(mesh, (z + 360) % 360);
                }
                else
                {
                    rectangleMesh(mesh);
                }
            }
            else
            if (interactable.name.Contains("Prism"))
            {
                rectangleMesh(mesh);
                mesh.triangles = new int[] { 0, 3, 2, 0, 2, 1 };
            }
            else
            {
                Debug.Log("Weder Cube, noch Prism oder Wedge erkannt!");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log("projectionVorderansicht() ohne interactable");
            QuestDebugLogic.instance.log("projection() ohne interactable. "+e.Message);
        }
        this.projectionVorderansicht.GetComponent<MeshFilter>().mesh = mesh;
    }

    void rectangleMesh(Mesh mesh)
    {
        mesh.vertices = RectangleVertices;
        mesh.triangles = new int[] { 0, 3, 2, 0, 2, 1 };
    }


    void triangleMesh(Mesh changedMesh, int rotationInDegrees)
    {
        Vector3[] vertices = new Vector3[3];
        switch (rotationInDegrees)
        {
            case 0:
                vertices[0] = RectangleVertices[0];
                vertices[1] = RectangleVertices[3];
                vertices[2] = RectangleVertices[1];
                break;
            case 90:
                vertices[0] = RectangleVertices[0];
                vertices[1] = RectangleVertices[2];
                vertices[2] = RectangleVertices[1];
                break;
            case 180:
                vertices[0] = RectangleVertices[2];
                vertices[1] = RectangleVertices[1];
                vertices[2] = RectangleVertices[3];
                break;
            case 270:
                vertices[0] = RectangleVertices[3];
                vertices[1] = RectangleVertices[2];
                vertices[2] = RectangleVertices[0];
                break;
        }
        changedMesh.vertices = vertices;
        changedMesh.triangles = new int[] { 0, 1, 2 };
    }

    Vector3[] triangleVertices(int rotationInDegrees)
    {
        Vector3[] res = new Vector3[3];
        switch (rotationInDegrees)
        {
            case 0:
                res[0] = RectangleVertices[0];
                res[1] = RectangleVertices[3];
                res[2] = RectangleVertices[1];
                break;
            case 90:
                res[0] = RectangleVertices[0];
                res[1] = RectangleVertices[2];
                res[2] = RectangleVertices[1];
                break;
            case 180:
                res[0] = RectangleVertices[0];
                res[1] = RectangleVertices[2];
                res[2] = RectangleVertices[3];
                break;
            case 270:
                res[0] = RectangleVertices[2];
                res[1] = RectangleVertices[3];
                res[2] = RectangleVertices[1];
                break;
        }
        return res;
    }

    Vector3[] circleVertices()
    {
        Vector3[] vertices = new Vector3[20];
        for(int i = 0; i < 20; i++)
        {
            vertices[i] = new Vector3(Mathf.Sin(2*Mathf.PI / 20 * i), 0, Mathf.Cos(2*Mathf.PI / (20) * i));
        }
        return vertices;
    }

    Vector3[] circleVertices(float radius)
    {
        Vector3[] vertices = new Vector3[20];
        for (int i = 0; i < 20; i++)
        {
            vertices[i] = new Vector3(Mathf.Sin(2 * Mathf.PI / 20 * i) * radius, 0, Mathf.Cos(2 * Mathf.PI / (20) * i) * radius);
        }
         return vertices;
    }

    public void destroyProjection()
    {
        try
        {
            if (this.projectionAufsicht != null)
            {
                Destroy(this.projectionAufsicht);
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.Message + " --> projectionAufsicht von " + this.name + " war null!");
            QuestDebugLogic.instance.log(e.Message + " --> projectionAufsicht von " + this.name + " war null!");
        }
        try
        {
            if (this.projectionSeitenansicht != null)
            {
                Destroy(this.projectionSeitenansicht);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message + " --> projectionAufsicht von " + this.name + " war null!");
            QuestDebugLogic.instance.log(e.Message + " --> projectionAufsicht von " + this.name + " war null!");
        }
        try
        {
            if (this.projectionVorderansicht != null)
            {
                Destroy(this.projectionVorderansicht);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message + " --> projectionAufsicht von " + this.name + " war null!");
            QuestDebugLogic.instance.log(e.Message + " --> projectionAufsicht von " + this.name + " war null!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

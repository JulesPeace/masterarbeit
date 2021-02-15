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
        try
        {
            project(TestProjectionRemoveMeLater);
        }
        catch
        {
            Debug.Log("TestProjectionRemoveMeLater ist leer!");
        }
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
        this.projectionAufsicht.transform.localPosition = this.transform.localPosition*2 + new Vector3(-4f, 0.025f-2*this.transform.localPosition.y, -4f);
        this.projectionAufsicht.transform.localScale = new Vector3(2, 2, 2);
        this.projectionAufsicht.GetComponent<MeshRenderer>().material = correctColor;
        Mesh mesh = new Mesh();
        try
        {
            //Debug.Log("Anfang des try-Blocks");
            //Debug.Log("Interactable ist " + interactable.name);
            if (interactable.name.Contains("Cube"))
            {
                QuestDebugLogic.instance.log("Cube snap erkannt");
                mesh.vertices = rectangleVertices();
                mesh.triangles = new int[] { 0, 3, 2, 0, 2, 1 };
            }
            else
            if (interactable.name.Contains("Wedge"))
            {
                //Debug.Log("Wedge snap erkannt: Bereite Ecken vor:");
                QuestDebugLogic.instance.log("Wedge snap erkannt");
                mesh.vertices = triangleVertices(0);
                //Debug.Log("Ecken vorbereitet, bereite Dreieck vor:");
                mesh.triangles = new int[] { 0, 1, 2 };
                //Debug.Log("Dreieck vorbereitet!");
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
        this.projectionSeitenansicht.transform.localPosition = this.transform.localPosition + new Vector3(-4f, 0.025f, -4f);
        this.projectionSeitenansicht.transform.localScale = new Vector3(2, 2, 2);
        this.projectionSeitenansicht.GetComponent<MeshRenderer>().material = correctColor;
        Mesh mesh = new Mesh();
        try
        {
            if (interactable.name.Contains("Cube"))
            {
                mesh.vertices = rectangleVertices();
                mesh.triangles = new int[] { 0, 3, 2, 0, 2, 1 };
            }
            else
            if (interactable.name.Contains("Wedge"))
            {
                //TODO Komplizierte interactable.transform.rotation analyse
                mesh.vertices = triangleVertices(0);
                mesh.triangles = new int[] { 0, 1, 2 };
            }
            else
            if (interactable.name.Contains("Prism"))
            {
                mesh.vertices = rectangleVertices();
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
            Debug.Log("projectionSeitenansicht() ohne interactable");
            QuestDebugLogic.instance.log("projection() ohne interactable");
        }
        this.projectionSeitenansicht.GetComponent<MeshFilter>().mesh = mesh;
    }

    void projectVorderansicht(GameObject interactable)
    {
        this.projectionVorderansicht = Instantiate(shadowProjection, vorderansicht_plane.transform, false);
        this.projectionVorderansicht.transform.localPosition = this.transform.localPosition + new Vector3(-4f, 0.025f, -4f);
        this.projectionVorderansicht.transform.localScale = new Vector3(2, 2, 2);
        this.projectionVorderansicht.GetComponent<MeshRenderer>().material = correctColor;
        Mesh mesh = new Mesh();
        try
        {
            if (interactable.name.Contains("Cube"))
            {
                mesh.vertices = rectangleVertices();
                mesh.triangles = new int[] { 0, 3, 2, 0, 2, 1 };
            }
            else
            if (interactable.name.Contains("Wedge"))
            {
                //TODO Komplizierte interactable.transform.rotation analyse
                mesh.vertices = triangleVertices(0);
                mesh.triangles = new int[] { 0, 1, 2 };
            }
            else
            if (interactable.name.Contains("Prism"))
            {
                mesh.vertices = rectangleVertices();
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
            QuestDebugLogic.instance.log("projection() ohne interactable");
        }
        this.projectionVorderansicht.GetComponent<MeshFilter>().mesh = mesh;
    }

    Vector3[] rectangleVertices()
    {
        return RectangleVertices;
    }

    Vector3[] calculateVerticesForWedge (Vector3 wedgeRotation)
    {
        Vector3[] vertices = new Vector3[3];

        return vertices;
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

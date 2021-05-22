using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShadowThrower : MonoBehaviour
{
    public GameObject vorderansicht_plane;
    public GameObject seitenansicht_plane;
    public GameObject aufsicht_plane;
    public GameObject gameArea_plane;
    public GameObject shadowProjection;
    private GameObject targetProjection;
    private GameObject projectionAufsicht;
    /// <summary>
    /// 1st entry is shape (rectangle,triangle,circle),2nd entry is rotation 
    /// </summary>
    private int[] projectionAufsichtTransform=new int[2];
    private GameObject projectionSeitenansicht;
    /// <summary>
    /// 1st entry is shape (rectangle,triangle,circle),2nd entry is rotation 
    /// </summary>
    private int[] projectionSeitenansichtTransform = new int[2];
    private GameObject projectionVorderansicht;
    /// <summary>
    /// 1st entry is shape (rectangle,triangle,circle),2nd entry is rotation 
    /// </summary>
    private int[] projectionVorderansichtTransform = new int[2];
    public Material correctColor;
    public Material incorrectColor;
    public Material targetColor;
    public Material gameAreaTargetColor;
    public GameObject TestProjectionRemoveMeLater;
    private static Vector3[] RectangleVertices = new Vector3[] { new Vector3(-0.5f, 0f, -0.5f), new Vector3(0.5f, 0f, -0.5f), new Vector3(0.5f, 0f, 0.5f), new Vector3(-0.5f, 0f, 0.5f) };

    // Start is called before the first frame update
    void Start()
    {
    }

    public void project(GameObject interactable)
    {
        try
        {
            Debug.Log(interactable.tag);
        }
        catch
        {
            QuestDebugLogic.instance.log("Keine Übergabe für project()");
        }
        projectAufsicht(interactable);
        projectSeitenansicht(interactable);
        projectVorderansicht(interactable);
    }

    public void createTargetProjection(string parentPlane, int x, int y, int shape, int rotationInByte)
    {
        GameObject plane=null;
        if (parentPlane.ToLower().Equals("seitenansicht"))
        {
            plane = seitenansicht_plane;
        }
        if (parentPlane.ToLower().Equals("vorderansicht"))
        {
            plane = vorderansicht_plane;
        }
        if (parentPlane.ToLower().Equals("aufsicht"))
        {
            plane = aufsicht_plane;
        }
        if (parentPlane.ToLower().Equals("gamearea"))
        {
            plane = gameArea_plane;
        }
        GameObject target = Instantiate(shadowProjection, plane.transform, false);
        target.transform.localPosition = new Vector3(x * 2 - 4f, 0.0025f, y * 2 - 4f);
        target.transform.localScale = new Vector3(2, 2, 2);
        if (!parentPlane.ToLower().Equals("gamearea"))
        {
            target.GetComponent<MeshRenderer>().material = targetColor;
        }
        else
        {
            target.GetComponent<MeshRenderer>().material = gameAreaTargetColor;
        }
        Mesh mesh = new Mesh();
        if (shape == 0)
        {
            rectangleMesh(mesh);
        }
        if (shape == 1)
        {
            triangleMesh(mesh, rotationInByte*90);
        }
        if (shape == 2)
        {
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
        target.GetComponent<MeshFilter>().mesh = mesh;
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
            if (interactable.name.Contains("Cube"))
            {
                rectangleMesh(mesh);
                GameController.instance.insertProgressAufsicht((int)this.transform.localPosition.z,(int)this.transform.localPosition.x,"rectangle",0);
                if(!GameController.instance.checkCorrectnessAufsicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.x, "rectangle", 0))
                {
                    this.projectionAufsicht.GetComponent<MeshRenderer>().material = incorrectColor;
                    this.projectionAufsicht.transform.localPosition -= new Vector3(0,0.005f,0);
                }
                projectionAufsichtTransform[0] = 0;
                projectionAufsichtTransform[1] = 0;
            }
            else
            if (interactable.name.Contains("Wedge"))
            {
                int x = (int)interactable.transform.rotation.eulerAngles.x;
                int y = (int)interactable.transform.rotation.eulerAngles.y;
                int z = (int)interactable.transform.rotation.eulerAngles.z;
                if (x == 90)
                {
                    while (y != 0)
                    {
                        y += 90;
                        z += 90;
                        y %= 360;
                        z %= 360;
                    }
                    int rot = (z + 90) % 360;
                    triangleMesh(mesh, rot);
                    GameController.instance.insertProgressAufsicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.x, "triangle", rot);
                    if (!GameController.instance.checkCorrectnessAufsicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.x, "triangle", rot))
                    {
                        this.projectionAufsicht.GetComponent<MeshRenderer>().material = incorrectColor;
                        this.projectionAufsicht.transform.localPosition -= new Vector3(0, 0.005f, 0);
                    }
                    projectionAufsichtTransform[0] = 1;
                    projectionAufsichtTransform[1] = rot;
                }
                else if (x == 270)
                {
                    while (y != 0)
                    {
                        y -= 90;
                        z += 90;
                        z %= 360;
                    }
                    int rot = (-z + 540) % 360;
                    triangleMesh(mesh, rot);
                    GameController.instance.insertProgressAufsicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.x, "triangle", rot);
                    if (!GameController.instance.checkCorrectnessAufsicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.x, "triangle", rot))
                    {
                        this.projectionAufsicht.GetComponent<MeshRenderer>().material = incorrectColor;
                        this.projectionAufsicht.transform.localPosition -= new Vector3(0, 0.005f, 0);
                    }
                    projectionAufsichtTransform[0] = 1;
                    projectionAufsichtTransform[1] = rot;
                }
                else
                {
                    rectangleMesh(mesh);
                    GameController.instance.insertProgressAufsicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.x, "rectangle", 0);
                    if (!GameController.instance.checkCorrectnessAufsicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.x, "rectangle", 0))
                    {
                        this.projectionAufsicht.GetComponent<MeshRenderer>().material = incorrectColor;
                        this.projectionAufsicht.transform.localPosition -= new Vector3(0, 0.005f, 0);
                    }
                    projectionAufsichtTransform[0] = 0;
                    projectionAufsichtTransform[1] = 0;
                }
            }
            else
            if (interactable.name.Contains("Prism"))
            {
                               //TODO: Rotationsabhängigkeit vom prisma UND isnertProgress() anpassen!
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
                GameController.instance.insertProgressAufsicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.x, "circle", 0);
                if (!GameController.instance.checkCorrectnessAufsicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.x, "circle", 0))
                {
                    this.projectionAufsicht.GetComponent<MeshRenderer>().material = incorrectColor;
                    this.projectionAufsicht.transform.localPosition -= new Vector3(0, 0.005f, 0);
                }
                projectionAufsichtTransform[0] = 2;
                projectionAufsichtTransform[1] = 0;
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
        this.projectionSeitenansicht.transform.localPosition = new Vector3(this.transform.localPosition.x * 2 - 4f, 0.025f, this.transform.localPosition.y * 2 - 4f );
        this.projectionSeitenansicht.transform.localScale = new Vector3(2, 2, 2);
        //Next line only for seitenansicht
        this.projectionSeitenansicht.transform.localRotation = Quaternion.Euler(0, 270, 0);
        this.projectionSeitenansicht.GetComponent<MeshRenderer>().material = correctColor;

        Mesh mesh = new Mesh();
        int xPlane = (int)-this.transform.localPosition.x + 3;
        int yPlane = (int)this.transform.localPosition.y;

        try
        {
            if (interactable.name.Contains("Cube"))
            {
                rectangleMesh(mesh);
                GameController.instance.insertProgressSeitenansicht(xPlane, yPlane, "rectangle", 0);
                if (!GameController.instance.checkCorrectnessSeitenansicht(xPlane, yPlane, "rectangle", 0))
                {
                    this.projectionSeitenansicht.GetComponent<MeshRenderer>().material = incorrectColor;
                    this.projectionSeitenansicht.transform.localPosition -= new Vector3(0, 0.005f, 0);
                }
                projectionSeitenansichtTransform[0] = 0;
                projectionSeitenansichtTransform[1] = 0;
            }
            else
            if (interactable.name.Contains("Wedge"))
            {
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
                    int rot= (z + 360) % 360;
                    triangleMesh(mesh, rot);
                    GameController.instance.insertProgressSeitenansicht(xPlane, yPlane, "triangle", rot);
                    if (!GameController.instance.checkCorrectnessSeitenansicht(xPlane, yPlane, "triangle", rot))
                    {
                        this.projectionSeitenansicht.GetComponent<MeshRenderer>().material = incorrectColor;
                        this.projectionSeitenansicht.transform.localPosition -= new Vector3(0, 0.005f, 0);
                    }
                    projectionSeitenansichtTransform[0] = 1;
                    projectionSeitenansichtTransform[1] = rot;
                }
                else
                {
                    rectangleMesh(mesh);
                    Debug.Log("InsertProgress für Seitenansicht bei: "+xPlane +","+ yPlane+"rectangle,0");
                    GameController.instance.insertProgressSeitenansicht(xPlane, yPlane, "rectangle", 0);
                    if (!GameController.instance.checkCorrectnessSeitenansicht(xPlane, yPlane, "rectangle", 0))
                    {
                        this.projectionSeitenansicht.GetComponent<MeshRenderer>().material = incorrectColor;
                        this.projectionSeitenansicht.transform.localPosition -= new Vector3(0, 0.005f, 0);
                    }
                    projectionSeitenansichtTransform[0] = 0;
                    projectionSeitenansichtTransform[1] = 0;
                }
            }
            else
            if (interactable.name.Contains("Prism"))
            {
                rectangleMesh(mesh);
                GameController.instance.insertProgressSeitenansicht(xPlane, yPlane, "rectangle", 0);
                if (!GameController.instance.checkCorrectnessSeitenansicht(xPlane, yPlane, "rectangle", 0))
                {
                    this.projectionSeitenansicht.GetComponent<MeshRenderer>().material = incorrectColor;
                    this.projectionSeitenansicht.transform.localPosition -= new Vector3(0, 0.005f, 0);
                }
                projectionSeitenansichtTransform[0] = 0;
                projectionSeitenansichtTransform[1] = 0;
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
                GameController.instance.insertProgressVorderansicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.y, "rectangle", 0);
                if (!GameController.instance.checkCorrectnessVorderansicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.y, "rectangle", 0))
                {
                    this.projectionVorderansicht.GetComponent<MeshRenderer>().material = incorrectColor;
                    this.projectionVorderansicht.transform.localPosition -= new Vector3(0, 0.005f, 0);
                }
                projectionVorderansichtTransform[0] = 0;
                projectionVorderansichtTransform[1] = 0;
            }
            else
            if (interactable.name.Contains("Wedge"))
            {
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
                    int rot = (z + 360) % 360;
                    triangleMesh(mesh, rot);
                    GameController.instance.insertProgressVorderansicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.y, "triangle", rot);
                    if (!GameController.instance.checkCorrectnessVorderansicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.y, "triangle", rot))
                    {
                        this.projectionVorderansicht.GetComponent<MeshRenderer>().material = incorrectColor;
                        this.projectionVorderansicht.transform.localPosition -= new Vector3(0, 0.005f, 0);
                    }
                    projectionVorderansichtTransform[0] = 1;
                    projectionVorderansichtTransform[1] = rot;
                }
                else
                {
                    rectangleMesh(mesh);
                    GameController.instance.insertProgressVorderansicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.y, "rectangle", 0);
                    if (!GameController.instance.checkCorrectnessVorderansicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.y, "rectangle", 0))
                    {
                        this.projectionVorderansicht.GetComponent<MeshRenderer>().material = incorrectColor;
                        this.projectionVorderansicht.transform.localPosition -= new Vector3(0, 0.005f, 0);
                    }
                    Debug.Log("Quadrat");
                    projectionVorderansichtTransform[0] = 0;
                    projectionVorderansichtTransform[1] = 0;
                }
            }
            else
            if (interactable.name.Contains("Prism"))
            {
                rectangleMesh(mesh);
                GameController.instance.insertProgressVorderansicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.y, "rectangle", 0);
                if (!GameController.instance.checkCorrectnessVorderansicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.y, "rectangle", 0))
                {
                    this.projectionVorderansicht.GetComponent<MeshRenderer>().material = incorrectColor;
                    this.projectionVorderansicht.transform.localPosition -= new Vector3(0, 0.005f, 0);
                }
                projectionVorderansichtTransform[0] = 0;
                projectionVorderansichtTransform[1] = 0;
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
                string shape="";
                switch (projectionAufsichtTransform[0])
                {
                    case 0: shape = "rectangle";
                        break;
                    case 1: shape = "triangle";
                        break;
                    case 2: shape = "circle";
                        break;
                }
                Destroy(this.projectionAufsicht);
                GameController.instance.removeProgressAufsicht((int)this.transform.localPosition.z,(int)this.transform.localPosition.x,shape,projectionAufsichtTransform[1]);
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
                string shape = "";
                switch (projectionSeitenansichtTransform[0])
                {
                    case 0:
                        shape = "rectangle";
                        break;
                    case 1:
                        shape = "triangle";
                        break;
                    case 2:
                        shape = "circle";
                        break;
                }
                Destroy(this.projectionSeitenansicht);
                GameController.instance.removeProgressSeitenansicht((-(int)this.transform.localPosition.x+3)%4, ((int)this.transform.localPosition.y), shape, projectionSeitenansichtTransform[1]);
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
                string shape = "";
                switch (projectionVorderansichtTransform[0])
                {
                    case 0:
                        shape = "rectangle";
                        break;
                    case 1:
                        shape = "triangle";
                        break;
                    case 2:
                        shape = "circle";
                        break;
                }
                Destroy(this.projectionVorderansicht);
                GameController.instance.removeProgressVorderansicht((int)this.transform.localPosition.z, (int)this.transform.localPosition.y, shape, projectionVorderansichtTransform[1]);
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

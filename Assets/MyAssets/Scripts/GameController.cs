using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tilia.Interactions.SnapZone;
using UnityEngine.VFX;

public class GameController : MonoBehaviour
{
    [Header("Play Area")]
    public int length;
    public int width;
    public int height;
    public GameObject snapZonesGame;
    private GameObject[,,] playArea;
    public int debugRemoveMeLater = 0;
    public GameObject vfxGameObject;
    [HideInInspector]
    public static int currentSzenario = 1;
    public static bool loading = false;
    public int[][] flaggedIndexes = new int[64][];
    public GameObject tutorialMaterial;
    private IEnumerator coroutine;
    //public Vector2 currentRightThumbstick = new Vector2();
    public GameObject table;


    public static GameController instance;

    ///<summary>1st and 2nd digit are coordinates, 3rd digit is shape (rectangle, triangle, circle), 4th digit is rotation (0, 90, 180, 270) </summary>
    private byte[,,,] targetAufsicht = new byte[4, 4, 3, 4];
    ///<summary>1st and 2nd digit are coordinates, 3rd digit is shape (rectangle, triangle, circle, 2-triangle-rectangle), 4th digit is rotation (0, 90, 180, 270) </summary>
    private byte[,,,] currentProgressAufsicht = new byte[4, 4, 4, 4];
    ///<summary>1st and 2nd digit are coordinates, 3rd digit is shape (rectangle, triangle, circle), 4th digit is rotation (0, 90, 180, 270) </summary>
    private byte[,,,] targetSeitenansicht = new byte[4, 4, 3, 4];
    ///<summary>1st and 2nd digit are coordinates, 3rd digit is shape (rectangle, triangle, circle, 2-triangle-rectangle), 4th digit is rotation (0, 90, 180, 270) </summary>
    //TODO make this private
    public byte[,,,] currentProgressSeitenansicht = new byte[4, 4, 4, 4];
    ///<summary>1st and 2nd digit are coordinates, 3rd digit is shape (rectangle, triangle, circle), 4th digit is rotation (0, 90, 180, 270) </summary>
    private byte[,,,] targetVorderansicht = new byte[4, 4, 3, 4];
    ///<summary>1st and 2nd digit are coordinates, 3rd digit is shape (rectangle, triangle, circle, 2-triangle-rectangle), 4th digit is rotation (0, 90, 180, 270) </summary>
    private byte[,,,] currentProgressVorderansicht = new byte[4, 4, 4, 4];

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        loading = true;
        loadSzenario(currentSzenario);
        //szenario1();
        loading = false;
    }

    // Update is called once per frame
    public void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One)&&!loading)
        {
            loading = true;
            resetGame();
            loadSzenario(currentSzenario + 1);
            loading = false;
            //szenario1();
        }
        if ((OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y>0.05)|| (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y<-0.05))
        {
            float y = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
            if (y>0 && table.transform.position.y<0.6f || y<0 && table.transform.position.y>-0.25f)
            table.transform.position += (new Vector3(0f, y, 0f)) * Time.deltaTime;
        }
        //currentRightThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
    }

    public string arrayToString(byte[,,,] arr)
    {
        string res = "\n[";
        int imax = arr.GetLength(0);
        int jmax = arr.GetLength(1);
        int kmax = arr.GetLength(2);
        int lmax = arr.GetLength(3);

        for (int i = 0; i < imax; i++)
        {
            for (int j = 0; j < jmax; j++)
            {
                res += "[";
                for (int k = 0; k < kmax; k++)
                {
                    res += "[";
                    for (int l = 0; l < lmax; l++)
                    {
                        res += arr[-j+3,-i+3,k,l]+",";
                    }
                    res += "]";
                }
                res += "], ";
            }
            res += "\n";
            //Console.Write(Environment.NewLine + Environment.NewLine);
        }
        res += "]";
        return res;
    }
    
    public void loadSzenario(int szenarioNumber)
    {
        currentSzenario = szenarioNumber;
        if (szenarioNumber != 1)
        {
            resetGame();
        }
        
        switch (currentSzenario)
        {
            case 1:
                DebugUIBuilder.instance.Show();
                targetAufsicht[3, 0, 0, 0] += 1;
                targetAufsicht[2, 1, 0, 0] += 1;
                targetAufsicht[1, 2, 2, 0] += 1;
                targetSeitenansicht[3, 0, 0, 0] += 1;
                targetSeitenansicht[2, 0, 0, 0] += 1;
                targetSeitenansicht[1, 0, 0, 0] += 1;
                targetVorderansicht[3, 0, 1, 3] += 1;
                targetVorderansicht[2, 0, 0, 0] += 1;
                targetVorderansicht[1, 0, 0, 0] += 1;
                tutorialMaterial.SetActive(true);
                //QuestCanvasCenter.instance.log("Baue die Figur nach, deren drei Schatten im Spielbereich zu sehen sind.");
                break;
            case 2:
                tutorialMaterial.SetActive(false);
                DebugUIBuilder.instance.Hide();
                targetAufsicht[2, 1, 0, 0] += 1;
                targetAufsicht[3, 1, 0, 0] += 1;
                targetSeitenansicht[2, 0, 0, 0] += 1;
                targetVorderansicht[3, 0, 1, 0] += 1;
                targetVorderansicht[2, 0, 1, 3] += 1;
                break;
            case 3:
                targetAufsicht[3, 0, 0, 0] += 1;
                targetAufsicht[2, 0, 0, 0] += 1;
                targetAufsicht[2, 1, 0, 0] += 1;
                targetAufsicht[1, 0, 0, 0] += 1;
                targetSeitenansicht[3, 0, 0, 0] += 1;
                targetSeitenansicht[3, 1, 0, 0] += 1;
                targetSeitenansicht[3, 2, 0, 0] += 1;
                targetSeitenansicht[2, 2, 1, 2] += 1;
                targetVorderansicht[2, 0, 0, 0] += 1;
                targetVorderansicht[2, 1, 0, 0] += 1;
                targetVorderansicht[3, 2, 1, 1] += 1;
                targetVorderansicht[2, 2, 0, 0] += 1;
                targetVorderansicht[1, 2, 1, 2] += 1;
                break;
            case 4:
                targetAufsicht[3, 2, 0, 0] += 1;
                targetAufsicht[2, 2, 0, 0] += 1;
                targetAufsicht[1, 2, 0, 0] += 1;
                targetSeitenansicht[1, 0, 0, 0] += 1;
                targetSeitenansicht[1, 1, 1, 0] += 1;
                targetSeitenansicht[1, 1, 1, 3] += 1;
                targetSeitenansicht[1, 1, 2, 0] += 1;
                targetVorderansicht[3, 0, 0, 0] += 1;
                targetVorderansicht[3, 1, 0, 0] += 1;
                targetVorderansicht[2, 1, 0, 0] += 1;
                targetVorderansicht[1, 1, 0, 0] += 1;
                targetVorderansicht[1, 0, 0, 0] += 1;
                break;
            case 5:
                targetAufsicht[3, 1, 0, 0] += 1;
                targetAufsicht[2, 1, 0, 0] += 1;
                targetAufsicht[1, 1, 0, 0] += 1;
                targetAufsicht[1, 0, 0, 0] += 1;
                targetSeitenansicht[2, 0, 0, 0] += 1;
                targetSeitenansicht[3, 0, 0, 0] += 1;
                targetSeitenansicht[2, 1, 0, 0] += 1;
                targetSeitenansicht[3, 1, 0, 0] += 1;
                targetSeitenansicht[2, 2, 0, 0] += 1;
                targetSeitenansicht[3, 2, 1, 0] += 1;
                targetVorderansicht[3, 0, 0, 0] += 1;
                targetVorderansicht[1, 0, 0, 0] += 1;
                targetVorderansicht[1, 1, 0, 0] += 1;
                targetVorderansicht[1, 2, 0, 0] += 1;
                targetVorderansicht[2, 2, 1, 0] += 1;
                targetVorderansicht[2, 1, 1, 2] += 1;
                targetVorderansicht[3, 1, 1, 0] += 1;
                break;
            case 6:
                targetAufsicht[3, 2, 0, 0] += 1;
                targetAufsicht[2, 2, 0, 0] += 1;
                targetAufsicht[0, 0, 0, 0] += 1;
                targetAufsicht[0, 1, 0, 0] += 1;
                targetAufsicht[1, 1, 1, 1] += 1;
                targetAufsicht[1, 2, 1, 3] += 1;
                targetSeitenansicht[3, 0, 0, 0] += 1;
                targetSeitenansicht[2, 0, 1, 2] += 1;
                targetSeitenansicht[2, 1, 0, 0] += 1;
                targetSeitenansicht[1, 1, 0, 0] += 1;
                targetSeitenansicht[2, 2, 2, 0] += 1;
                targetSeitenansicht[1, 2, 0, 0] += 1;
                targetVorderansicht[0, 0, 0, 0] += 1;
                targetVorderansicht[0, 1, 0, 0] += 1;
                targetVorderansicht[0, 2, 0, 0] += 1;
                targetVorderansicht[1, 1, 0, 0] += 1;
                targetVorderansicht[1, 2, 0, 0] += 1;
                targetVorderansicht[2, 1, 0, 0] += 1;
                targetVorderansicht[3, 1, 1, 1] += 1;
                targetVorderansicht[3, 2, 2, 0] += 1;
                break;
            case 7:
                currentSzenario = 1;
                targetAufsicht[3, 0, 0, 0] += 1;
                targetAufsicht[2, 1, 0, 0] += 1;
                targetAufsicht[1, 2, 2, 0] += 1;
                targetSeitenansicht[3, 0, 0, 0] += 1;
                targetSeitenansicht[2, 0, 0, 0] += 1;
                targetSeitenansicht[1, 0, 0, 0] += 1;
                targetVorderansicht[3, 0, 1, 3] += 1;
                targetVorderansicht[2, 0, 0, 0] += 1;
                targetVorderansicht[1, 0, 0, 0] += 1;
                break;
        }
        createPlayArea();
    }

    public void szenario1()
    {
        //TODO: test and in-comment next line
        //resetGame();
        targetAufsicht[3, 0, 0, 0] += 1;
        targetAufsicht[2, 0, 0, 0] += 1;
        targetAufsicht[2, 1, 0, 0] += 1;
        targetAufsicht[1, 0, 0, 0] += 1;
        targetSeitenansicht[3, 0, 0, 0] += 1;
        targetSeitenansicht[3, 1, 0, 0] += 1;
        targetSeitenansicht[3, 2, 0, 0] += 1;
        targetSeitenansicht[2, 2, 1, 2] += 1;
        targetVorderansicht[2, 0, 0, 0] += 1;
        targetVorderansicht[2, 1, 0, 0] += 1;
        targetVorderansicht[3, 2, 1, 1] += 1;
        targetVorderansicht[2, 2, 0, 0] += 1;
        targetVorderansicht[1, 2, 1, 2] += 1;
        createPlayArea();
    }

    public void removeProgressAufsicht(int x, int y, string shape, int rotation)
    {
        if (shape.ToLower().Equals("rectangle"))
        {
            currentProgressAufsicht[x, y, 0, 0] -= 1;
        }
        if (shape.ToLower().Equals("triangle"))
        {
            currentProgressAufsicht[x, y, 1, rotation/90] -= 1;
            //2 triangles can be 1 rectangle
            int compareIndex = (rotation/90 + 2) % 4;
            if (currentProgressAufsicht[x, y, 1, compareIndex] != 0)
            {
                currentProgressAufsicht[x, y, 4, 0] -= 1;
            }
        }
        if (shape.ToLower().Equals("circle"))
        {
            currentProgressAufsicht[x, y, 2, 0] -= 1;
        }
    }


    public void removeProgressSeitenansicht(int x, int y, string shape, int rotation)
    {
        if (shape.ToLower().Equals("rectangle"))
        {
            currentProgressSeitenansicht[x, y, 0, 0] -= 1;
        }
        if (shape.ToLower().Equals("triangle"))
        {
            currentProgressSeitenansicht[x, y, 1, rotation/90] -= 1;
            //2 triangles can be 1 rectangle
            int compareIndex = (rotation/90 + 2) % 4;
            if (currentProgressSeitenansicht[x, y, 1, compareIndex] != 0)
            {
                currentProgressSeitenansicht[x, y, 4, 0] -= 1;
            }
        }
        if (shape.ToLower().Equals("circle"))
        {
            currentProgressSeitenansicht[x, y, 2, 0] -= 1;
        }
    }


    public void removeProgressVorderansicht(int x, int y, string shape, int rotation)
    {
        if (shape.ToLower().Equals("rectangle"))
        {
            currentProgressVorderansicht[x, y, 0, 0] -= 1;
        }
        if (shape.ToLower().Equals("triangle"))
        {
            currentProgressVorderansicht[x, y, 1, rotation / 90] -= 1;
            //2 triangles can be 1 rectangle
            int compareIndex = (rotation / 90 + 2) % 4;
            if (currentProgressVorderansicht[x, y, 1, compareIndex] != 0)
            {
                currentProgressVorderansicht[x, y, 4, 0] -= 1;
            }
        }
        if (shape.ToLower().Equals("circle"))
        {
            currentProgressVorderansicht[x, y, 2, 0] -= 1;
        }
    }

    ///<summary>
    ///inserts Progress depending on if parameter @plane ist seitenansicht, aufsicht or vorderansicht
    /// </summary> 
    public void insertProgress(int x, int y, string shape, int rotation, String plane)
    {
        byte[,,,] currentProgress=null;
        if (plane.ToLower().Equals("aufsicht"))
        {
            currentProgress = currentProgressAufsicht;
        }
        if (plane.ToLower().Equals("seitenansicht"))
        {
            currentProgress = currentProgressSeitenansicht;
        }
        if (plane.ToLower().Equals("vorderansicht"))
        {
            currentProgress = currentProgressVorderansicht;
        }
        if (shape.ToLower().Equals("rectangle"))
        {
            currentProgress[x, y, 0, 0] += 1;
        }
        if (shape.ToLower().Equals("triangle"))
        {
            currentProgress[x, y, 1, rotation / 90] += 1;
            //2 triangles can be 1 rectangle
            int compareIndex = (rotation / 90 + 2) % 4;
            if (currentProgress[x, y, 1, compareIndex] != 0)
            {
                currentProgress[x, y, 3, 0] += 1;
            }
        }
        if (shape.ToLower().Equals("circle"))
        {
            currentProgress[x, y, 2, 0] += 1;
        }

    }

    public void insertProgressAufsicht(int x, int y, string shape, int rotation)
    {
        if (shape.ToLower().Equals("rectangle"))
        {
            currentProgressAufsicht[x, y, 0, 0] += 1;
        }
        if (shape.ToLower().Equals("triangle"))
        {
            currentProgressAufsicht[x, y, 1, rotation / 90] += 1;
            //2 triangles can be 1 rectangle
            int compareIndex = (rotation / 90 + 2) % 4;
            if (currentProgressAufsicht[x, y, 1, compareIndex] != 0)
            {
                currentProgressAufsicht[x, y, 3, 0] += 1;
            }
        }
        if (shape.ToLower().Equals("circle"))
        {
            currentProgressAufsicht[x, y, 2, 0] += 1;
        }
    }

    public void insertProgressSeitenansicht(int x, int y, string shape, int rotation)
    {
        if (shape.ToLower().Equals("rectangle"))
        {
            currentProgressSeitenansicht[x, y, 0, 0] += 1;
        }
        if (shape.ToLower().Equals("triangle"))
        {
            currentProgressSeitenansicht[x, y, 1, rotation / 90] += 1;
            //2 triangles can be 1 rectangle
            int compareIndex = (rotation / 90 + 2) % 4;
            if (currentProgressSeitenansicht[x, y, 1, compareIndex] != 0)
            {
                currentProgressSeitenansicht[x, y, 3, 0] += 1;
            }
        }
        if (shape.ToLower().Equals("circle"))
        {
            currentProgressSeitenansicht[x, y, 2, 0] += 1;
        }
    }

    public void insertProgressVorderansicht(int x, int y, string shape, int rotation)
    {
        if (shape.ToLower().Equals("rectangle"))
        {
            currentProgressVorderansicht[x, y, 0, 0] += 1;
        }
        if (shape.ToLower().Equals("triangle"))
        {
            currentProgressVorderansicht[x, y, 1, rotation / 90] += 1;
            //2 triangles can be 1 rectangle
            int compareIndex = (rotation / 90 + 2) % 4;
            if (currentProgressVorderansicht[x, y, 1, compareIndex] != 0)
            {
                currentProgressVorderansicht[x, y, 3, 0] += 1;
            }
        }
        if (shape.ToLower().Equals("circle"))
        {
            currentProgressVorderansicht[x, y, 2, 0] += 1;
        }
    }

    void createPlayArea()
    {
        playArea = new GameObject[length, width, height];
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < width; y++)
            {
                for (int z = 0; z < height; z++)
                {
                    playArea[x, y, z] = snapZonesGame.GetComponent<SnapZoneGenerator>().generateSnapZone();
                    GameObject current = playArea[x, y, z];
                    current.transform.localPosition = new Vector3(x, y, z);
                    //Generate the target projections
                    for (int m = 0; m < 3; m++)
                    {
                        if (targetAufsicht[z, x, m, y] > 0)
                        {
                            playArea[x, y, z].GetComponent<ShadowThrower>().createTargetProjection("aufsicht", x, z, m, y);
                            playArea[x, y, z].GetComponent<ShadowThrower>().createTargetProjection("gameArea", x, z, m, y);
                        }
                        if (targetSeitenansicht[z, x, m, y] > 0)
                        {
                            playArea[x, y, z].GetComponent<ShadowThrower>().createTargetProjection("seitenansicht", (-z+3)%4, x, m, (y+1)%4);
                        }
                        if (targetVorderansicht[z, x, m, y] > 0)
                        {
                            playArea[x, y, z].GetComponent<ShadowThrower>().createTargetProjection("vorderansicht", x, z, m, y);
                        }
                    }
                    if (current.transform.localPosition.y != 0)
                    {
                        current.SetActive(false);
                    }
                    else
                    {
                        current.SetActive(true);
                    }
                }
            }
        }
        //make sure the initial objects, that are copied before, aren't deleted on game reset for further game rounds to be created
        snapZonesGame.GetComponent<SnapZoneGenerator>().snapZone.GetComponent<dontDeleteMe>().dontDelete = true;
    }

    public void activateSnapZone(GameObject snapZone)
    {
        snapZone.SetActive(true);
    }
    
    public void deactivateSnapZone(GameObject snapZone)
    {
        snapZone.GetComponentInChildren<SnapZoneConfigurator>().Unsnap();
        snapZone.SetActive(false);
    }

    /// <summary>
    /// returns the number of neighbours of a given snapzone, whose SnapzoneFacade ZoneState is "ZoneIsSnapped".
    /// </summary>
    /// <param name="index">The index of the Snapzone in playArea</param>
    /// <returns></returns>
    private byte neighboursSnapped(int x, int y, int z)
    {//TODO SnapZones deactivaten nicht mehr. Warum?
        byte count = 0;
        if (x != 0)
        {
            if (playArea[x - 1, y, z].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped"))
            {
                count+=1;
            }
        }
        if (x < length - 1)
        {
            if (playArea[x + 1, y, z].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped"))
            {
                count+=1;
            }
        }
        if (y != 0)
        {
            if (playArea[x, y - 1, z].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped"))
            {
                count+=1;
            }
        }
        if (y < height - 1)
        {
            if (playArea[x, y + 1, z].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped"))
            {
                count+=1;
            }
        }
        if (z != 0)
        {
            if (playArea[x, y, z - 1].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped"))
            {
                count += 1;
            }
        }
        if (z < width - 1)
        {
            if (playArea[x, y, z + 1].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped"))
            {
                count += 1;
            }
        }
        return count;
    }


    public void deactivateNeighbourSnapzones(GameObject snapZone)
    {
        Debug.Log("Hauptmarkierroutine Aufruf durch Snapzone "+ string.Join(",", getIndexOfSnapZoneInPlayArea(snapZone)));
        for(int j = 0; j<4; j++)
        {
            for(int i =0; i<4; i++)
            {
                playArea[i, 0, j].GetComponent<ShadowThrower>().flagGroundConnection = true;
                if (playArea[i, 0, j].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped"))
                {
                    if (playArea[i, 1, j].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped")&&!playArea[i,1,j].GetComponent<ShadowThrower>().flagGroundConnection)
                     {
                        Debug.Log("Aufruf Helper bei " + i + ",1,"+j);
                        activateDeativateNeighbourSnapzonesHelper(playArea[i,1,j]);
                        Debug.Log("Rücksprung");
                     }
                }
            }
        }
        Debug.Log("Alle unmarkierten unsnappen...");
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                for (int k =0; k <4; k++)
                {
                    //alle durchlaufen, Unsnappen was nicht geflagt ist + flag zurücksetzen.
                    if (!playArea[i, j, k].GetComponent<ShadowThrower>().flagGroundConnection && playArea[i,j,k].activeSelf)
                    {
                        //playArea[i, j, k].GetComponent<GameObjectActivator>().origin = true;
                        //TODO Warum hier nicht-sensitiv Unsnappen!? Zonestate checken, ggf. deaktivieren statt Unsnappen.
                        playArea[i,j,k].GetComponentInChildren<SnapZoneConfigurator>().Unsnap();
                        //deactivateSnapZone(playArea[i, j, k]);
                    }
                    playArea[i, j, k].GetComponent<ShadowThrower>().flagGroundConnection = false;
                }

            }
        }
        Debug.Log("erfolreich!");
        //in diesem Durchlauf alle leeren SnapZones ohne Nachbar deaktivieren
        //TODO vielleicht ist die Rechenleistung das Problem - länger warten (bis der frame berechnet ist) und kein Absturz?
        coroutine = EmptySnapZoneDeactivationWorkAround();
        StartCoroutine(coroutine);
    }


    /// <summary>
    /// This is neccessary, because right after Unsnapping a SnapZOne has the state "ZoneIsActive" and will bug, if it is deactivated in that state. This method waits for 0.1s and THEN deactivates all empty SnapZones without neighbour.
    /// </summary>
    /// <param name="newlySpawnedObject"></param>
    /// <returns></returns>
    public IEnumerator EmptySnapZoneDeactivationWorkAround()
    {
        yield return new WaitForEndOfFrame();// WaitForSeconds(0.1f);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    //Debug.Log("ZoneState von (" + i + "," + j + "," + k + ") ist " + playArea[i, j, k].GetComponent<SnapZoneFacade>().ZoneState.ToString());
                    if (neighboursSnapped(i, j, k) == 0 && (playArea[i, j, k].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsEmpty")|| playArea[i, j, k].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsActivated")))
                    {
                        playArea[i, j, k].SetActive(false);
                    }
                }

            }
        }
    }

    ///<summary>
    ///snapzone.flagGroundConnection auf true setzen, rekursiver Aufruf aller _gesnapten_ Nachbarn, deren flagGroundConnection = false ist  </summary>
    public void activateDeativateNeighbourSnapzonesHelper(GameObject snapZone)
    {
        snapZone.GetComponent<ShadowThrower>().flagGroundConnection = true;
        int[] index = getIndexOfSnapZoneInPlayArea(snapZone);

        flaggedIndexes[16*index[2]+4*index[1]+index[0]] = index;

        if (index[0] > 0)
        {
            if (playArea[index[0]-1, index[1], index[2]].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped") && !playArea[index[0] - 1, index[1], index[2]].GetComponent<ShadowThrower>().flagGroundConnection)
            {
                activateDeativateNeighbourSnapzonesHelper(playArea[index[0] - 1, index[1], index[2]]);
            }
        }
        if(index[0] != length - 1)
        {
            if (playArea[index[0] + 1, index[1], index[2]].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped") && !playArea[index[0] + 1, index[1], index[2]].GetComponent<ShadowThrower>().flagGroundConnection)
            {
                activateDeativateNeighbourSnapzonesHelper(playArea[index[0] + 1, index[1], index[2]]);
            }
        }
        if (index[1] > 1)
        {
            if (playArea[index[0], index[1]-1, index[2]].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped") && !playArea[index[0], index[1]-1, index[2]].GetComponent<ShadowThrower>().flagGroundConnection)
            {
                activateDeativateNeighbourSnapzonesHelper(playArea[index[0], index[1]-1, index[2]]);
            }
        }
        if (index[1] != height-1)
        {
            if (playArea[index[0], index[1]+1, index[2]].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped") && !playArea[index[0] + 1, index[1]+1, index[2]].GetComponent<ShadowThrower>().flagGroundConnection)
            {
                activateDeativateNeighbourSnapzonesHelper(playArea[index[0], index[1]+1, index[2]]);
            }
        }
        if (index[2] > 0)
        {
            if (playArea[index[0], index[1], index[2]-1].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped") && !playArea[index[0], index[1], index[2]-1].GetComponent<ShadowThrower>().flagGroundConnection)
            {
                activateDeativateNeighbourSnapzonesHelper(playArea[index[0], index[1], index[2]-1]);
            }
        }
        if (index[2] != width-1)
        {
            if (playArea[index[0], index[1], index[2]+1].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped") && !playArea[index[0], index[1], index[2]+1].GetComponent<ShadowThrower>().flagGroundConnection)
            {
                activateDeativateNeighbourSnapzonesHelper(playArea[index[0], index[1], index[2]+1]);
            }
        }
    }

    ///"activate true if activating, false if deactivating
    public void activateDeactivateNeighbours(GameObject snapZone, bool activate)
    {
        int[] index = getIndexOfSnapZoneInPlayArea(snapZone);
        if (index[0] != 0)
        {
            if (activate)
            {
                activateSnapZone(playArea[index[0] - 1, index[1], index[2]]);
            }
            /*else
            {
                if (index[1] != 0 && (neighboursSnapped(new int[] { index[0]-1,index[1],index[2] })<2)&& !playArea[index[0] - 1, index[1], index[2]].GetComponent<GameObjectActivator>().origin)
                {
                    deactivateSnapZone(playArea[index[0] - 1, index[1], index[2]]);
                }
            }*/
        }
        if (index[0] != length - 1)
        {
            if (activate)
            {
                activateSnapZone(playArea[index[0] + 1, index[1], index[2]]);
            }
            /*else
            {
                if ((index[1] != 0) && (neighboursSnapped(new int[] { index[0] + 1, index[1], index[2] })<2)&&!playArea[index[0] + 1, index[1], index[2]].GetComponent<GameObjectActivator>().origin)
                {
                    deactivateSnapZone(playArea[index[0] + 1, index[1], index[2]]);
                }
            }*/
        }
        //TODO Die Snapzone darunter muss ggf. aktiviert/deaktiviert werden!
        //Die Snapzone darunter muss weder aktiviert noch deaktiviert werden!
        if (index[1] > 1)
        {
            if (activate)
            {
                activateSnapZone(playArea[index[0], index[1] - 1, index[2]]);
            }
        }
          /* else
            {
                deactivateSnapZone(playArea[index[0], index[1]-1, index[2]]);
            }
        }*/
        if (index[1] != height - 1)
        {
            if (activate)
            {
                activateSnapZone(playArea[index[0], index[1] + 1, index[2]]);
            }
            /*else
            {
                if((neighboursSnapped(new int[] { index[0], index[1] + 1, index[2] })<2)&& !playArea[index[0], index[1]+1, index[2]].GetComponent<GameObjectActivator>().origin)
                {
                    deactivateSnapZone(playArea[index[0], index[1] + 1, index[2]]);
                }
            }*/
        }
        if (index[2] != 0)
        {
            if (activate)
            {
                activateSnapZone(playArea[index[0], index[1], index[2] - 1]);
            }
            /*else
            {
                if ((index[1] != 0) && (neighboursSnapped(new int[] { index[0], index[1], index[2] - 1 })<2) && !playArea[index[0], index[1], index[2]-1].GetComponent<GameObjectActivator>().origin)
                {
                    deactivateSnapZone(playArea[index[0], index[1], index[2] - 1]);
                }
            }*/
        }
        if (index[2] != width - 1)
        {
            if (activate)
            {
                activateSnapZone(playArea[index[0], index[1], index[2] + 1]);
            }
            /*else
            {
                if ((index[1] != 0) && (neighboursSnapped(new int[] { index[0], index[1], index[2] + 1 })<2) && !playArea[index[0], index[1], index[2]+1].GetComponent<GameObjectActivator>().origin)
                {
                    deactivateSnapZone(playArea[index[0], index[1], index[2] + 1]);
                }
            }*/
        }
        if (!activate&& snapZone.GetComponent<GameObjectActivator>().origin)
        {
            //snapZone.GetComponent<GameObjectActivator>().origin = true;
            deactivateNeighbourSnapzones(snapZone);
            //TODO delete this
            debugRemoveMeLater++;
        }
        if (checkVictory())
        {
            QuestDebugLogic.instance.log("geschafft! Alpha 0.9.2 komplettiert!");
            StartCoroutine(fireworks());
        }
    }

    private IEnumerator fireworks()
    {
        QuestDebugLogic.instance.log("Coroutine gestartet");
        vfxGameObject.SetActive(true);
        VisualEffect vfx = vfxGameObject.GetComponent<VisualEffect>();
        try
        {
            vfx.SendEvent("victory");
            QuestDebugLogic.instance.log(QuestDebugLogic.logTextM.text + "\nvfx event victory geladen");
            //vfx.SendEvent("victory");
            //QuestDebugLogic.instance.log(QuestDebugLogic.logTextM.text + "\nvfx event victory geladen");
        }
        catch(Exception e)
        {
            QuestDebugLogic.instance.log(QuestDebugLogic.logTextM.text + "\nFehler beim vfx event: " +e.Message);
        }
        yield return new WaitForSeconds(.8f);
        vfx.SendEvent("victory");
        yield return new WaitForSeconds(.8f);
        vfx.SendEvent("victory");
        yield return new WaitForSeconds(10);
    }

    private int[] getIndexOfSnapZoneInPlayArea(GameObject gameObject)
    {
        int[] res = new int[3];
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    if (gameObject == instance.playArea[i, j, k])
                    {
                        res[0] = i;
                        res[1] = j;
                        res[2] = k;
                        return res;
                    }
                }
            }
        }
        Debug.Log("Übergebene SnapZone ist nicht in playArea!");
        return null;
    }
    
    /// <summary>
    /// 0 rectangle, 1 triangle, 2 circle. 255 error
    /// </summary>
    /// <param name="shape"></param>
    /// <returns></returns>
    public byte shapeStringToIndex(string shape)
    {
        if (shape.ToLower().Equals("rectangle"))
        {
            return 0;
        }
        if (shape.ToLower().Equals("triangle"))
        {
            return 1;
        }
        if (shape.ToLower().Equals("circle"))
        {
            return 2;
        }
        return 255;
    }

    ///<summary>
    /// checks correctness depending on if parameter @plane ist seitenansicht, aufsicht or vorderansicht
    /// </summary> 
    public bool checkCorrectness(int x, int y, string shape, int rotation, String plane)
    {
        byte[,,,] target=null;
        if (plane.ToLower().Equals("aufsicht"))
        {
            target = targetAufsicht;
        }
        if (plane.ToLower().Equals("seitenansicht"))
        {
            target = targetSeitenansicht;
        }
        if (plane.ToLower().Equals("vorderansicht"))
        {
            target = targetVorderansicht;
        }
        byte shapeIndex = shapeStringToIndex(shape);
        if (shapeIndex == 0)
        {
            //if target needs a rectangle there
            if (target[x, y, 0, 0] != 0)
            {
                return true;
            }
        }
        if (shapeIndex == 1)
        {
            //if target needs this triangle or a rectangle there
            if ((target[x, y, 1, rotation / 90] != 0) || (target[x, y, 0, 0] != 0))
            {
                return true;
            }
        }
        if (shapeIndex == 2)
        {
            //if target need this circle or a rectangle there
            if ((target[x, y, 2, 0] != 0) || (target[x, y, 0, 0] != 0))
            {
                return true;
            }
        }

        return false;
    }

        /// <summary>
        /// checks, if the new projections are ok with the target
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="shape"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public bool checkCorrectnessAufsicht(int x, int y, string shape, int rotation)
    {
        byte shapeIndex = shapeStringToIndex(shape);
        if (shapeIndex == 0)
        {
            //if target needs a rectangle there
            if (targetAufsicht[x, y, 0, 0] != 0)
            {
                return true;
            }
        }
        if (shapeIndex == 1)
        {
            //if target needs this triangle or a rectangle there
            if ((targetAufsicht[x, y, 1, rotation / 90] != 0) || (targetAufsicht[x, y, 0, 0] != 0))
            {
                return true;
            }
        }
        if (shapeIndex == 2)
        {
            //if target need this circle or a rectangle there
            if ((targetAufsicht[x, y, 2, 0] != 0) || (targetAufsicht[x, y, 0, 0] != 0))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// checks, if the new projections are ok with the target
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="shape"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public bool checkCorrectnessSeitenansicht(int x, int y, string shape, int rotation)
    {
        byte shapeIndex = shapeStringToIndex(shape);
        if (shapeIndex == 0)
        {
            //if target needs a rectangle there
            if (targetSeitenansicht[x, y, 0, 0] != 0)
            {
                return true;
            }
        }
        if (shapeIndex == 1)
        {
            //if target needs this triangle or a rectangle there
            if ((targetSeitenansicht[x, y, 1, rotation / 90] != 0) || (targetSeitenansicht[x, y, 0, 0] != 0))
            {
                return true;
            }
        }
        if (shapeIndex == 2)
        {
            //if target need this circle or a rectangle there
            if ((targetSeitenansicht[x, y, 2, 0] != 0) || (targetSeitenansicht[x, y, 0, 0] != 0))
            {
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// checks, if the new projections are ok with the target
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="shape"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public bool checkCorrectnessVorderansicht(int x, int y, string shape, int rotation)
    {
        byte shapeIndex = shapeStringToIndex(shape);
        if (shapeIndex == 0)
        {
            //if target needs a rectangle there
            if (targetVorderansicht[x, y, 0, 0] != 0)
            {
                return true;
            }
        }
        if (shapeIndex == 1)
        {
            //if target needs this triangle or a rectangle there
            if ((targetVorderansicht[x, y, 1, rotation / 90] != 0) || (targetVorderansicht[x, y, 0, 0] != 0))
            {
                return true;
            }
        }
        if (shapeIndex == 2)
        {
            //if target needs this circle or a rectangle there
            if ((targetVorderansicht[x, y, 2, 0] != 0) || (targetVorderansicht[x, y, 0, 0] != 0))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// true if the projections from currentProgress show the target, false if not.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="currentProgress"></param>
    /// <returns></returns>
    public bool checkProjections(byte[,,,] target, byte[,,,] currentProgress)
    {
        //passt das target zum currentProgress?
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (((currentProgress[i, j, 0, 0] > 0) || (currentProgress[i, j, 3, 0] > 0)) && (target[i, j, 0, 0] == 0))
                {
                    QuestDebugLogic.instance.log("false weil progress bei " + i + "," + j + ",(0||3),0 größer 0 war, aber 0 sein sollte.");
                    return false;
                }
                if ((currentProgress[i, j, 2, 0] > 0) && (target[i, j, 2, 0] == 0) && target[i, j, 0, 0] == 0)
                {
                    QuestDebugLogic.instance.log("false weil progress bei " + i + "," + j + ",2,0 größer 0 war, aber 0 sein sollte.");
                    return false;
                }
                for (int k = 0; k < 4; k++)
                {
                    if ((currentProgress[i, j, 1, k] > 0) && (target[i, j, 1, k] == 0) && target[i,j,0,0]==0)
                    {
                        QuestDebugLogic.instance.log("false weil progress bei " + i + "," + j + ",1," + k + " größer 0 war, aber 0 sein sollte und auch kein Quadrat dort gefordert ist.");
                        return false;
                    }
                }

            }

        }
    

        //passt der currentProgress zum target?
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                /*if ((target[i, j, 0, 0] == 0)
                    && (target[i, j, 1, 0] == 0)
                    && (target[i, j, 1, 1] == 0)
                    && (target[i, j, 1, 2] == 0)
                    && (target[i, j, 1, 3] == 0)
                    && (target[i, j, 2, 0] == 0)
                    )
                {
                    if ((currentProgress[i, j, 0, 0] != 0)
                        || (currentProgress[i, j, 1, 0] != 0)
                        || (currentProgress[i, j, 1, 1] != 0)
                        || (currentProgress[i, j, 1, 2] != 0)
                        || (currentProgress[i, j, 1, 3] != 0)
                        || (currentProgress[i, j, 2, 0] != 0)
                        )
                    {
                        QuestDebugLogic.instance.log("false weil progress bei "+i+","+j+",x,x ungleich 0 war, aber 0 sein sollte.");
                        return false;
                    }
                }
                else*/
                    for (int k = 0; k < 3; k++)
                    {
                        //rectangle
                        if (k == 0)
                        {
                            if (
                                (target[i, j, k, 0] > 0)
                                && (currentProgress[i, j, 0, 0] == 0)
                                && ((currentProgress[i, j, 1, 0] == 0) || (currentProgress[i, j, 1, 2] == 0))
                                && ((currentProgress[i, j, 1, 1] == 0) || (currentProgress[i, j, 1, 3] == 0))
                                )
                            {
                                QuestDebugLogic.instance.log("false weil progress bei " + i + "," + j + ",0,0 = 0 war und auch nicht bei zwei dazu passenden Dreiecken >0, aber >0 sein sollte.");
                                return false;
                            }
                        }
                        else
                        //triangles 
                        if (k == 1)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                if (
                                    (target[i, j, k, l] > 0)
                                    && ((currentProgress[i, j, k, l] == 0)
                                        //|| (currentProgress[i, j, 0, 0] > 0)
                                        //|| (currentProgress[i, j, 2, 0] > 0)
                                        //|| (currentProgress[i, j, k, (l + 1) % 4] > 0)
                                        //|| (currentProgress[i, j, k, (l + 2) % 4] > 0)
                                        //|| (currentProgress[i, j, k, (l + 3) % 4] > 0)
                                        )
                                    )
                                {
                                    QuestDebugLogic.instance.log("false weil progress bei " + i + "," + j +","+k+","+l+ " = "+ currentProgress[i, j, k, l]+" war oder QUATSCH bei quadrat/gedrehtem Dreieck >0, aber target dort =" + target[i, j, k, l] + "  ist.");
                                    return false;
                                }
                            }
                        }
                        else
                        //circles
                        if (k == 2)
                        {
                            if (
                                (target[i, j, 2, 0] > 0)
                                && ((currentProgress[i, j, 2, 0] == 0)
                                    //|| (currentProgress[i, j, 1, 0] > 0)
                                    //|| (currentProgress[i, j, 1, 1] > 0)
                                    //|| (currentProgress[i, j, 1, 2] > 0)
                                    //|| (currentProgress[i, j, 1, 3] > 0)
                                    //|| (currentProgress[i, j, 0, 0] > 0)
                                    )
                                )
                            {
                                QuestDebugLogic.instance.log("false weil progress bei " + i + "," + j + ",2,0 = 0 war oder QUATSCH bei Quadrat/Dreieck dort >0, aber >0 sein sollte.");
                                return false;
                            }
                        }
                        else
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                if (target[i, j, k, l] != currentProgress[i, j, k, l])
                                {
                                    QuestDebugLogic.instance.log("false weil index zu weit geht (k war nicht 0, 1 oder 2)");
                                    return false;
                                }
                            }
                        }
                    }
            }
        }
        return true;
    }

    public bool checkVictory()
    {
        string msg="";
        bool aufsicht = false;
        bool seitenansicht = false;
        bool vorderansicht = false;
        try
        {
            aufsicht = checkProjections(targetAufsicht, currentProgressAufsicht);
        }
        catch(Exception e)
        {
            
            msg += "bug in aufsicht" + e.Message +" innerMessage " + " stack trace " + e.StackTrace;
        }
        try
        {
            seitenansicht = checkProjections(targetSeitenansicht, currentProgressSeitenansicht);
        }
        catch (Exception e)
        {
            msg += "bug in seitenansicht" + e.Message;
        }
        try
        {
            vorderansicht = checkProjections(targetVorderansicht, currentProgressVorderansicht);
        }
        catch (Exception e)
        {
            msg += "bug in vorderansicht" + e.Message;
        }
        QuestDebugLogic.instance.logL(msg);
        if (seitenansicht && vorderansicht && aufsicht)
        {
            return true;
        }
        return false;
    }


    private void resetGame()
    {
        //make sure the initial shadowProjection, which is copied during the game, won't get deleted for further rounds.
        snapZonesGame.GetComponent<SnapZoneGenerator>().snapZone.GetComponent<ShadowThrower>().shadowProjection.GetComponent<dontDeleteMe>().dontDelete = true;

        foreach (Transform snapZoneTransform in snapZonesGame.transform)
        {
            if(!snapZoneTransform.name.Equals("Interactions.SnapZone 0.0.0")&&!snapZoneTransform.gameObject.GetComponent<dontDeleteMe>().dontDelete)
            {
                Destroy(snapZoneTransform.gameObject);
            }
        }

        //new snapZone copies shall be deleted in further rounds on reset
        snapZonesGame.GetComponent<SnapZoneGenerator>().snapZone.GetComponent<dontDeleteMe>().dontDelete = false;
        //Destroy(snapZonesGame);
        foreach (GameObject shadowProjection in GameObject.FindGameObjectsWithTag("ShadowProjection"))
        {
            if (!shadowProjection.GetComponent<dontDeleteMe>().dontDelete) {
                Destroy(shadowProjection);
            }

        }
        snapZonesGame.GetComponent<SnapZoneGenerator>().snapZone.GetComponent<ShadowThrower>().shadowProjection.GetComponent<dontDeleteMe>().dontDelete = false;

        targetAufsicht = new byte[4, 4, 3, 4];
        targetSeitenansicht = new byte[4, 4, 3, 4];
        targetVorderansicht = new byte[4, 4, 3, 4];
        currentProgressAufsicht = new byte[4, 4, 4, 4];
        currentProgressSeitenansicht = new byte[4, 4, 4, 4];
        currentProgressVorderansicht = new byte[4, 4, 4, 4];
    }
}

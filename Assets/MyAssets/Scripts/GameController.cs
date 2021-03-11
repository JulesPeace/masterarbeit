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
    public GameObject debugRemoveMeLater;


    public static GameController instance;

    ///<summary>1st and 2nd digit are coordinates, 3rd digit is shape (rectangle, triangle, circle, 2-triangle-rectangle), 4th digit is rotation (0, 90, 180, 270) </summary>
    private byte[,,,] targetAufsicht = new byte[4, 4, 3, 4];
    ///<summary>1st and 2nd digit are coordinates, 3rd digit is shape (rectangle, triangle, circle, 2-triangle-rectangle), 4th digit is rotation (0, 90, 180, 270) </summary>
    private byte[,,,] currentProgressAufsicht = new byte[4, 4, 4, 4];
    ///<summary>1st and 2nd digit are coordinates, 3rd digit is shape (rectangle, triangle, circle, 2-triangle-rectangle), 4th digit is rotation (0, 90, 180, 270) </summary>
    private byte[,,,] targetSeitenansicht = new byte[4, 4, 3, 4];
    ///<summary>1st and 2nd digit are coordinates, 3rd digit is shape (rectangle, triangle, circle, 2-triangle-rectangle), 4th digit is rotation (0, 90, 180, 270) </summary>
    //TODO make this private
    public byte[,,,] currentProgressSeitenansicht = new byte[4, 4, 4, 4];
    ///<summary>1st and 2nd digit are coordinates, 3rd digit is shape (rectangle, triangle, circle, 2-triangle-rectangle), 4th digit is rotation (0, 90, 180, 270) </summary>
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
        szenario1();
        //playArea[0,2,1].GetComponent<ShadowThrower>().project(debugRemoveMeLater);
        Debug.Log("targetSeitenansicht ist "+arrayToString(targetSeitenansicht));
        Debug.Log("currentProgressSeitenansicht ist " + arrayToString(currentProgressSeitenansicht));
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
        //string msg="";
        if (shape.ToLower().Equals("rectangle"))
        {
            //msg+=("before "+x+", "+y+", "+shape+", "+rotation+":\n"+ currentProgressSeitenansicht[x, y, shapeStringToIndex(shape), 0]);
            //QuestDebugLogic.instance.log(msg);
            currentProgressSeitenansicht[x, y, 0, 0] -= 1;
        }
        if (shape.ToLower().Equals("triangle"))
        {
           // msg += ("before " + x + ", " + y + ", " + shape + ", " + rotation + ":\n" + currentProgressSeitenansicht[x, y, 1, rotation / 90]);
            //QuestDebugLogic.instance.log(msg);
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
            //msg += ("before " + x + ", " + y + ", " + shape + ", " + rotation + ":\n" + currentProgressSeitenansicht[x, y, 2, 0]);
            //QuestDebugLogic.instance.log(msg);
            currentProgressSeitenansicht[x, y, 2, 0] -= 1;
        }
        QuestDebugLogic.instance.logL("Input: " + x + "," + y + "," + shape + ", " + rotation + "0; currentProgressSeitenansicht ist" + GameController.instance.arrayToString(GameController.instance.currentProgressSeitenansicht));
        //msg += ("\nafter " + x + ", " + y + ", " + shape + ", " + rotation + ":\n" + currentProgressSeitenansicht[x, y, shapeStringToIndex(shape), 0]);
        //QuestDebugLogic.instance.log(msg);
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
        QuestDebugLogic.instance.logR("Input: " + x + "," + y + "," + shape + ", " + rotation + "0; currentProgressVorderansicht ist" + GameController.instance.arrayToString(GameController.instance.currentProgressVorderansicht));
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
        QuestDebugLogic.instance.log("Input: " + x + "," + y + "," + shape + ", " + rotation + "0; currentProgressAufsicht ist" + GameController.instance.arrayToString(GameController.instance.currentProgressAufsicht));
    }

    public void insertProgressSeitenansicht(int x, int y, string shape, int rotation)
    {
        Debug.Log(x + "," + y + "," + shape + "," + rotation);
        if (shape.ToLower().Equals("rectangle"))
        {
            currentProgressSeitenansicht[x, y, 0, 0] += 1;
            Debug.Log("daraus folgt currentPorgressSeietenansicht ist dort"+ currentProgressSeitenansicht[x, y, 0, 0]);
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
        QuestDebugLogic.instance.logL("Input: " + x + "," + y + "," + shape + ", " + rotation + "0; currentProgressSeitenansicht ist" + GameController.instance.arrayToString(GameController.instance.currentProgressSeitenansicht));
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
        QuestDebugLogic.instance.logR("Input: " + x + "," + y + "," + shape + ", " + rotation + "0; currentProgressVorderansicht ist" + GameController.instance.arrayToString(GameController.instance.currentProgressVorderansicht));
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
                    //Debug.Log("Creating SnapZone at ["+i+","+j+","+k+"]");
                    playArea[x, y, z] = snapZonesGame.GetComponent<SnapZoneGenerator>().generateSnapZone();
                    GameObject current = playArea[x, y, z];
                    current.transform.localPosition = new Vector3(x, y, z);
                    //Generate the target projections
                    for (int m = 0; m < 3; m++)
                    {
                        if (targetAufsicht[z, x, m, y] > 0)
                        {
                            playArea[x, y, z].GetComponent<ShadowThrower>().createTargetProjection("aufsicht", z, x, m, y);
                            playArea[x, y, z].GetComponent<ShadowThrower>().createTargetProjection("gameArea", z, x, m, y);
                        }
                        if (targetSeitenansicht[z, x, m, y] > 0)
                        {
                            playArea[x, y, z].GetComponent<ShadowThrower>().createTargetProjection("seitenansicht", z, x, m, y);
                        }
                        if (targetVorderansicht[z, x, m, y] > 0)
                        {
                            playArea[x, y, z].GetComponent<ShadowThrower>().createTargetProjection("vorderansicht", z, x, m, y);
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
    }

    public void activateSnapZone(GameObject snapZone)
    {
        snapZone.SetActive(true);
        //TODO checkVictory
        /*if (checkVictory())
        {
            QuestDebugLogic.instance.log("Geschafft! alpha 0.6 komplettiert!");
        }*/
    }

    public void deactivateSnapZone(GameObject snapZone)
    {
        //snapZone.GetComponent<ShadowThrower>().destroyProjection();
        snapZone.GetComponentInChildren<SnapZoneConfigurator>().Unsnap();
        snapZone.SetActive(false);
        //TODO checkVictory
        /*if (checkVictory())
        {
            QuestDebugLogic.instance.log("Geschafft! alpha 0.6 komplettiert!");
        }*/
    }

    /// <summary>
    /// returns if a neighbour of a given snapzone is active or none of them are.
    /// </summary>
    /// <param name="index">The index of the Snapzone in playArea</param>
    /// <returns></returns>
    private byte neighboursSnapped(int[] index)
    {//TODO SnapZones deactivaten nicht mehr. Warum?
        byte count = 0;
        if (index[0] != 0)
        {
            if (playArea[index[0] - 1, index[1], index[2]].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped"))
            {
                count+=1;
            }
        }
        if (index[0] < length - 1)
        {
            if (playArea[index[0] + 1, index[1], index[2]].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped"))
            {
                count+=1;
            }
        }
        if (index[1] != 0)
        {
            if (playArea[index[0], index[1] - 1, index[2]].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped"))
            {
                count+=1;
            }
        }
        if (index[1] < height - 1)
        {
            if (playArea[index[0], index[1] + 1, index[2]].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped"))
            {
                count+=1;
            }
        }
        if (index[2] != 0)
        {
            if (playArea[index[0], index[1], index[2] - 1].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped"))
            {
                count += 1;
            }
        }
        if (index[2] < width - 1)
        {
            if (playArea[index[0], index[1], index[2] + 1].GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped"))
            {
                count += 1;
            }
        }
        return count;
    }

    /*private byte isNeighbourSnapped(GameObject snapzone)
    {
        return neighboursSnapped(getIndexOfSnapZoneInPlayArea(snapzone));
    }*/

    ///true if activating, false if deactivating
    public void activateDeactivateNeighbours(GameObject snapZone, bool activate)
    {
        int[] index = getIndexOfSnapZoneInPlayArea(snapZone);
        //TODO remove debug
        /*string msg = "Neighbour counts sind: (";
        try
        {
            msg += neighboursSnapped(new int[] { index[0] - 1, index[1], index[2] })+", ";
        }
        catch
        {
            msg += "x=0, ";
        }
        try
        {
            msg += neighboursSnapped(new int[] { index[0] + 1, index[1], index[2] })+", ";
        }
        catch
        {
            msg += "x=max, ";
        }
        try
        {
            msg += neighboursSnapped(new int[] { index[0], index[1]-1, index[2] }) + ", ";
        }
        catch
        {
            msg += "y=0, ";
        }
        try
        {
            msg += neighboursSnapped(new int[] { index[0], index[1]+1, index[2] }) + ", ";
        }
        catch
        {
            msg += "y=max, ";
        }
        try
        {
            msg += neighboursSnapped(new int[] { index[0], index[1], index[2]-1 }) + ", ";
        }
        catch
        {
            msg += "z=0, ";
        }
        try
        {
            msg += neighboursSnapped(new int[] { index[0], index[1], index[2]+1 }) + ", ";
        }
        catch
        {
            msg += "z=max)";
        }
        QuestDebugLogic.instance.log(msg);*/
        if (index[0] != 0)
        {
            if (activate)
            {
                activateSnapZone(playArea[index[0] - 1, index[1], index[2]]);
            }
            else
            {
                if (index[1] != 0 && (neighboursSnapped(new int[] { index[0]-1,index[1],index[2] })<2)&& !playArea[index[0] - 1, index[1], index[2]].GetComponent<GameObjectActivator>().origin)
                {
                    deactivateSnapZone(playArea[index[0] - 1, index[1], index[2]]);
                    //QuestDebugLogic.instance.log("x-1 nachbar deaktiveirt");
                }
            }
        }
        if (index[0] != length - 1)
        {
            if (activate)
            {
                activateSnapZone(playArea[index[0] + 1, index[1], index[2]]);
            }
            else
            {
                if ((index[1] != 0) && (neighboursSnapped(new int[] { index[0] + 1, index[1], index[2] })<2)&&!playArea[index[0] + 1, index[1], index[2]].GetComponent<GameObjectActivator>().origin)
                {
                    deactivateSnapZone(playArea[index[0] + 1, index[1], index[2]]);
                }
            }
        }
        //TODO Die Snapzone darunter muss ggf. aktiviert/deaktiviert werden!
        //Die Snapzone darunter muss weder aktiviert noch deaktiviert werden!
        /*if (index[1] != 0)
        {
            if (activate)
            {
                activateSnapZone(playArea[index[0], index[1]-1, index[2]]);
            }
            else
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
            else
            {
                if((neighboursSnapped(new int[] { index[0], index[1] + 1, index[2] })<2)&& !playArea[index[0], index[1]+1, index[2]].GetComponent<GameObjectActivator>().origin)
                {
                    deactivateSnapZone(playArea[index[0], index[1] + 1, index[2]]);
                }
            }
        }
        if (index[2] != 0)
        {
            if (activate)
            {
                activateSnapZone(playArea[index[0], index[1], index[2] - 1]);
            }
            else
            {
                if ((index[1] != 0) && (neighboursSnapped(new int[] { index[0], index[1], index[2] - 1 })<2) && !playArea[index[0], index[1], index[2]-1].GetComponent<GameObjectActivator>().origin)
                {
                    deactivateSnapZone(playArea[index[0], index[1], index[2] - 1]);
                }
            }
        }
        if (index[2] != width - 1)
        {
            if (activate)
            {
                activateSnapZone(playArea[index[0], index[1], index[2] + 1]);
            }
            else
            {
                if ((index[1] != 0) && (neighboursSnapped(new int[] { index[0], index[1], index[2] + 1 })<2) && !playArea[index[0], index[1], index[2]+1].GetComponent<GameObjectActivator>().origin)
                {
                    deactivateSnapZone(playArea[index[0], index[1], index[2] + 1]);
                }
            }
        }
        if (checkVictory())
        {
            /*VisualEffect vfx = new VisualEffect();
            vfx.SendEvent("victory");
            vfx.SendEvent("OnPlay");*/
            QuestDebugLogic.instance.log("geschafft! Alpha 0.7 komplettiert!");
            StartCoroutine(fireworks());
        }
    }

    private IEnumerator fireworks()
    {
        QuestDebugLogic.instance.log("Coroutine gestartet");
        VisualEffect vfx = new VisualEffect();
        try
        {
            vfx.SendEvent("victory");
            QuestDebugLogic.instance.log("vfx event victory geladen");
            vfx.SendEvent("OnPlay");
            QuestDebugLogic.instance.log(QuestDebugLogic.logTextM.text+"\nvfx event OnPlay geladen");
        }
        catch(Exception e)
        {
            QuestDebugLogic.instance.log("Fehler beim vfx event: "+e.Message);
        }
        vfx.SendEvent("victory");
        yield return new WaitForSeconds(.8f);
        vfx.SendEvent("victory");
        yield return new WaitForSeconds(.8f);
        vfx.SendEvent("victory");
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
    public bool checkProjections(byte[,,,]target, byte[,,,]currentProgress)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if ((target[i, j, 0, 0] == 0)
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
                else
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
                                        || (currentProgress[i, j, 0, 0] > 0)
                                        || (currentProgress[i, j, 2, 0] > 0)
                                        || (currentProgress[i, j, k, (l + 1) % 4] > 0)
                                        || (currentProgress[i, j, k, (l + 2) % 4] > 0)
                                        || (currentProgress[i, j, k, (l + 3) % 4] > 0)
                                        )
                                    )
                                {
                                    QuestDebugLogic.instance.log("false weil progress bei " + i + "," + j +","+k+","+l+ " = 0 war oder bei quadrat/gedrehtem Dreieck >0, aber target dort >0 ist.");
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
                                    || (currentProgress[i, j, 1, 0] > 0)
                                    || (currentProgress[i, j, 1, 1] > 0)
                                    || (currentProgress[i, j, 1, 2] > 0)
                                    || (currentProgress[i, j, 1, 3] > 0)
                                    || (currentProgress[i, j, 0, 0] > 0))
                                    )
                            {
                                QuestDebugLogic.instance.log("false weil progress bei " + i + "," + j + ",2,0 = 0 war oder bei Quadrat/Dreieck dort >0, aber >0 sein sollte.");
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
        //TODO a lot. compare all projections, check if rectangle meets 2 triangles, rectangle or 2 triangles > 1 circle
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
            msg += "bug in aufsicht" + e.Message;
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
        //QuestDebugLogic.instance.log(/*QuestDebugLogic.logTextM.text*/arrayToString(targetAufsicht)+msg+"\nAufsicht korrekt: "+aufsicht);
        QuestDebugLogic.instance.logL(/*QuestDebugLogic.logTextL.text+=*/arrayToString(targetSeitenansicht)+"\nSeitenansicht korrekt: " +seitenansicht);
        QuestDebugLogic.instance.logR(/*QuestDebugLogic.logTextR.text +=*/arrayToString(targetVorderansicht)+ "\nVorderansicht korrekt: " + vorderansicht);
        if (seitenansicht && vorderansicht && aufsicht)
        {
            return true;
        }
        return false;

        /*if(checkProjections(targetAufsicht,currentProgressAufsicht)
            &&checkProjections(targetSeitenansicht,currentProgressSeitenansicht)
            &&checkProjections(targetVorderansicht,currentProgressVorderansicht)
            )
        {
            return true;
        }
        return false;*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void resetGame()
    {
        foreach(Transform snapZoneTransform in snapZonesGame.transform)
        {
            if(!snapZoneTransform.name.Equals("Interactions.SnapZone 0.0.0"))
            {
                Destroy(snapZoneTransform.gameObject);
            }
        }
        //Destroy(snapZonesGame);
        foreach(GameObject shadowProjection in GameObject.FindGameObjectsWithTag("ShadowProjection"))
        {
            Destroy(shadowProjection);
        }
        targetAufsicht = new byte[4, 4, 3, 4];
        targetSeitenansicht = new byte[4, 4, 3, 4];
        targetVorderansicht = new byte[4, 4, 3, 4];
        currentProgressAufsicht = new byte[4, 4, 3, 4];
        currentProgressSeitenansicht = new byte[4, 4, 3, 4];
        currentProgressVorderansicht = new byte[4, 4, 3, 4];
    }
}

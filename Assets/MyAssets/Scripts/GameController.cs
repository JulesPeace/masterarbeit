using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tilia.Interactions.SnapZone;

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
    private byte[,,,] currentProgressAufsicht = new byte[4, 4, 3, 4];
    ///<summary>1st and 2nd digit are coordinates, 3rd digit is shape (rectangle, triangle, circle, 2-triangle-rectangle), 4th digit is rotation (0, 90, 180, 270) </summary>
    private byte[,,,] targetSeitenansicht = new byte[4, 4, 3, 4];
    ///<summary>1st and 2nd digit are coordinates, 3rd digit is shape (rectangle, triangle, circle, 2-triangle-rectangle), 4th digit is rotation (0, 90, 180, 270) </summary>
    private byte[,,,] currentProgressSeitenansicht = new byte[4, 4, 3, 4];
    ///<summary>1st and 2nd digit are coordinates, 3rd digit is shape (rectangle, triangle, circle, 2-triangle-rectangle), 4th digit is rotation (0, 90, 180, 270) </summary>
    private byte[,,,] targetVorderansicht = new byte[4, 4, 3, 4];
    ///<summary>1st and 2nd digit are coordinates, 3rd digit is shape (rectangle, triangle, circle, 2-triangle-rectangle), 4th digit is rotation (0, 90, 180, 270) </summary>
    private byte[,,,] currentProgressVorderansicht = new byte[4, 4, 3, 4];

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        szenario1();
        playArea[0,2,1].GetComponent<ShadowThrower>().project(debugRemoveMeLater);
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
                currentProgressAufsicht[x, y, 0, 0] -= 1;
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
                currentProgressSeitenansicht[x, y, 0, 0] -= 1;
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
                currentProgressVorderansicht[x, y, 0, 0] -= 1;
            }
        }
        if (shape.ToLower().Equals("circle"))
        {
            currentProgressVorderansicht[x, y, 2, 0] -= 1;
        }
    }

    public void insertProgessAufsicht(int x, int y, string shape, int rotation)
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

    public void insertProgessSeitenansicht(int x, int y, string shape, int rotation)
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
    }

    public void insertProgessVorderansicht(int x, int y, string shape, int rotation)
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
        snapZone.GetComponent<ShadowThrower>().destroyProjection();
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
        QuestDebugLogic.instance.log("currentProgressSeitenansicht ist"+arrayToString(currentProgressSeitenansicht));
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
                if (index[1] != 0 && (neighboursSnapped(new int[] { index[0]-1,index[1],index[2] })<2))
                {
                    deactivateSnapZone(playArea[index[0] - 1, index[1], index[2]]);                   
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
                if ((index[1] != 0) && (neighboursSnapped(new int[] { index[0] + 1, index[1], index[2] })<2))
                {
                    deactivateSnapZone(playArea[index[0] + 1, index[1], index[2]]);
                }
            }
        }
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
                if(neighboursSnapped(new int[] { index[0], index[1] + 1, index[2] })<2)
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
                if ((index[1] != 0) && (neighboursSnapped(new int[] { index[0], index[1], index[2] - 1 })<2))
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
                if ((index[1] != 0) && (neighboursSnapped(new int[] { index[0], index[1], index[2] + 1 })<2))
                {
                    deactivateSnapZone(playArea[index[0], index[1], index[2] + 1]);
                }
            }
        }
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


    bool checkVictory()
    {
        //TODO a lot. compare all projections, check if rectangle meets 2 triangles, rectangle or 2 triangles > 1 circle
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    //rectangle
                    if (k == 0)
                    {
                        if (
                            (targetAufsicht[i, j, 0, 0] > 0)
                            && (currentProgressAufsicht[i, j, 0, 0] == 0)
                            && ((currentProgressAufsicht[i, j, 1, 1] == 0) || currentProgressAufsicht[i, j, 1, 1] == 0)
                            && ((currentProgressAufsicht[i, j, 1, 2] == 0) || currentProgressAufsicht[i, j, 1, 4] == 0)
                            )
                        {
                            return false;
                        }
                    }
                    //triangles 
                    if (k == 1)
                    {
                        for (int l = 0; l < 4; l++)
                        {
                            if (
                                (targetAufsicht[i, j, k, l] > 0)
                                && ((currentProgressAufsicht[i, j, k, l] == 0)
                                    || (currentProgressAufsicht[i, j, 0, 0] > 0)
                                    || (currentProgressAufsicht[i, j, 2, 0] > 0)
                                    || (currentProgressAufsicht[i, j, k, (l + 1) % 4] > 0)
                                    || (currentProgressAufsicht[i, j, k, (l + 2) % 4] > 0)
                                    || (currentProgressAufsicht[i, j, k, (l + 3) % 4] > 0)
                                    )

                                )
                            {
                                return false;
                            }
                        }
                    }
                    //circles
                    if (k == 2)
                    {
                        if (
                            (targetAufsicht[i, j, 2, 0] > 0)
                            && (currentProgressAufsicht[i, j, 2, 0] == 0)
                                || (currentProgressAufsicht[i, j, 1, 0] > 0)
                                || (currentProgressAufsicht[i, j, 1, 1] > 0)
                                || (currentProgressAufsicht[i, j, 1, 2] > 0)
                                || (currentProgressAufsicht[i, j, 1, 3] > 0)
                                || (currentProgressAufsicht[i, j, 0, 0] > 0)
                            )
                        {
                            return false;
                        }
                    }
                    else
                    {
                        for (int l = 0; l < 4; l++)
                        {
                            if (targetAufsicht[i, j, k, l] != currentProgressAufsicht[i, j, k, l])
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }
        return true;
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

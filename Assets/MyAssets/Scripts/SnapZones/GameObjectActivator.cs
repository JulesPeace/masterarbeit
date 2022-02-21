using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tilia.Interactions.SnapZone;
using System;
public class GameObjectActivator : MonoBehaviour
{
    public SnapZoneConfigurator configurator;
    public GameObject toBeActivated;
    /// <summary>
    /// variable used to make sure, that a neighbour, that is being deactivated by this.gameObject.snapzonefacade.unsnap(), doesn't deactivate this snapZone in return
    /// </summary>
    public bool origin=false;
    public static bool globalOrigin=false;
    //public SnapZoneFacade facade;
    // Start is called before the first frame update


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activate()
    {
        try
        { 
            GameController.instance.activateDeactivateNeighbours(this.gameObject, true);
        }
        catch (Exception e)
        {
            QuestDebugLogic.instance.log("Fehler bei deactivate(): " + e.Message);
        }
        /*if (toBeActivated != null)
        {
            toBeActivated.SetActive(true);
        }*/
    }

    public void deactivate()
    {
        try
        {
            if (!globalOrigin)
            {
                globalOrigin = true;
                origin = true;
            }
            GameController.instance.activateDeactivateNeighbours(this.gameObject, false);
            if (origin)
            {
                origin = false;
                globalOrigin = false;
            }
        }
        catch(Exception e)
        {
            QuestDebugLogic.instance.log("Fehler bei deactivate(): "+e.Message);
        }
        /*
        try
        {
            QuestDebugLogic.instance.log("deactivate auf "+configurator.transform.parent.name);
        }
        catch { QuestDebugLogic.instance.log("Snapreferenz scheint null zu sein!"); }
        try
        {
            toBeActivated.GetComponent<ShadowThrower>().destroyProjection();
        }
        catch(Exception e)
        {
            QuestDebugLogic.instance.log("toBeActivated von "+this.name+" ist null!");
            Debug.Log(toBeActivated.name + " hat keinen ShadowThrower! " + e.Message);
            QuestDebugLogic.instance.log(toBeActivated.name + " hat keinen ShadowThrower! " + e.Message);
        }
        if (toBeActivated != null)
        {
            configurator.Unsnap();
            toBeActivated.SetActive(false);
        }*/
    }
}

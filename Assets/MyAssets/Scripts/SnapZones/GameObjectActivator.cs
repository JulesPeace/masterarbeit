using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tilia.Interactions.SnapZone;
public class GameObjectActivator : MonoBehaviour
{
    public SnapZoneConfigurator configurator;
    public GameObject toBeActivated;
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
        if (toBeActivated != null)
        {
            toBeActivated.SetActive(true);
        }
    }

    public void deactivate()
    {
        try
        {
            QuestDebugLogic.instance.log("deactivate auf "+configurator.transform.parent.name);
        }
        catch { QuestDebugLogic.instance.log("Snapreferenz scheint null zu sein!"); }
        string msg = "toBeActivated null?";
        toBeActivated.GetComponent<ShadowThrower>().destroyProjection();
        if (toBeActivated != null)
        {
            msg += " nein";
            configurator.Unsnap();
            msg += ", unsnapped";
            toBeActivated.SetActive(false);
            msg += ", "+toBeActivated.name+" deaktiviert";
            QuestDebugLogic.instance.log(msg);
        }
    }
}

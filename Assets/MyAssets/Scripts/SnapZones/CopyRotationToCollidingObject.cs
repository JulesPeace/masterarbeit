using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tilia.Interactions.SnapZone;

public class CopyRotationToCollidingObject : MonoBehaviour
{

    private float RoundMe(float r)
    {
        if (r <= 45)
        {
            return 0;
        }
        else if(r<=135)
        {
            return 90;
        }
        else if (r <= 225)
        {
            return 180;
        }
        else if (r <= 315)
        {
            return 270;
        }
        else
        {
            return 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected");
        Debug.Log("Objektname des kollidierten Objekts ist " + other.name + " und transformname ist " + other.transform.name + " und parentname ist " + other.transform.parent.name + ".");
        //Debug.Log("this.tag ist "+ this.tag);
        //Debug.Log("Tag des transforms ist "+ other.transform.tag);
        //Debug.Log("tag des parents des transforms des ANDEREN Colliders ist " + other.transform.parent.tag);
        QuestDebugLogic.instance.log("other: "+other.transform.parent.tag);
        try
        {
            Debug.Log("tag des parents des transforms des EIGENEN Colliders ist " + this.transform.parent.tag);
        }
        catch(System.Exception e)
        {
            Debug.Log("tag des parents des transforms des EIGENEN Colliders: nicht möglich! Wie fange ich hier die Exception ab?");
            Debug.Log("Exception lautet: " + e.Message);
        }
        try
        {
            Debug.Log("Collider in self " + this.gameObject.GetComponent<Collider>());
        }
        catch
        {
            Debug.Log(this.gameObject.tag+" kann eigenen Collider nicht finden!");
        }
        //if (!other.CompareTag("Interactable") && !other.gameObject.CompareTag("Interactable"))
        if(other.transform.parent.CompareTag("SnapZone"))
        {
            //Debug.Log("SnapZoneState is " + other.transform.parent.gameObject.GetComponent<SnapZoneFacade>().ZoneState.ToString());
            QuestDebugLogic.instance.log("SnapZoneState is " + other.transform.parent.gameObject.GetComponent<SnapZoneFacade>().ZoneState.ToString());
            if (!other.transform.parent.gameObject.GetComponent<SnapZoneFacade>().ZoneState.ToString().Equals("ZoneIsSnapped"))
            {
                Quaternion rot = new Quaternion();
                rot.w = this.gameObject.transform.rotation.w;
                rot.x = this.gameObject.transform.rotation.x;
                rot.y = this.gameObject.transform.rotation.y;
                rot.z = this.gameObject.transform.rotation.z;
                Vector3 angles = rot.eulerAngles;
                angles.x = RoundMe(angles.x);
                angles.y = RoundMe(angles.y);
                angles.z = RoundMe(angles.z);
                rot = Quaternion.Euler(angles);
                //Transform trans = new T;
                other.gameObject.transform.parent.gameObject.transform.rotation = rot;
                //Debug.Log("Rotation des mit dem Objekt kollidierten Objekts ist: " + other.gameObject.transform.parent.gameObject.transform.rotation.eulerAngles);
                /*       Debug.Log("Collision detected");
                       Debug.Log("Interactable rotation ist "+ this.gameObject.transform.rotation.ToString());
                       Debug.Log("SnapZone rotation ist "+ other.gameObject.transform.parent.gameObject.transform.rotation.ToString());
                       Debug.Log("Das auslösende Objekt ist " + this.gameObject.name + ", das kollidierende Objekt ist " + other.gameObject.transform.parent.gameObject.name);*/
                //QuestDebugLogic.instance.log("Gedrehtes Object ist: " + other.transform.parent.tag);
                //QuestDebugLogic.instance.log("SnapZoneState is " + other.transform.parent.gameObject.GetComponent<SnapZoneFacade>().ZoneState+" gedreht!");
            }
        }
    }
}

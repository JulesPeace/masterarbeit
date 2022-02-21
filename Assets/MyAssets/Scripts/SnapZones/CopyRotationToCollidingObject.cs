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

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.parent.CompareTag("SnapZone"))
        {
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
                //TODO parent.gameObject.transform zirkel?
                other.gameObject.transform.parent.gameObject.transform.rotation = rot;
                MeshFilter fltr = other.gameObject.transform.parent.gameObject.GetComponentInChildren<MeshFilter>();
                if (this.GetComponent<MeshFilter>() != null)
                {
                    fltr.mesh = this.GetComponent<MeshFilter>().mesh;
                    fltr.mesh.RecalculateNormals();
                    fltr.mesh.RecalculateBounds();
                    fltr.mesh.RecalculateTangents();
                }
            }
        }
    }
}

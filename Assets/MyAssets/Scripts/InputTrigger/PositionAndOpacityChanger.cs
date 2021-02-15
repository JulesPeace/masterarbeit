using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAndOpacityChanger : MonoBehaviour
{
    public Material target;
    public Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
             //float alphaValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
            float alphaValue = Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger");
            //float decrease = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);
            float decrease = Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger");
            Color color = target.color;
            color.a = alphaValue;
            target.color = color;

            Vector3 trans = transform.position;
 /*           trans.x += alphaValue / 2;
            trans.y += alphaValue / 2;
            trans.z += alphaValue / 2;*/
            trans.x -= decrease / 8;
            trans.y -= decrease / 8;
            trans.z -= decrease / 8;
            transform.position = trans;
    }

}

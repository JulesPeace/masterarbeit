using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*Quaternion rot = new Quaternion();
        rot.w = this.transform.rotation.w;
        rot.x = this.transform.rotation.x;
        rot.y = this.transform.rotation.y;
        rot.z = this.transform.rotation.z;
        Vector3 angles = rot.eulerAngles;*/
        QuestDebugLogic.instance.log("Rotation: "+this.transform.rotation.eulerAngles.ToString());
    }
}

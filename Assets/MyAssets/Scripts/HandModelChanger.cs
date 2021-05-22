using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandModelChanger : MonoBehaviour
{
    private GameObject handLeft;
    private GameObject handRight;
    public Material mat;

    //
    void Update()
    {
        // find and grab the hand objects
        handRight = GameObject.Find("hand_right_renderPart_0");
        handLeft = GameObject.Find("hand_left_renderPart_0");

        // if i've found the hands change the texture
        if (handRight != null && handLeft != null)
        {
            QuestDebugLogic.instance.log("Hände gefunden!");
            handLeft.GetComponent<Renderer>().material = mat;
            handRight.GetComponent<Renderer>().material = mat;
            Destroy(GetComponent<HandModelChanger>()); // remove this script so it stops running.
        }
    }
}

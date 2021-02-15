using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexTriggerReader : MonoBehaviour
{

    protected bool rightIndexTriggerPressed;
    protected bool leftIndexTriggerPressed;

    protected float rightIndexTriggerFloatValue;
    protected float leftIndexTriggerFloatValue;
    public float pressedLimit = 0.4f;

    // Update is called once per frame
    public bool rightIndexTriggerIsPressed()
    {
        return rightIndexTriggerPressed;
    }

    public bool leftIndexTriggerIsPressed()
    {
        return leftIndexTriggerPressed;
    }

    public float getLeftIndexTriggerFloatValue()
    {
        return leftIndexTriggerFloatValue;
    }

    public float getRightIndexTriggerFloatValue()
    {
        return rightIndexTriggerFloatValue;
    }

    void Update()
    {
        leftIndexTriggerFloatValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        rightIndexTriggerFloatValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        if (leftIndexTriggerFloatValue > pressedLimit)
        {
            leftIndexTriggerPressed = true;
        }
        else
        {
            leftIndexTriggerPressed = false;
        }
        if (rightIndexTriggerFloatValue > pressedLimit)
        {
            rightIndexTriggerPressed = true;
        }
        else
        {
            rightIndexTriggerPressed = false;
        }

    }
}

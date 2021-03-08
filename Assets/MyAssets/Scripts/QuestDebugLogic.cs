using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestDebugLogic : MonoBehaviour
{
    public static QuestDebugLogic instance;
    private bool inMenu;
    public static Text logTextM;
    public static Text logTextL;
    public static Text logTextR;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        RectTransform rtL = DebugUIBuilder.instance.AddLabel("DebugL",2);
        RectTransform rt = DebugUIBuilder.instance.AddLabel("Debug");
        RectTransform rtR = DebugUIBuilder.instance.AddLabel("DebugR",1);
        logTextM = rt.GetComponent<Text>();
        logTextL = rtL.GetComponent<Text>();
        logTextR = rtR.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Start))
        {
            if (inMenu) DebugUIBuilder.instance.Hide();
            else DebugUIBuilder.instance.Show();
            inMenu = !inMenu;
        }
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            logTextM.text = "";
        }
    }

    public void log(string message)
    {
        logTextM.text = message;
    }

    public void logL(string message)
    {
        logTextL.text = message;
    }

    public void logR(string message)
    {
        logTextR.text = message;
    }
}

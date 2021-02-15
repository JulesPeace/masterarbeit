using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabGeneratorToHand : MonoBehaviour
{
    public GameObject rightHandAnchor;
    public int limit;
    [Header("Spawn Location")]
    public bool rightHand = true;
    
    public GameObject myPrefab;
    GameObject[] instances;

    private void Start()
    {
        instances = new GameObject[limit];
    }
    public void generatePrefab()
    {
        for (int i = 0; i <= limit - 1; i++)
            if (instances[i] == null)
            {
                instances[i] = Instantiate(myPrefab, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), Quaternion.identity);
                break;
            }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapZoneGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject snapZone;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject generateSnapZone()
    {
        return Instantiate(snapZone, this.transform);
    }
}

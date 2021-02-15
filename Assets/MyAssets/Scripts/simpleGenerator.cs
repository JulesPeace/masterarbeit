using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleGenerator : MonoBehaviour
{
    public GameObject myPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //generatePrefab();
        //InvokeRepeating("generatePrefab", 5, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void generatePrefab()
    {
        Instantiate(myPrefab, new Vector3(-0.5f,1f, 0.5f), Quaternion.identity,this.transform.parent);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollidingGameObject : MonoBehaviour
{
    public Collider col;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        Destroy(col.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
     
    }
}

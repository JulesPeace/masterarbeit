using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyColliderOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.GetComponent<MeshCollider>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

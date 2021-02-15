using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tilia.Interactions.SnapZone;

public class SpawnSystem : MonoBehaviour
{
    public SnapZoneFacade snap;
    // Start is called before the first frame update
    public GameObject prefabToCopy;
    public Transform optionalSpawn;
    void Start()
    {
        respawn();
    }

    public void respawn()
    {
        //if(pgen.limit<=pgen.instances.length)
        if (optionalSpawn != null)
        {
            float x = optionalSpawn.position.x;
            float y = optionalSpawn.position.y;
            float z = optionalSpawn.position.z;
            PrefabGenerator.instance.generatePrefab(x,y,z,prefabToCopy);
        }
        //GameObject interactable = PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount-1];
        //interactable.GetComponent<Rigidbody>().MovePosition(snap.transform.position);
        //interactable.GetComponent<Rigidbody>().isKinematic = false;
        //interactable.GetComponent<Rigidbody>().useGravity = true;
        PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount - 1].transform.parent=this.transform;
        snap.Snap(PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount - 1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

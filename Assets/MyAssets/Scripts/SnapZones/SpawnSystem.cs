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
        //TODO: DAS WIEDER EINFÜGEN!
        if (optionalSpawn != null)
        {
            float x = optionalSpawn.position.x;
            float y = optionalSpawn.position.y;
            float z = optionalSpawn.position.z;
            /*snap.Snap*/
            PrefabGenerator.instance.generatePrefab(this.transform,prefabToCopy);
            PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount - 1].GetComponent<Rigidbody>().isKinematic = true;
            snap.Snap(PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount-1]);
            PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount - 1].GetComponent<Rigidbody>().isKinematic = false;
        }
        //GameObject interactable = PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount-1];
        //interactable.GetComponent<Rigidbody>().MovePosition(snap.transform.position);
        //interactable.GetComponent<Rigidbody>().isKinematic = false;
        //interactable.GetComponent<Rigidbody>().useGravity = true;
        //snap.Snap(PrefabGenerator.instance.generatePrefab(this.transform, prefabToCopy));

        /* TODO RÜCKGÄNGIG MACHEN! Das hier statt der oberen Zeile.
        * PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount - 1].transform.parent=this.transform;
        * snap.Snap(PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount - 1]);*/
    }

    /// <summary>
    /// This method sets the kinematic variable of the unsnapped object's rigidbody to false again. This must happen after the object is snapped, but after the animation of snapping ends and before the object is grabed again.
    /// </summary>
    /// <param name="newlySpawnedObject"></param>
    public IEnumerator kinematicWorkAround(GameObject newlySpawnedObject)
    {
        yield return new WaitForSeconds(0.41f);
        newlySpawnedObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void respawn(GameObject unsnappedObject)
    {
        //if(pgen.limit<=pgen.instances.length)
        //TODO: DAS WIEDER EINFÜGEN!
        if (optionalSpawn != null)
        {
            float x = optionalSpawn.position.x;
            float y = optionalSpawn.position.y;
            float z = optionalSpawn.position.z;
            /*snap.Snap*/
            PrefabGenerator.instance.generatePrefab(this.transform,prefabToCopy);
            PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount - 1].GetComponent<Rigidbody>().isKinematic = true;
            snap.Snap(PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount-1]);
            //StartCoroutine("kinematicWorkAround", PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount - 1]);
            PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount - 1].GetComponent<Rigidbody>().isKinematic = false;
            //unsnappedObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        //GameObject interactable = PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount-1];
        //interactable.GetComponent<Rigidbody>().MovePosition(snap.transform.position);
        //interactable.GetComponent<Rigidbody>().isKinematic = false;
        //interactable.GetComponent<Rigidbody>().useGravity = true;
        //snap.Snap(PrefabGenerator.instance.generatePrefab(this.transform, prefabToCopy));

        /* TODO RÜCKGÄNGIG MACHEN! Das hier statt der oberen Zeile.
        * PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount - 1].transform.parent=this.transform;
        * snap.Snap(PrefabGenerator.instance.instances[PrefabGenerator.instance.instanceCount - 1]);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

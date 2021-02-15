using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabGenerator : MonoBehaviour
{
    public int limit=200;
    [Header("Spawn Location")]
    public float x=0f;
    public float y=0f;
    public float z=0f;
    // public float nextSpawnHeightDif;
    [Tooltip("A Transform, which position (world space) is copied to the new instance of the prefab.")]
    public Transform optionalSpawn;

    public GameObject wedgePrefab;
    public GameObject cubePrefab;
    public GameObject prismPrefab;
    [HideInInspector]
    public GameObject[] instances;
    [HideInInspector]
    public int instanceCount = 0;

    public static PrefabGenerator instance;

    private void Awake()
    {
        instance = this;
        Debug.Log("Awake des Prefabgeneratorskripts");
        this.instances = new GameObject[this.limit];
    }

    private void Start()
    {
        Debug.Log("Start des Prefabgeneratorskripts, limit ist "+this.limit);
    /*    generatePrefab();
        instances[0].transform.localScale=new Vector3(0.08f, 0.08f, 0.08f);*/
    }

    public static PrefabGenerator getInstance()
    {
        return instance;
    }

    public GameObject getLatestInstance()
    {
        int i=0;
        GameObject target=null;
        while(instances[i] != null && (i<limit))
        {
            i++;
        }
        target = instances[i-1];
        return target;
    }

    public void generatePrefab(float x, float y, float z, GameObject prefab)
    {
        try
        {
            Debug.Log(this.instances[0]);
         /*   if (this.instances == null)
            {
                this.instances = new GameObject[this.limit];
            }*/
        }
        catch
        {
            this.instances = new GameObject[this.limit];
        }
        Debug.Log("Durchlauf " + this.instanceCount + " mit prefab " + prefab.name + ".");
        /*int j=0;
        while (this.instances[j] != null)
        {
            //Debug.Log("instances["+j+"] ist "+this.instances[j]);
            j++;
        }*/
        if (this.instances[this.instanceCount] == null)
        {
            if (wedgePrefab.name.Equals(prefab.name))
            {
                instances[instanceCount] = Instantiate(wedgePrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
            }
            if (cubePrefab.name.Equals(prefab.name))
            {
                instances[instanceCount] = Instantiate(cubePrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
            }
            if (prismPrefab.name.Equals(prefab.name))
            {
                instances[instanceCount] = Instantiate(prismPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
            }
            /*Debug.Log(instances[0]);
            Debug.Log(instances[1]);
            Debug.Log(instances[2]);*/
            // Rigidbody a = instances[i].GetComponent(typeof(Rigidbody)) as Rigidbody;
            // a.useGravity = true;
            //Debug.Log(a.useGravity);
            this.instances[instanceCount].GetComponent<Rigidbody>().isKinematic = true;
            //Debug.Log(instances[instanceCount].GetComponent<Rigidbody>());
            this.instances[instanceCount].transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
            this.instances[instanceCount].GetComponent<Rigidbody>().isKinematic = false;
            Debug.Log("Typname ist " + instances[instanceCount].GetType() + ", LIES DAS!!!!!! Name ist "+instances[instanceCount].name);
            instanceCount++;
            Debug.Log("Ende des generatePrefab von "+prefab.name+": "+this.instances.ToString());
            //Debug.Log(this.instances[0]);
            //QuestDebugLogic.instance.log(this.instances.ToString());
        }
    }

    public void generatePrefab(GameObject prefab)
    {
        if (optionalSpawn != null)
        {
            x = optionalSpawn.position.x;
            y = optionalSpawn.position.y;
            z = optionalSpawn.position.z;
        }
        //Debug.Log("Durchlauf "+instanceCount+" mit prefab "+prefab.name+".");
        //Debug.Log(instances);
        //Debug.Log(instances[0]);
        //Debug.Log(instances[1]);
        //Debug.Log(instances[2]);
        if (instances[instanceCount] == null)
        {
            if (wedgePrefab.name.Equals(prefab.name))
            {
                instances[instanceCount] = Instantiate(wedgePrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
            }
            if (cubePrefab.name.Equals(prefab.name))
            {
                instances[instanceCount] = Instantiate(cubePrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
            }
            if (prismPrefab.name.Equals(prefab.name))
            {
                instances[instanceCount] = Instantiate(prismPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
            }
            // Rigidbody a = instances[i].GetComponent(typeof(Rigidbody)) as Rigidbody;
            // a.useGravity = true;
            //Debug.Log(a.useGravity);
            instances[instanceCount].GetComponent<Rigidbody>().isKinematic = true;
            Debug.Log(instances[instanceCount].GetComponent<Rigidbody>());
            instances[instanceCount].transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
            instanceCount++;
        }
    }
}

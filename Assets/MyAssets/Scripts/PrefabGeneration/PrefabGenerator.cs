﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabGenerator : MonoBehaviour
{
    public int limit = 200;
    [Header("Spawn Location")]
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
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
        this.instances = new GameObject[this.limit];
    }

    private void Start()
    {
    }

    public static PrefabGenerator getInstance()
    {
        return instance;
    }

    public GameObject getLatestInstance()
    {
        int i = 0;
        GameObject target = null;
        while (instances[i] != null && (i < limit))
        {
            i++;
        }
        target = instances[i - 1];
        return target;
    }

    public void generatePrefab(Transform parentTransform, GameObject prefab)
    {
        if (wedgePrefab.name.Equals(prefab.name))
        {
            instances[instanceCount] = Instantiate(wedgePrefab, parentTransform, false);
        }
        else
        if (cubePrefab.name.Equals(prefab.name))
        {
            instances[instanceCount] = Instantiate(cubePrefab, parentTransform, false);
        }
        else
        if (prismPrefab.name.Equals(prefab.name))
        {
            instances[instanceCount] = Instantiate(prismPrefab, parentTransform, false);
        }
        else
        {
            Debug.Log("Interactable prefabs nicht erkannt");
        }
        this.instances[instanceCount].GetComponent<Rigidbody>().isKinematic = true;
        this.instances[instanceCount].transform.localPosition = new Vector3(0, 0, 0);
        this.instances[instanceCount].transform.localRotation = Quaternion.identity;
        this.instances[instanceCount].GetComponent<Rigidbody>().isKinematic = false;
        instanceCount++;
    }

    public GameObject generatePrefab(float x, float y, float z, GameObject prefab)
    {
        try
        {
            Debug.Log(this.instances[0]);
        }
        catch
        {
            this.instances = new GameObject[this.limit];
        }
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
            this.instances[instanceCount].GetComponent<Rigidbody>().isKinematic = true;
            this.instances[instanceCount].transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
            this.instances[instanceCount].GetComponent<Rigidbody>().isKinematic = false;
            instanceCount++;
        }
        return instances[instanceCount - 1];
    }

    public void generatePrefab(GameObject prefab)
    {
        if (optionalSpawn != null)
        {
            x = optionalSpawn.position.x;
            y = optionalSpawn.position.y;
            z = optionalSpawn.position.z;
        }
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
            instances[instanceCount].GetComponent<Rigidbody>().isKinematic = true;
            instances[instanceCount].transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
            instanceCount++;
        }
    }
}

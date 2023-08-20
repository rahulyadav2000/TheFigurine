using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public GameObject wandererPrefab;

    public int poolSize = 12;   // number of objects to pre-allocate in the pool

    public List<GameObject> pooledObj;  // list for the object to be pooled in the game

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        pooledObj= new List<GameObject>();

        for(int i= 0; i < poolSize; i++)
        {
            // instantiate the wanderer enemy, keep them inactive and add them to the object pool list
            GameObject go = (GameObject)Instantiate(wandererPrefab);
            go.SetActive(false);
            pooledObj.Add(go);
        }
    }

    public GameObject GetPooledObj()
    {
        foreach(GameObject g in pooledObj)
        {
            // Check if the wanderer enemy is not currently active in the scene
            if (!g.activeInHierarchy)
            {
                return g;
            }
        }
        return null; // return null if no wanderer enemy is found
    }

}

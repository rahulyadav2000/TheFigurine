using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public GameObject wandererPrefab;

    public int poolSize = 12;

    public List<GameObject> pooledObj;

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
            GameObject go = (GameObject)Instantiate(wandererPrefab);
            go.SetActive(false);
            pooledObj.Add(go);
        }
    }

    public GameObject GetPooledObj()
    {
        foreach(GameObject g in pooledObj)
        {
            if (!g.activeInHierarchy)
            {
                return g;
            }
        }
        return null;
    }

}

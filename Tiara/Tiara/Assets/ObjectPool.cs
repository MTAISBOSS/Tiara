using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] private int amountToPool = 20;
    [SerializeField] private GameObject poolGameObject;

    private List<GameObject> _pooledObjects = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(poolGameObject);
            obj.SetActive(false);
            _pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledGameObject()
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                return _pooledObjects[i];
            }
        }

        return null;
    }
}
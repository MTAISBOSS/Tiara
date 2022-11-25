using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class MirrorGates : MonoBehaviour
{
    [SerializeField] private List<LevelInfo> _levelInfos = new List<LevelInfo>();
    public static List<PeaLogic> _peaGameObjects = new List<PeaLogic>();

    private int _level;
    private float _spawnSpeed;
    private int _numberOfSpawns;
    private float _timeOfAppearing;
    private float _distanceToDecoys;
    private float _timeOfDisappearing;
    private float _timeBetweenSpawn;
    private bool _isOnStart;
    private void Awake()
    {
        _level = 0;
        
        Initialize();
        
    }

    private void Update()
    {
        SpawnManager();
    }

    private void Initialize()
    {
        var currentLevelInfo = _levelInfos[_level];
        _spawnSpeed = currentLevelInfo.spawnSpeed;
        _numberOfSpawns = currentLevelInfo.numberOfSpawns;
        _timeOfAppearing = currentLevelInfo.timeOfAppearing;
        _distanceToDecoys = currentLevelInfo.distanceToDecoys;
        _timeOfDisappearing = currentLevelInfo.timeOfDisappearing;
        
        _timeBetweenSpawn = _spawnSpeed;
        _isOnStart = true;


    }

    private void SpawnManager()
    {
        if (_timeBetweenSpawn > 0)
        {
            _timeBetweenSpawn -= Time.deltaTime;
        }
        else
        {
            _timeBetweenSpawn = _spawnSpeed;

            
                Spawn(transform);
            
            foreach (var p in _peaGameObjects.ToList())
            {
                Spawn(p.transform);
            }
            
        }
    }

    private void Spawn(Transform _transform)
    {
        for (int i = 0; i < _numberOfSpawns; i++)
        {
            _distanceToDecoys *= -1;
            PeaLogic p = ObjectPool.Instance.GetPooledGameObject().GetComponent<PeaLogic>();

            if (p != null)
            {
                p.distanceToDecoys = _distanceToDecoys;
                p.timeOfAppearing = _timeOfAppearing;
                p.timeOfDisappearing = _timeOfDisappearing;
                p.transform.position = _transform.position;
                p.transform.parent = transform;
                p.speed = Random.Range(0.5f, 1.5f);
                p.gameObject.SetActive(true);
                _peaGameObjects.Add(p);
            }
        }

        if (_isOnStart)
        {
            _peaGameObjects[0].isMainPea = true;
        }
        _isOnStart = false;
    }
}

[System.Serializable]
public class LevelInfo
{
    public int level;
    [Header("Speed and Amount")] [Space] public float spawnSpeed;
    public int numberOfSpawns;
    [Header("Situation")] [Space] public float distanceToDecoys;
    [Header("Disappear")] [Space] public float timeOfDisappearing;
    [Header("Appear")] [Space] public float timeOfAppearing;
}
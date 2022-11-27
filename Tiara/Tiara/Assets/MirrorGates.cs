using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class MirrorGates : MonoBehaviour
{
    public static MirrorGates Instance;
    [SerializeField] private List<LevelInfo> _levelInfos = new List<LevelInfo>();
    [SerializeField] private List<Sprite> _sprites = new List<Sprite>();
    public static List<PeaLogic> _peaGameObjects = new List<PeaLogic>();

    [HideInInspector]public int _level;
    private float _spawnSpeed;
    private int _numberOfSpawns;
    private float _timeOfAppearing;
    private float _distanceToDecoys;
    private float _timeOfDisappearing;
    private float _timeBetweenSpawn;
    private bool _isOnStart;
    private int currentSpawnNumber;

    private void Awake()
    {
        Instance = this;
        _level = 0;
        Initialize();
    }
    
    private void LateUpdate()
    {
        SpawnManager();
    }

    public void Initialize()
    {
        if (_level >= _levelInfos.Count)
        {
            Debug.Log("Levels Are Empty");
            return;
        }
        var currentLevelInfo = _levelInfos[_level];
        _spawnSpeed = currentLevelInfo.spawnSpeed;
        _numberOfSpawns = currentLevelInfo.numberOfSpawns;
        _timeOfAppearing = currentLevelInfo.timeOfAppearing;
        _timeOfDisappearing = (_numberOfSpawns*_spawnSpeed) + currentLevelInfo.timeOfDisappearing;

        _timeBetweenSpawn = _spawnSpeed;
        _isOnStart = true;
        currentSpawnNumber = 0;
        Invoke(nameof(StopAllActions), _timeOfDisappearing);
    }

    private void StopAllActions()
    {
        foreach (var peaGameObject in _peaGameObjects)
        {
            peaGameObject.StopAllActions();
        }
    }

    private void SpawnManager()
    {
        if (currentSpawnNumber >= _numberOfSpawns)
        {
            return;
        }

        if (_timeBetweenSpawn > 0)
        {
            _timeBetweenSpawn -= Time.deltaTime;
        }
        else
        {
            _timeBetweenSpawn = _spawnSpeed;
            currentSpawnNumber++;

            if (_peaGameObjects.Count == 0)
            {
                Spawn(transform);
            }
            else
            {
                foreach (var p in _peaGameObjects.ToList())
                {
                    Spawn(p.transform);
                }
            }
        }
    }

    private void Spawn(Transform _transform)
    {
        int index;
        index = _peaGameObjects.Count == 0 ? 1 : 2;

        for (int i = 0; i < index; i++)
        {
            PeaLogic p = ObjectPool.Instance.GetPooledGameObject().GetComponent<PeaLogic>();

            if (p != null)
            {
                p.transform.parent = transform;
                p.transform.position = _transform.position;
                p.maxDistanceToMainPea = _distanceToDecoys;
                p.speed = Random.Range(1f, 1.5f);
                p.GetComponent<SpriteRenderer>().sprite = _sprites[Random.Range(0, _sprites.Count)];
                p.gameObject.SetActive(true);
                p.transform.DOScale(0, 0);
                p.transform.DOScale(1, _timeOfAppearing);
                _peaGameObjects.Add(p);
            }
        }

        if (_isOnStart)
        {
            _peaGameObjects[0].isMainPea = true;
            _isOnStart = false;

        }

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
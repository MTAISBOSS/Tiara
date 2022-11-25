using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PeaLogic : MonoBehaviour
{
    [SerializeField] private float speedOfWave;
    [SerializeField] private float maxDistance;
    public float speed;
    public float distanceToDecoys;
    public float timeOfAppearing;
    public float timeOfDisappearing;
    [HideInInspector] public float rate = 0;
    public bool isMainPea;
    private Vector3 _startPos;

    private void OnEnable()
    {
        _startPos = transform.position;
        rate = 0;
    }

    private void Update()
    {
        transform.position += new Vector3(speed * Time.deltaTime,
            distanceToDecoys * Mathf.Sin(Time.time * speedOfWave) * Time.deltaTime, 0);
        rate = transform.position.x - _startPos.x;
        if (maxDistance <= rate)
        {
            Disable();
        }
    }

    private void OnMouseDown()
    {
        Disable();
    }

    private void Disable()
    {
        MirrorGates._peaGameObjects.Remove(this);
        gameObject.SetActive(false);
    }
}
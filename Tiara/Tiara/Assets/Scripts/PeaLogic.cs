using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using Random = UnityEngine.Random;

public class PeaLogic : MonoBehaviour
{
    [SerializeField] private Sprite OnStopSprite;
    [SerializeField] private float xLim;
    [SerializeField] private float yLim;
    [SerializeField] private Ease moveAnim = Ease.InOutElastic;
    public float speed;
    [HideInInspector] public float maxDistanceToMainPea;
    [HideInInspector] public float rate = 0;
    [HideInInspector] public bool isMainPea;
    private Vector3 _startPos;
    private SpriteRenderer _spr;
    private CircleCollider2D _collider2D;
    private bool _canMove;
    private PeaLogic _mainPea;

    private void OnEnable()
    {
        _spr = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<CircleCollider2D>();
        _collider2D.enabled = false;
        _canMove = true;
        _mainPea = MirrorGates._peaGameObjects.Find(o => o.isMainPea);
        Move();

        rate = 0;
    }

    private void Move()
    {
        var pos = PickRandomPosition(xLim, yLim);
        Rotate(pos);
        transform.DOMove(pos, speed).SetEase(moveAnim).OnComplete(() =>
        {
            if (_canMove)
            {
                if (isMainPea)
                {
                    Move();
                }
                else
                {
                    if (Vector3.Distance(_mainPea.transform.position, transform.position) >= maxDistanceToMainPea)
                    {
                        Move();
                    }

                    else
                    {
                        pos = PickRandomPosition(xLim, yLim);
                        Rotate(pos);
                        Move();
                    }
                }
            }
        });
    }

    private void Rotate(Vector3 vector3)
    {
        var dir = vector3 - transform.position;
        dir = dir.normalized;
        transform.eulerAngles = new Vector3(0, 0, dir.z);
    }

    private Vector3 PickRandomPosition(float xLimit, float yLimit)
    {
        return new Vector3(Random.Range(-xLimit, xLimit), Random.Range(-yLimit, yLimit), 0);
    }

    public void StopAllActions()
    {
        _spr.sprite = OnStopSprite;
        _canMove = false;
        _collider2D.enabled = true;
    }

    private void OnMouseDown()
    {
        Debug.Log(isMainPea ? "Correct" : "Wrong");

        if (isMainPea)
        {
            PlayerScoreManager.Instance.IncreaseScore(2);
        }
      

        MirrorGates.Instance._level++;
        foreach (var peaGameObject in MirrorGates._peaGameObjects)
        {
            peaGameObject.Disable();
        }

        MirrorGates._peaGameObjects.Clear();

        MirrorGates.Instance.Initialize();
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (isMainPea)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, maxDistanceToMainPea);
    }
}
﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NokhodAI : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    public Transform nokhodStandingPlace;
    private Animator _anim;
    private static readonly int IsIdel = Animator.StringToHash("IsIdel");
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void Move(NoteLogic noteLogic, Vector3 standingPos)
    {
        Vector3 target;
        if (noteLogic != null)
        {
            target = noteLogic.transform.position;
        }
        else
        {
            target = standingPos;
        }

        if (target.x > transform.position.x)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if (target.x < transform.position.x)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }


        RunAnim();
        transform.DOMove(target, speed).OnComplete((() =>
        {
            if (standingPos == nokhodStandingPlace.position)
            {
                IdelAnim();
                var player = GameObject.FindGameObjectWithTag("Player").transform.position;
                transform.rotation = player.x > transform.position.x
                    ? new Quaternion(0, 0, 0, 0)
                    : new Quaternion(0, 180, 0, 0);
            }
        }));
    }

    private void RunAnim()
    {
        _anim.SetBool(IsRunning,true);
    }

    private void IdelAnim()
    {
        _anim.SetBool(IsRunning,false);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public enum DoorState
{
    Open,
    Close,
    Default
}


public class NoteLogic : MonoBehaviour
{
    public new string name;
    [HideInInspector] public BoxCollider2D box;
    private Action _onSelectNote;
    public  readonly int DoorOpen = Animator.StringToHash("DoorOpen");
    public  readonly int DoorClose = Animator.StringToHash("DoorClose");

    [HideInInspector] public Animator _anim;
    [HideInInspector]public DoorState _doorState = DoorState.Default;

    private void Start()
    {
        _onSelectNote += GameManager.Instance.CheckNotes;
        box = GetComponent<BoxCollider2D>();
        _anim = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        _doorState = DoorState.Open;
        OnInputNote();
        foreach (var door in GameManager.Instance.doorColliders.Where(o =>
                     o.gameObject.GetComponent<NoteLogic>().name != name))
        {
            door.gameObject.GetComponent<NoteLogic>()._anim.SetTrigger(DoorClose);
            door.gameObject.GetComponent<NoteLogic>()._doorState = DoorState.Close;
        }

        _anim.SetTrigger(DoorOpen);
    }

    private void OnInputNote() //when player selects this door
    {
        GameManager.Instance.userInputNotes.Add(this);
        GameManager.Instance.playerNotes.Add(name);

        _onSelectNote?.Invoke();


        GameManager.Instance.OnChooseDoor();
    }

 
}
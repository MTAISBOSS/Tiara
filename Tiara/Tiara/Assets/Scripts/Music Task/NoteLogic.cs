using System;
using System.Linq;
using UnityEngine;

public class NoteLogic : MonoBehaviour
{
    public new string name;
    [HideInInspector] public BoxCollider2D box;
    private Action _onSelectNote;
    private Animator _anim;
    private static readonly int DoorOpen = Animator.StringToHash("DoorOpen");

    private void Start()
    {
        _onSelectNote += GameManager.Instance.CheckNotes;
        box = GetComponent<BoxCollider2D>();
        _anim = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        OnInputNote();
    }

    private void OnInputNote()//when player selects this door
    {
        GameManager.Instance.userInputNotes.Add(this);
        GameManager.Instance.playerNotes.Add(name);
        
        _onSelectNote?.Invoke();
        
        
        GameManager.Instance.OnChooseDoor();
        
        
    }
    private void OnMouseUp()
    {
        foreach (var doorCollider in GameManager.Instance.doorColliders.Where(doorCollider => doorCollider != box))
        {
            doorCollider.gameObject.GetComponent<Animator>().SetTrigger("DoorClose");
        }

        _anim.SetTrigger(DoorOpen);
    }

}
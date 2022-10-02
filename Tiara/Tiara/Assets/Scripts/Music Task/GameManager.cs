using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public enum PlayerCoOperators
{
    Ghost,
    BlackPea
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector] public List<NoteLogic> userInputNotes = new List<NoteLogic>();
    [HideInInspector] public List<string> playerNotes = new List<string>();//the available notes that we have to check
    List<string> finallNotes = new List<string>();//the given notes that player has to play

    [SerializeField] private List<string> notesSounds = new List<string>();//the sound of each note do re mi fa sol la si
    [SerializeField] private float delayBetweenSounds;
    [SerializeField] private int notesCount;//the number of notes that player has to play
    public Text status;//a simple text to show the notes 
    private PlayerCoOperators _coOperators = PlayerCoOperators.Ghost;//the player co op which is ghost or black pea

    public List<BoxCollider2D> doorColliders = new List<BoxCollider2D>();//here is for the ability to add delay between selecting door by simply disabling the box collider so we can't select the door
    [SerializeField] private float delayTimeBetweenDoors;//the delay time between selecting each door

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateNotes();
    }

    public void OnChooseDoor()
    {
        StartCoroutine(AbilityToChooseDoor());
    }

    IEnumerator AbilityToChooseDoor()//delay between selecting doors
    {
        foreach (var doorCollider in doorColliders)
        {
            doorCollider.enabled = false;
        }

        yield return new WaitForSeconds(delayTimeBetweenDoors);

        foreach (var doorCollider in doorColliders)
        {
            doorCollider.enabled = true;
        }
    }


    void CreateNotes()//generates the notes that the player has to play
    {
        status.text = "Notes :";

        Random coRand = new Random();
        _coOperators = (PlayerCoOperators)coRand.Next(0, Enum.GetNames(typeof(PlayerCoOperators)).Length);

        Random rand = new Random();
        for (int i = 0; i < notesCount; i++)
        {
            int index = rand.Next(0, notesSounds.Count);
            finallNotes.Add(notesSounds[index]);
            status.text += notesSounds[index];
        }
    }

    bool CheckForCorrectNote()//checks that the input note is correct or not if it is returns true else false
    {
        print(nameof(CheckForCorrectNote));
        for (int i = 0; i < playerNotes.Count; i++)
        {
            if (playerNotes[i] != finallNotes[i])
            {
                return false;
            }
        }

        if (playerNotes.Count == finallNotes.Count)
        {
            Win();
        }

        return true;
    }


    public void CheckNotes()//if the note that player has entered was wrong then we call the Lost function
    {
        print(nameof(CheckNotes));

        if (CheckForCorrectNote())
        {
            return;
        }

        Lost();
    }

    private void Lost()//anything after loosing the game
    {
        status.text = "You Lost The Game";
        playerNotes.Clear();
        finallNotes.Clear();
        CreateNotes();
    }

    private void Win()//everything after winning the game
    {
        status.text = "You Won The Game";
        StartCoroutine(PlayNotes(_coOperators));
        playerNotes.Clear();
        finallNotes.Clear();
        CreateNotes();
    }

    private IEnumerator PlayNotes(PlayerCoOperators coOperatorType)//tell us the notes with a delay, useful for playing all notes together after player wins the game 
    {
        if (userInputNotes.Count > 0)
        {
            if (coOperatorType == PlayerCoOperators.BlackPea)
            {
                userInputNotes.Reverse();
            }

            foreach (var note in userInputNotes)
            {
                AudioHolder.Instance.Play(note.name);
                yield return new WaitForSeconds(delayBetweenSounds);
            }

            yield return new WaitForSeconds(1f);

            userInputNotes.Clear();
        }
    }
}
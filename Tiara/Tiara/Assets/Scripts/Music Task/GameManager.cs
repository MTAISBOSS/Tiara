using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public enum PlayerCoOperators
{
    BlackPea
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private List<string> notesSounds = new List<string>();//the sound of each note do re mi fa sol la si
    [SerializeField] private float delayBetweenSounds;
    [SerializeField] private int notesCount;//the number of notes that player has to play
    [SerializeField] private float delayTimeBetweenDoors;//the delay time between selecting each door
    [SerializeField] private float nokhodDelayBtwDoors;
    [SerializeField] private List<int> noteCountPerLevel = new List<int>();
    public int legalMistakes = 3;
    public List<BoxCollider2D> doorColliders = new List<BoxCollider2D>();//here is for the ability to add delay between selecting door by simply disabling the box collider so we can't select the door
    
    
    [HideInInspector] public List<NoteLogic> userInputNotes = new List<NoteLogic>();
    [HideInInspector] public List<string> playerNotes = new List<string>();//the available notes that we have to check
    
    
    private List<string> finallNotes = new List<string>();//the given notes that player has to play
    private List<string> finallNotesReversed;
    private PlayerCoOperators _coOperators = PlayerCoOperators.BlackPea;//the player co op which is ghost or black pea
    private bool _canChoose = true;

    private int _mistakesCount;
    private int currentState = 0;
    
    private void Awake()
    {
        Instance = this;
        PlayerPrefs.SetInt("NotePerLevel",0);
    }

    private void Start()
    {
        currentState = 0;
        CreateNotes();
    }

    public void OnChooseDoor()
    {
        StartCoroutine(AbilityToChooseDoor(_canChoose));
    }

    private IEnumerator AbilityToChooseDoor(bool canChoose)//delay between selecting doors
    {
        if (canChoose == false)
        {
            yield return null;
        }
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


    private void CreateNotes()//generates the notes that the player has to play
    {
        _mistakesCount = 0;
        _coOperators = PlayerCoOperators.BlackPea;
        notesCount = noteCountPerLevel[PlayerPrefs.GetInt("NotePerLevel", 0)];

        Random rand = new Random();
        string finalNote = "";

        int i = 0;
        while (i<notesCount)
        {
            int index = rand.Next(0, notesSounds.Count);
            if (finallNotes.Contains(notesSounds[index]))
            {
                continue;
            }
            i++;
            finallNotes.Add(notesSounds[index]);
            finalNote += notesSounds[index];
           
        }
        
        finallNotes.Reverse();
        finallNotesReversed = new List<string>(finallNotes);
        finallNotes.Reverse();

        StartCoroutine(MoveNokhod());
        Debug.Log($"Try : {finalNote}");
    }

    private IEnumerator MoveNokhod()
    {
        foreach (var doorCollider in doorColliders)
        {
            doorCollider.enabled = false;
        }
        for (int i = 0; i < finallNotesReversed.Count; i++)
        {
            yield return new WaitForSeconds(nokhodDelayBtwDoors);
            var door = doorColliders.Find(o => o.gameObject.GetComponent<NoteLogic>().name == finallNotes[i]).gameObject.GetComponent<NoteLogic>();
            FindObjectOfType<NokhodAI>().Move(door,Vector3.zero);
        }
        yield return new WaitForSeconds(nokhodDelayBtwDoors);
        FindObjectOfType<NokhodAI>().Move(null,FindObjectOfType<NokhodAI>().nokhodStandingPlace.position);
        
        foreach (var doorCollider in doorColliders)
        {
            doorCollider.enabled = true;
        }
    }

    bool CheckForCorrectNote()//checks that the input note is correct or not if it is returns true else false
    {
        for (int i = 0; i < playerNotes.Count; i++)
        {
            if (playerNotes[i] != finallNotesReversed[i])
            {
                return false;
            }
        }

        if (playerNotes.Count == finallNotesReversed.Count)
        {
            StartCoroutine(Win());
        }

        return true;
    }


    public void CheckNotes()//if the note that player has entered was wrong then we call the Lost function
    {
        _mistakesCount++;
        if (CheckForCorrectNote())
        {
            return;
        }

        if (_mistakesCount <= legalMistakes)
        {
            Debug.Log("Try Again ");
            playerNotes.Remove(playerNotes[playerNotes.Count - 1]);
            return;
        }
        StartCoroutine(Lost());
    }

    private IEnumerator Lost()//anything after loosing the game
    {
        Debug.Log("Lost");
        _canChoose = false;
        foreach (var doorCollider in doorColliders)
        {
            doorCollider.enabled = false;
        }
        yield return new WaitForSeconds(1);
        foreach (var doorCollider in doorColliders)
        {
            var door = doorCollider.gameObject.GetComponent<NoteLogic>();
            door._anim.SetTrigger(door.DoorClose);
            door._doorState = DoorState.Close;
        }
        foreach (var doorCollider in doorColliders)
        {
            doorCollider.enabled = true;
        }

        _canChoose = true;
        playerNotes.Clear();
        finallNotes.Clear();
        CreateNotes();
        
    }

    private IEnumerator Win()//everything after winning the game
    {
        Debug.Log("Win");
        PlayerScoreManager.Instance.IncreaseScore(2);

        currentState++;

        if (currentState >= noteCountPerLevel.Count)
        {
            Debug.Log("Level Finished");
            SceneManager.LoadScene("Main");
        }
        _canChoose = false;
        foreach (var doorCollider in doorColliders)
        {
            doorCollider.enabled = false;
        }
        yield return new WaitForSeconds(1);
        foreach (var doorCollider in doorColliders)
        {
            var door = doorCollider.gameObject.GetComponent<NoteLogic>();
            door._anim.SetTrigger(door.DoorClose);
            door._doorState = DoorState.Close;
        }
        foreach (var doorCollider in doorColliders)
        {
            doorCollider.enabled = true;
        }
        StartCoroutine(PlayNotes(_coOperators));
        playerNotes.Clear();
        finallNotes.Clear();
        PlayerPrefs.SetInt("NotePerLevel",PlayerPrefs.GetInt("NotePerLevel") + 1);
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

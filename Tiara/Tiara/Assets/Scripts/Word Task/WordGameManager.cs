using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Word_Task.Farsi;

public class WordGameManager : MonoBehaviour
{
    public static WordGameManager Instance;

    [HideInInspector] public bool mouseDown;
    [HideInInspector] public List<WordData> letters = new List<WordData>();
    
    [SerializeField] private Text resultText;
    public List<string> wordsToCheck = new List<string>();

    private string _result;
    private int _index;

    private void Awake()
    {
        Instance = this;
        ResetChar();
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mouseDown = true;
        }

        if (Input.GetMouseButtonUp(0) && mouseDown)
        {
            CheckStatus();
            ResetChar();
        }
    }
    public void GetChar(string letter)
    {
        _result += letter;
        resultText.text = _result;
    }
    private void ResetChar()
    {
        mouseDown = false;

        if (_result != null && _result == "")
        {
            return;
        }

        _result = "";
        resultText.text = _result;
        letters.Clear();
    }
    private void CheckStatus()
    {
        if (WordIsValid())
        {
            wordsToCheck.RemoveAt(_index);
            _index = 0;
        }
    }
    private bool WordIsValid()
    {
        for (int i = 0; i < wordsToCheck.Count; i++)
        {
            if (wordsToCheck[i] == _result)
            {
                _index = i;
                return true;
            }
        }
        return false;
    }
}

[Serializable]
public class WordData
{
    public string letter;
    public int id;
}
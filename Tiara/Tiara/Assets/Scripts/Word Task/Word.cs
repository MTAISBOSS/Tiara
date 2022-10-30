using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word : MonoBehaviour
{
    [SerializeField] private string letter;
    [SerializeField] private int id;
    [SerializeField] private TextMesh _textMesh;

    private void Awake()
    {
        _textMesh.text = letter;
    }

    private void OnMouseOver()
    {
        if (WordGameManager.Instance.letters.Exists(o => o.id == id) || !WordGameManager.Instance.mouseDown)
        {
            return;
        }

        var l = new WordData() { letter = letter, id = id };
        WordGameManager.Instance.letters.Add(l);
        WordGameManager.Instance.GetChar(l.letter);
    }
}

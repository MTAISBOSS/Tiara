using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterContainer : MonoBehaviour
{

    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private Vector3 initializeOffset;
    [SerializeField] private Sprite containerBoarder;
    [SerializeField] private Font _font;
    [SerializeField] private WordGameManager manager;
    private void Start()
    {
        for (int i = 0; i < manager.wordsToCheck.Count; i++)
        {
            manager.wordContainerDatas[i].word = manager.wordsToCheck[i];
            for (int j = 0; j < manager.wordsToCheck[i].Length; j++)
            {
                GameObject board = new GameObject();
                board.AddComponent<SpriteRenderer>().sprite = containerBoarder;

                GameObject content = new GameObject
                {
                    transform =
                    {
                        parent = board.transform
                    }
                };

                content.AddComponent<TextMesh>().text = manager.wordsToCheck[i][j].ToString();
                content.GetComponent<TextMesh>().font = _font;
                content.GetComponent<TextMesh>().characterSize = 0.1f;
                content.GetComponent<TextMesh>().fontSize = 50;
                content.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
                content.GetComponent<TextMesh>().alignment = TextAlignment.Center;
                
                board.transform.position = new Vector3(initializeOffset.x + xOffset * j, initializeOffset.y + yOffset * i, 0);

                content.gameObject.SetActive(false);
                manager.wordContainerDatas[i].wordContainers.Add(content);

            }
        }
    }
}

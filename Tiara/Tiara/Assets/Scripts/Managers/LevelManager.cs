using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> levelUnlockSprites = new List<Sprite>();
    [SerializeField] private List<GameObject> levelItems = new List<GameObject>();

    private void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("Level", 0);
        //levels start from 1 , so zero means we haven't unlocked first level yet
        for (int i = 0; i < levelItems.Count; i++)
        {
            var buttonComponent = levelItems[i].GetComponent<Button>();
            buttonComponent.interactable = false;
        }

        for (int i = 0; i < currentLevel; i++)
        {
            var imgComponent = levelItems[i].GetComponent<Image>();
            var buttonComponent = levelItems[i].GetComponent<Button>();

            imgComponent.sprite = levelUnlockSprites[i];
            buttonComponent.interactable = true;
            buttonComponent.onClick.RemoveAllListeners();
            var levelIndex = i + 1;
            buttonComponent.onClick.AddListener(() => { LoadLevel(levelIndex); });
        }
    }

    void LoadLevel(int levelIndex)
    {
        //load level
        Debug.Log($"Level {levelIndex} Loaded");
    }
}
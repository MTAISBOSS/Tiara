using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ScoreStatus
{
    Empty = 0,
    Half = 1,
    Full = 2
}

public class ScoreItem : MonoBehaviour
{
    [SerializeField] private Sprite fullScore;
    [SerializeField] private Sprite halfScore;
    [SerializeField] private Sprite emptyScore;

    private Image _scoreImage;

    private void Awake()
    {
        _scoreImage = GetComponent<Image>();
    }

    public void SetScoreImage(ScoreStatus scoreStatus)
    {
        switch (scoreStatus)
        {
            case ScoreStatus.Empty:
                _scoreImage.sprite = emptyScore;
                break;
            case ScoreStatus.Half:
                _scoreImage.sprite = halfScore;
                break;
            case ScoreStatus.Full:
                _scoreImage.sprite = fullScore;
                break;
        }
    }
}
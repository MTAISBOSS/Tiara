using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GameObject scorePrefab;


    private List<ScoreItem> _scoreItems = new List<ScoreItem>();
    private int currentScore = 0;
    private void OnEnable()
    {
        PlayerScoreManager.OnDecreaseScore += DrawScoreItems;
    }
    private void OnDisable()
    {
        PlayerScoreManager.OnDecreaseScore -= DrawScoreItems;
    }

    private void Start()
    {
        DrawScoreItems();
    }

    private void DrawScoreItems()
    {
        ClearScoreItems();
        float remainder = PlayerScoreManager.Instance.maxScore % 2;
        int numberOfScores = (int)((PlayerScoreManager.Instance.maxScore / 2) + remainder);
        for (int i = 0; i < numberOfScores; i++)
        {
            CreateScoreItems();
        }

        for (int i = 0; i < _scoreItems.Count; i++)
        {
            int scoreStatusReminder = (int)(Mathf.Clamp(PlayerScoreManager.Instance.currentScore - (i * 2), 0, 2));
            _scoreItems[i].SetScoreImage((ScoreStatus)scoreStatusReminder);
        }
    }

    private void CreateScoreItems()
    {
        var newScoreItem = Instantiate(scorePrefab, transform, true);
        var scoreComponent = newScoreItem.GetComponent<ScoreItem>();
        scoreComponent.SetScoreImage(ScoreStatus.Empty);
        _scoreItems.Add(scoreComponent);
    }

    private void ClearScoreItems()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        _scoreItems = new List<ScoreItem>();
    }

   
}
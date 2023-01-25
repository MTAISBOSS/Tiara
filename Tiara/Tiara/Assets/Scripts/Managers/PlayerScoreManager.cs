using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
   
   public static Action OnIncreaseScore;
   public static Action OnDecreaseScore;
   public static PlayerScoreManager Instance;

   public int maxScore = 20;
   public int currentScore;
   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      currentScore = 0;
   }

   void DecreaseScore()
   {
      OnDecreaseScore?.Invoke();
      currentScore--;
   }

   void IncreaseScore()
   {
      OnIncreaseScore?.Invoke();
      currentScore++;
   }
}

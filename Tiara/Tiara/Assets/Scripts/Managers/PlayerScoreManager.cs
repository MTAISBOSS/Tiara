using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// attach this script to game manager
public class PlayerScoreManager : MonoBehaviour
{
   public static PlayerScoreManager Instance;

   public static Action OnIncreaseScore;
   public static Action OnDecreaseScore;

   public int maxScore = 20;
   public int currentScore;
   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      currentScore = maxScore/2;
   }

   public void DecreaseScore(int amount)
   {
      currentScore-=amount;
      OnDecreaseScore?.Invoke();
   }

   public void IncreaseScore(int amount)
   {
      currentScore+=amount;
      OnIncreaseScore?.Invoke();
   }
}

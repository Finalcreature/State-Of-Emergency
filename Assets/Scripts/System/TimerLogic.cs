using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class TimerLogic : MonoBehaviour
{
    public event EventHandler OnEndTime;  

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] float setTime;
    float pausedTime;
    
    private void Start()
    {
        SetTimePerDifficulty();
    }

    private void SetTimePerDifficulty()
    {
        if (DifficultyPrefs.difficulty == DifficultyPrefs.Difficulies.Easy)
        {
            setTime = 60;
        }
        else if (DifficultyPrefs.difficulty == DifficultyPrefs.Difficulies.Normal)
        {
            setTime = 90;
        }
        else if (DifficultyPrefs.difficulty == DifficultyPrefs.Difficulies.Hard)
        {
            setTime = 180;
        }
        else
        {
            setTime = 360;
        }
    }

    // Update is called once per frame
    void Update()
    {
         if(setTime >= 0)
        {
            pausedTime = setTime; // Assignment has to be here, otherwise the time will keep on decreasing
            setTime -= Time.deltaTime;
            timer.text = setTime.ToString("F0");   
        }
        else
        {
             OnEndTime?.Invoke(this, EventArgs.Empty);
        }
    }

    void StopTimer()
    {   
        setTime = pausedTime;
    }

  
}

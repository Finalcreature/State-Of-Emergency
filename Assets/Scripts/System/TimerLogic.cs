using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System; //Need for EventHandler
public class TimerLogic : MonoBehaviour
{
    public event EventHandler OnEndTime;  //Send a callback when the timer == zero

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] float setTime;
    
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

    void Update()
    {
        if(setTime >= 0)
        {
            setTime -= Time.deltaTime;
            timer.text = setTime.ToString("F0");  //Let me print the float without the decimals
        }
        else
        {
             OnEndTime?.Invoke(this, EventArgs.Empty); //?.Invoke - check if there are any subscribers
        }
    }

  
}

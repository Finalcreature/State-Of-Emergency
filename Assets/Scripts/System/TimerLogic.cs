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


    // Update is called once per frame
    void Update()
    {
         if(setTime >= 0)
        {
            setTime -= Time.deltaTime;
            timer.text = setTime.ToString("F0");
        }
        else
        {
             OnEndTime?.Invoke(this, EventArgs.Empty);
        }
    }
}

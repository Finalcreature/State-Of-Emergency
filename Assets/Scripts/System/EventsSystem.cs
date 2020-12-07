using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventsSystem : MonoBehaviour
{
    bool isGameStopped;
    TimerLogic timerLogic;
    // Start is called before the first frame update
    void Start()
    {
        timerLogic = GetComponent<TimerLogic>();
        timerLogic.OnEndTime += TimeEnded;
        isGameStopped = false;
    }

    void TimeEnded(object sender, EventArgs e)
    {
        timerLogic.OnEndTime -= TimeEnded;
    }

}

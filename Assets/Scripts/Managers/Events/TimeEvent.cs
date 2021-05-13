using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEvent : MonoBehaviour
{
    private void Start()
    {
        GameEventSystem.current.onSunrise += onSunrise;
        GameEventSystem.current.onDayTime += onDayTime;
        GameEventSystem.current.onSunset += onSunset;
        GameEventSystem.current.onNightTime += onNightTime;
    }

    private void onSunrise()
    {
        gameObject.SendMessage("OnSunrise");
    }

    private void onDayTime()
    {
        gameObject.SendMessage("OnDayTime");
    }

    private void onSunset()
    {
        gameObject.SendMessage("OnSunset");
    }

    private void onNightTime()
    {
        gameObject.SendMessage("OnNightTime");
    }

    public void unregister()
    {
        GameEventSystem.current.onSunrise -= onSunrise;
        GameEventSystem.current.onDayTime -= onDayTime;
        GameEventSystem.current.onSunset -= onSunset;
        GameEventSystem.current.onNightTime -= onNightTime;
    }
}

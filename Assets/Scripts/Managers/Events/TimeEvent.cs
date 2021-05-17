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
        gameObject.SendMessage("OnSunrise", null, SendMessageOptions.DontRequireReceiver);
    }

    private void onDayTime()
    {
        gameObject.SendMessage("OnDayTime", null, SendMessageOptions.DontRequireReceiver);
    }

    private void onSunset()
    {
        gameObject.SendMessage("OnSunset", null, SendMessageOptions.DontRequireReceiver);
    }

    private void onNightTime()
    {
        gameObject.SendMessage("OnNightTime", null, SendMessageOptions.DontRequireReceiver);
    }

    public void unregister()
    {
        GameEventSystem.current.onSunrise -= onSunrise;
        GameEventSystem.current.onDayTime -= onDayTime;
        GameEventSystem.current.onSunset -= onSunset;
        GameEventSystem.current.onNightTime -= onNightTime;
    }
}

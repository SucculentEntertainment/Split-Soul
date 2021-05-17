using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEvent : MonoBehaviour
{
    private void Start()
    {
        GameEventSystem.current.onDebug += onDebug;
    }

    private void onDebug(string debugType)
    {
        gameObject.SendMessage("OnDebug", debugType, SendMessageOptions.DontRequireReceiver);
    }

    public void unregister()
    {
        GameEventSystem.current.onDebug -= onDebug;
    }
}

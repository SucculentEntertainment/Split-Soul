using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActionEvent : MonoBehaviour
{
    private void Start()
    {
        GameEventSystem.current.onUIAction += onUIAction;
    }

    private void onUIAction(string action)
    {
        gameObject.SendMessage("OnUIAction", action, SendMessageOptions.DontRequireReceiver);
    }

    public void unregister()
    {
        GameEventSystem.current.onUIAction -= onUIAction;
    }
}

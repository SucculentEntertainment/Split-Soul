using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveEvent : MonoBehaviour
{
    private void Start()
    {
        GameEventSystem.current.onRevive += onRevive;
    }

    private void onRevive(string id)
    {
        if (id == gameObject.name) gameObject.SendMessage("OnRevive");
    }

    public void unregister()
    {
        GameEventSystem.current.onRevive -= onRevive;
    }
}

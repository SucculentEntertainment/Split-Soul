using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEvent : MonoBehaviour
{
    private void Start()
    {
        GameEventSystem.current.onInteractHighlight += onInteractHighlight;
        GameEventSystem.current.onInteract += onInteract;
    }

    private void onInteract(string id)
    {
        if (id == gameObject.name) gameObject.SendMessage("OnInteract");
    }

    private void onInteractHighlight(string id, bool activate)
    {
        if (id == gameObject.name) gameObject.SendMessage("OnInteractHighlight", activate);
    }
}

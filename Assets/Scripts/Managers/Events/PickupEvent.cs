using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupEvent : MonoBehaviour
{
    private void Start()
    {
        GameEventSystem.current.onPickup += onPickup;
    }

    private void onPickup(string id, Item item)
    {
        if (id == gameObject.name) gameObject.SendMessage("OnPickup", item, SendMessageOptions.DontRequireReceiver);
    }

    public void unregister()
    {
        GameEventSystem.current.onPickup -= onPickup;
    }
}

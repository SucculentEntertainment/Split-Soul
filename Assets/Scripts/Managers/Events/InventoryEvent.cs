using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEvent : MonoBehaviour
{
    private void Start()
    {
        GameEventSystem.current.onInventoryInsert += onInventoryInsert;
    }

    private void onInventoryInsert(Collectable collectable)
    {
        gameObject.SendMessage("OnInventoryInsert", collectable, SendMessageOptions.DontRequireReceiver);
    }

    public void unregister()
    {
        GameEventSystem.current.onInventoryInsert -= onInventoryInsert;
    }
}

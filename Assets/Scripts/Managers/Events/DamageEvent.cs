using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEvent : MonoBehaviour
{
    private void Start()
    {
        GameEventSystem.current.onReceiveDamage += onReceiveDamage;
    }

    private void onReceiveDamage(string id, float damage)
    {
        if (id == gameObject.name) gameObject.SendMessage("OnReceiveDamage", damage);
    }

    public void unregister()
    {
        GameEventSystem.current.onReceiveDamage -= onReceiveDamage;
    }
}

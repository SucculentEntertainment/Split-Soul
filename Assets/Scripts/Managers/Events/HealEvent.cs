using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEvent : MonoBehaviour
{
    private void Start()
    {
        GameEventSystem.current.onReceiveHeal += onReceiveHeal;
    }

    private void onReceiveHeal(string id, float amount)
    {
        if (id == gameObject.name) gameObject.SendMessage("OnReceiveHeal", amount);
    }
}

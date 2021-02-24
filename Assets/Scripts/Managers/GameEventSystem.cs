using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem current;
    
    private void Awake()
    {
        current = this;
    }

    // ================================
    //  Events
    // ================================

    // --------------------------------
    //  Dimension Change
    // --------------------------------
    public event Action<string> onDimensionChange;
    public void DimensionChange(string dimension)
    {
        if (onDimensionChange != null) onDimensionChange(dimension);
    }

    // --------------------------------
    //  Damage Event
    // --------------------------------
    public event Action<string, float> onReceiveDamage;
    public void GiveDamage(string id, float damage)
    {
        if (onReceiveDamage != null) onReceiveDamage(id, damage);
    }
    
    // --------------------------------
    //  Heal Event
    // --------------------------------
    public event Action<string, float> onReceiveHeal;
    public void Heal(string id, float amount)
    {
        if (onReceiveHeal != null) onReceiveHeal(id, amount);
    }

    // --------------------------------
    //  Revive Event
    // --------------------------------
    public event Action<string> onRevive;
    public void Revive(string id)
    {
        if (onRevive != null) onRevive(id);
    }

    // --------------------------------
    //  Pickup Event
    // --------------------------------
    public event Action<string, Item> onPickup;
    public void Pickup(string id, Item item)
    {
        if (onPickup != null) onPickup(id, item);
    }
}

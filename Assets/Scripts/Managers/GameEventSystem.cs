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

    public event Action<string> onDimensionChange;
    public void DimensionChange(string dimension)
    {
        if (onDimensionChange != null) onDimensionChange(dimension);
    }
}

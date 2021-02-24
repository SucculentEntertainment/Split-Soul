using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionEvent : MonoBehaviour
{
    public List<string> dimensions;

    private void Start()
    {
        GameEventSystem.current.onDimensionChange += onDimensionChange;
    }

    private void onDimensionChange(string dimension)
    {
        if (dimensions.Contains(dimension)) gameObject.SendMessage("OnDimensionEnable", dimension);
        else gameObject.SendMessage("OnDimensionDisable", dimension);
    }

    public void unregister()
    {
        GameEventSystem.current.onDimensionChange -= onDimensionChange;
    }
}

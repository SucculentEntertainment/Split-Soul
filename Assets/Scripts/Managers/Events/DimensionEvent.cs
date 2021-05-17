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

    public void changeDimension(string dimension)
    {
        onDimensionChange(dimension);
    }

    private void onDimensionChange(string dimension)
    {
        if (dimensions.Contains(dimension)) gameObject.SendMessage("OnDimensionEnable", dimension, SendMessageOptions.DontRequireReceiver);
        else gameObject.SendMessage("OnDimensionDisable", dimension, SendMessageOptions.DontRequireReceiver);
    }

    public void unregister()
    {
        GameEventSystem.current.onDimensionChange -= onDimensionChange;
    }
}

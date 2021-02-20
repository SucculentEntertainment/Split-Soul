using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionEvent : MonoBehaviour
{
    public string dimension = "alive";

    private void Start()
    {
        GameEventSystem.current.onDimensionChange += onDimensionChange;
    }

    private void onDimensionChange(string dimension)
    {
        if (dimension == this.dimension) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
}

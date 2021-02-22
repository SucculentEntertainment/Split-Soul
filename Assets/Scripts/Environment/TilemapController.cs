using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapController : MonoBehaviour
{
    private void OnDimensionEnable(string dimension)
    {
        foreach (Transform child in transform) { child.gameObject.SetActive(true); }
    }

    private void OnDimensionDisable(string dimension)
    {
        foreach (Transform child in transform) { child.gameObject.SetActive(false); }
    }
}

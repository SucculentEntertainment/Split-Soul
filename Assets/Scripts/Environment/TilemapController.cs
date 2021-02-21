using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapController : MonoBehaviour
{
    public void OnDimensionEnable(string dimension)
    {
        gameObject.SetActive(true);
    }

    public void OnDimensionDisable(string dimension)
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static string _dimension;

    public static string dimension {
        set { GameEventSystem.current.DimensionChange(value); _dimension = value; }
        get { return _dimension; }
    }

    private void Start()
    {
        dimension = "alive";
    }
}

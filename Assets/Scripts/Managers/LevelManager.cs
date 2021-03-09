﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	// ================================
	//  Parameters
	// ================================

	public static LevelManager current;

	public List<string> dimensionInit;
	public static List<string> dimensions;

	public GameObject projectileContainer;

	// ================================
	//  Private values
	// ================================

    private bool firstUpdate = true;

    private static string _dimension;
    public static string dimension {
        set { GameEventSystem.current.DimensionChange(value); _dimension = value; }
        get { return _dimension; }
    }

	// ================================
	//  Functions
	// ================================

	private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        _dimension = "alive";
		dimensions = dimensionInit;
    }

	private void Update()
	{
		if(firstUpdate)
		{
            firstUpdate = false;
            dimension = "alive";
		}
	}
}

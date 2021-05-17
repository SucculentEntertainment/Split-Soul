using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	// ================================
	//  Parameters
	// ================================

	public static LevelManager current;

	// ================================
	//  Values
	// ================================

	[HideInInspector] public int previousLevel = -1;

	// ================================
	//  Private values
	// ================================

	// ================================
	//  Functions
	// ================================

	private void Awake()
    {
        current = this;
    }

	private void OnLevelChange(int targetLevel)
	{
		GameManager.current.loadLevel(targetLevel);
		Debug.Log("Requesting to load level " + targetLevel);
	}
}

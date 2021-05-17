using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	// ================================
	//  Parameters
	// ================================

	public static LevelManager current;
	public Player player;

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

	private void Awake() { activate(); }

	public void activate()
	{
		current = this;

		if(player != null) player.stopMovement();
	}

	// ================================
	//  Events
	// ================================

	private void OnLevelChange(int targetLevel)
	{
		GameManager.current.loadLevel(targetLevel);
	}
}

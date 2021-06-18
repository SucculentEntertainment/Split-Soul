using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public static LevelManager current;

	// ================================
	//  Values
	// ================================

	public GameObject levelTools;
	public Transform spawnPoint;
	public GameObject itemContainer;

	[HideInInspector] public int previousLevel = -1;
	[HideInInspector] public Vector2 previousPosition = Vector2.zero;
	[HideInInspector] public Player player;

	// ================================
	//  Functions
	// ================================

	private void Awake()
	{
		if(levelTools != null) levelTools.SetActive(false);
		activate();
	}

	public void Start()
	{
		previousPosition = spawnPoint.position;
	}

	public void activate()
	{
		current = this;

		if(player != null)
		{
			player.stopMovement();
			player.transform.position = previousPosition;
		}
	}

	// ================================
	//  Events
	// ================================

	public void OnLevelChange(int targetLevel)
	{
		previousPosition = (Vector2) player.transform.position;
		GameManager.current.loadLevel(targetLevel);
	}
}

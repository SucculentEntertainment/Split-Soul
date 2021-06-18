using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitSoul.Core.Level
{
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

		// ================================
		//  Functions
		// ================================

		private void Awake()
		{
			if (levelTools != null) levelTools.SetActive(false);
			activate();
		}

		public void Start()
		{
			previousPosition = spawnPoint.position;
		}

		public void activate()
		{
			current = this;
			GameEventSystem.current.Teleport("Player", previousPosition);
		}

		// ================================
		//  Events
		// ================================

		public void OnLevelChange(int targetLevel)
		{
			previousPosition = (Vector2)GameManager.current.playerPosition;
			GameManager.current.loadLevel(targetLevel);
		}
	}
}

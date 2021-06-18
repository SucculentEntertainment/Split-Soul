using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using SplitSoul.Core.Level;
using SplitSoul.Data.Inventory;
using SplitSoul.Data.Scriptable.Inventory;

namespace SplitSoul.Core
{
	public class GameManager : MonoBehaviour
	{
		// ================================
		//  Parameters
		// ================================

		public static GameManager current;

		public GameObject loadingScreen;
		public GameObject player;
		public Camera.CameraRigManager cameraRig;
		public List<string> dimensions;

		//TODO: Improve to accomodate all containers
		public GameObject cItemContainer;

		private List<int> loadedScenes = new List<int>();
		private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

		// ================================
		//  Global Variables
		// ================================

		[Header("Globals")]
		public int currLevel = -1;
		public string dimension = "alive";

		public List<Item> existingItems;

		[Header("Player Variables")]
		public Vector2 playerPosition = new Vector2(0, 0);
		public bool playerDisableMovement = false;
		public float playerHealth = 0;
		public int playerDeathState = 0;
		public float playerNextAttackTime = 0f;

		public List<ItemSlot> playerInventory;
		public int playerCoins = 0;
		public int playerSouls = 0;

		// ================================
		//  Functions
		// ================================

		private void Awake()
		{
			current = this;
			loadLevel((int)SceneIndecies.TitleScreen);
		}

		public void changeDimension(string dimension)
		{
			this.dimension = dimension;
			GameEventSystem.current.DimensionChange(dimension);
		}

		// ================================
		//  Level Loading
		// ================================

		public void loadLevel(int level)
		{
			loadingScreen.SetActive(true);
			disableAllScenes();

			if (loadedScenes.IndexOf(level) == -1)
			{
				scenesLoading.Add(SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive));
				loadedScenes.Add(level);
			}

			StartCoroutine(GetSceneLoadProgreess(level));
		}

		private void disableAllScenes()
		{
			foreach (int scene in loadedScenes)
			{
				//Disable Master GameObject in every Scene
				SceneManager.GetSceneByBuildIndex(scene).GetRootGameObjects()[0].SetActive(false);
			}
		}

		private void activateLevel(int level)
		{
			GameObject master = SceneManager.GetSceneByBuildIndex(level).GetRootGameObjects()[0];
			LevelManager levelManager = master.GetComponent<LevelManager>();

			if (levelManager != null)
			{
				//Normal Level

				if (!player.activeSelf) player.SetActive(true);
				if (cameraRig.titleScreen) cameraRig.setTitleScreenMode(false);

				levelManager.previousLevel = currLevel;
				levelManager.activate();

				GameEventSystem.current.ThrowUIAction(new Events.UIAction("CTRL_SetNormalMode"));

				cItemContainer = levelManager.itemContainer;
			}
			else
			{
				//Title Screen

				player.SetActive(false);
				cameraRig.setTitleScreenMode(true);
				GameEventSystem.current.ThrowUIAction(new Events.UIAction("CTRL_SetTitleMode"));
			}

			currLevel = level;
			SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(level));

			master.SetActive(true);
		}

		// ================================
		//  Coroutines
		// ================================

		public IEnumerator GetSceneLoadProgreess(int level)
		{
			for (int i = 0; i < scenesLoading.Count; i++)
			{
				while (!scenesLoading[i].isDone) { yield return null; }
			}

			yield return new WaitForSeconds(1);
			activateLevel(level);

			loadingScreen.SetActive(false);
		}
	}
}

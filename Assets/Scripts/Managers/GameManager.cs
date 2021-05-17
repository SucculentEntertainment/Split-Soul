using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
	public GameObject loadingScreen;

	public List<string> dimensions;

	private List<int> loadedScenes = new List<int>();
	private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

	// ================================
	//  Global Variables
	// ================================

	[Header("Globals")]
	public int currLevel = -1;
	public string dimension = "alive";

	// ================================
	//  Functions
	// ================================

    private void Awake()
    {
        current = this;
		loadLevel((int) SceneIndecies.TitleScreen);
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

		if(loadedScenes.IndexOf(level) == -1)
		{
			scenesLoading.Add(SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive));
			loadedScenes.Add(level);
		}

		StartCoroutine(GetSceneLoadProgreess(level));
	}

	private void disableAllScenes()
	{
		foreach(int scene in loadedScenes)
		{
			//Disable Master GameObject in every Scene
			SceneManager.GetSceneByBuildIndex(scene).GetRootGameObjects()[0].SetActive(false);
		}
	}

	private void activateLevel(int level)
	{
		GameObject master = SceneManager.GetSceneByBuildIndex(level).GetRootGameObjects()[0];
		Transform levelManager = master.transform.Find("LevelManager");
		if(levelManager != null)
		{
			levelManager.GetComponent<LevelManager>().previousLevel = currLevel;
			Debug.Log(levelManager.GetComponent<LevelManager>().previousLevel);
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
		for(int i = 0; i < scenesLoading.Count; i++) {
			while(!scenesLoading[i].isDone) { yield return null; }
		}

		yield return new WaitForSeconds(1);

		activateLevel(level);
		loadingScreen.SetActive(false);
	}
}

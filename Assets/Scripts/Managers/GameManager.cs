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

	private int currLevel = -1;
	private List<int> loadedScenes = new List<int>();
	private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    void Awake()
    {
        current = this;
		loadLevel((int) SceneIndecies.TitleScreen);
    }

    public void loadLevel(int level)
	{
		loadingScreen.SetActive(true);

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
		master.Find("LevelManager").GetComponent<LevelManager>().previousLevel = currLevel;
	
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

		disableAllScenes();
		yield return new WaitForSeconds(1);

		activateLevel(level);
		loadingScreen.SetActive(false);
	}

	// ================================
	//  Events
	// ================================

	private void OnLevelChange(int targetLevel)
	{
		loadLevel(targetLevel);
	}
}

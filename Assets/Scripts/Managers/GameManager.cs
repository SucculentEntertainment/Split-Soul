using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
	public GameObject loadingScreen;

	private int currLevel = -1;

    void Awake()
    {
        instance = this;
        loadLevel((int) SceneIndecies.TitleScreen);
    }

	List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void loadLevel(int level)
	{
		loadingScreen.SetActive(true);

		if(currLevel != -1) scenesLoading.Add(SceneManager.UnloadSceneAsync(currLevel));

		currLevel = level;
		scenesLoading.Add(SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive));

		StartCoroutine(GetSceneLoadProgreess());
	}

	// ================================
	//  Coroutines
	// ================================

	public IEnumerator GetSceneLoadProgreess()
	{
		for(int i = 0; i < scenesLoading.Count; i++)
		{
			while(!scenesLoading[i].isDone) { yield return null; }
		}

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

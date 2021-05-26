using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleScreenManager : MonoBehaviour
{
	public GameObject levelTools;

	private void Awake()
	{
		if(levelTools != null) levelTools.SetActive(false);
	}

    private void OnEscape(InputValue val)
    {
		GameEventSystem.current.UIAction("ESC");
    }

	private void OnReturn(InputValue val)
    {
		GameEventSystem.current.UIAction("Enter");
    }

	private void OnLevelChange(int targetLevel)
	{
		GameManager.current.loadLevel(targetLevel);
	}
}

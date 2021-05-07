using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBook : MonoBehaviour
{
    public void OnResumeButton()
	{
		GameEventSystem.current.UIAction("MainResume");
	}

	public void OnInventoryButton()
	{
		GameEventSystem.current.UIAction("MainInventory");
	}

	public void OnStatsButton()
	{
		GameEventSystem.current.UIAction("MainStats");
	}

	public void OnSavesButton()
	{
		GameEventSystem.current.UIAction("MainSaves");
	}

	public void OnSettingsButton()
	{
		GameEventSystem.current.UIAction("MainSettings");
	}

	public void OnExitButton()
	{
		GameEventSystem.current.UIAction("MainExit");
	}
}

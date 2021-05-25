using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBook : MonoBehaviour
{
	public List<GameObject> ingameOnly;
	public List<GameObject> titleOnly;

	public void throwUIActionEvent(string action)
	{
		GameEventSystem.current.UIAction(action);
	}

	public void setTitleScreenMode(bool active)
	{
		foreach(GameObject obj in ingameOnly) obj.SetActive(!active);
		foreach(GameObject obj in titleOnly)  obj.SetActive(active);
	}
}

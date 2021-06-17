using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBook : MonoBehaviour
{
	public List<GameObject> ingameOnly;
	public List<GameObject> titleOnly;

	public void throwUIActionEvent(string action)
	{
		GameEventSystem.current.ThrowUIAction(new UIAction(action));
	}

	public void throwIndexedUIActionEvent(string action, int index = -1)
	{
		GameEventSystem.current.ThrowUIAction(new UIAction(action, index));
	}

	public void setTitleScreenMode(bool active)
	{
		foreach(GameObject obj in ingameOnly) obj.SetActive(!active);
		foreach(GameObject obj in titleOnly)  obj.SetActive(active);
	}
}

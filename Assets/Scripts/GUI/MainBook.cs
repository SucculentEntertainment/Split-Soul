using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBook : MonoBehaviour
{
	public void throwUIActionEvent(string action)
	{
		GameEventSystem.current.UIAction(action);
	}
}

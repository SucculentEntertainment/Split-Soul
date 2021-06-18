using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using SplitSoul.Core.Events;

namespace SplitSoul.Core.Level
{
	public class TitleScreenManager : MonoBehaviour
	{
		public GameObject levelTools;

		private void Awake()
		{
			if (levelTools != null) levelTools.SetActive(false);
		}

		private void OnEscape(InputValue val)
		{
			GameEventSystem.current.ThrowUIAction(new UIAction("ESC"));
		}

		private void OnReturn(InputValue val)
		{
			GameEventSystem.current.ThrowUIAction(new UIAction("Enter"));
		}

		private void OnLevelChange(int targetLevel)
		{
			GameManager.current.loadLevel(targetLevel);
		}
	}
}

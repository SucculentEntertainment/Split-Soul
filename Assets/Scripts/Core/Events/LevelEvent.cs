using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitSoul.Core.Events
{
	public class LevelEvent : MonoBehaviour
	{
		private void Start()
		{
			GameEventSystem.current.onLevelChange += onLevelChange;
		}

		private void onLevelChange(int levelID)
		{
			gameObject.SendMessage("OnLevelChange", levelID, SendMessageOptions.DontRequireReceiver);
		}

		public void unregister()
		{
			GameEventSystem.current.onLevelChange -= onLevelChange;
		}
	}
}

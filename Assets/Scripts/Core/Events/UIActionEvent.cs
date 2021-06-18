using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitSoul.Core.Events
{
	[SerializableAttribute]
	public class UIAction
	{
		public string action;
		public int index;
		public UIAction(string action, int index = -1) { this.action = action; this.index = index; }
	}

	public class UIActionEvent : MonoBehaviour
	{
		private void Start()
		{
			GameEventSystem.current.onUIAction += onUIAction;
		}

		private void onUIAction(UIAction uiAction)
		{
			gameObject.SendMessage("OnUIAction", uiAction, SendMessageOptions.DontRequireReceiver);
		}

		public void unregister()
		{
			GameEventSystem.current.onUIAction -= onUIAction;
		}
	}
}

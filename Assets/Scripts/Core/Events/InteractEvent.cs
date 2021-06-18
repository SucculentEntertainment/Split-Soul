using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitSoul.Core.Events
{
	public class InteractEvent : MonoBehaviour
	{
		private void Start()
		{
			GameEventSystem.current.onInteractHighlight += onInteractHighlight;
			GameEventSystem.current.onInteract += onInteract;
		}

		private void onInteract(string id)
		{
			if (id == gameObject.name) gameObject.SendMessage("OnInteract", null, SendMessageOptions.DontRequireReceiver);
		}

		private void onInteractHighlight(string id, bool activate)
		{
			if (id == gameObject.name) gameObject.SendMessage("OnInteractHighlight", activate, SendMessageOptions.DontRequireReceiver);
		}

		public void unregister()
		{
			GameEventSystem.current.onInteractHighlight -= onInteractHighlight;
			GameEventSystem.current.onInteract -= onInteract;
		}
	}
}

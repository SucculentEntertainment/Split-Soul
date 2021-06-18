using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SplitSoul.Data;

namespace SplitSoul.Core.Events
{
	public class PickupEvent : MonoBehaviour
	{
		private void Start()
		{
			GameEventSystem.current.onPickup += onPickup;
		}

		private void onPickup(string id, Collectable collectable)
		{
			if (id == gameObject.name) gameObject.SendMessage("OnPickup", collectable, SendMessageOptions.DontRequireReceiver);
		}

		public void unregister()
		{
			GameEventSystem.current.onPickup -= onPickup;
		}
	}
}

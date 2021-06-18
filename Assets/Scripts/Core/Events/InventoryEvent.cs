using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SplitSoul.Data;

namespace SplitSoul.Core.Events
{
	public class InventoryEvent : MonoBehaviour
	{
		private void Start()
		{
			GameEventSystem.current.onInventory += onInventory;
		}

		private void onInventory(string action, Collectable collectable)
		{
			if (action == "insert") gameObject.SendMessage("OnInventoryInsert", collectable, SendMessageOptions.DontRequireReceiver);
			if (action == "drop") gameObject.SendMessage("OnInventoryDrop", collectable, SendMessageOptions.DontRequireReceiver);
		}

		public void unregister()
		{
			GameEventSystem.current.onInventory -= onInventory;
		}
	}
}

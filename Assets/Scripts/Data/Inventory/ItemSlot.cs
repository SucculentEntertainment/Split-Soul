using System;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitSoul.Data.Inventory
{
	[SerializableAttribute]
	public class ItemSlot
	{
		public string id;
		public int amount;
		public int slotIndex;

		public ItemSlot(string id, int amount, int slotIndex)
		{
			this.id = id;
			this.amount = amount;
			this.slotIndex = slotIndex;
		}
	}
}

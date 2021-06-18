using System;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SplitSoul.Core;
using SplitSoul.Data;
using SplitSoul.Data.Inventory;
using SplitSoul.Data.Scriptable.Inventory;
using SplitSoul.Entity.Spawner;

namespace SplitSoul.UI.Inventory
{
	//TODO: Remove unneeded conversions from ItemObj to ItemID and back to ItemObj
	public class InventoryController : MonoBehaviour
	{
		public GameObject slotPrefab;
		public Transform slotContainer;
		public ItemSpawner itemSpawner;

		private List<InventorySlot> slots;
		private GameManager gm;

		private void Start()
		{
			gm = GameManager.current;
		}

		public void updateSlot(int index)
		{
			Item item = gm.existingItems.Find(x => x.id == gm.playerInventory[index].id);

			slots[index].setValues(item, gm.playerInventory[index].amount);
			if (gm.playerInventory[index].amount <= 0) gm.playerInventory.RemoveAt(index);
		}

		public int createNewSlot(string id, int amount)
		{
			InventorySlot slot = Instantiate(slotPrefab, slotContainer).GetComponent<InventorySlot>();
			slots.Add(slot);

			int index = slots.Count - 1;
			slots[index].setIndex(index);
			gm.playerInventory.Add(new ItemSlot(id, amount, index));

			return index;
		}

		public void insert(string id, int amount)
		{
			//INFO: Does not accomodate for non stackable items
			if (gm.existingItems.FindIndex(x => x.id == id) == -1) return;
			int slotIndex = gm.playerInventory.FindIndex(x => x.id == id);

			if (slotIndex == -1) { slotIndex = createNewSlot(id, amount); }
			else gm.playerInventory[slotIndex].amount += amount;

			updateSlot(slotIndex);
		}

		private int remove(string id, int amount)
		{
			int slotIndex = gm.playerInventory.FindIndex(x => x.id == id);
			if (slotIndex == -1) return -1;

			if (amount > gm.playerInventory[slotIndex].amount) amount = gm.playerInventory[slotIndex].amount;
			gm.playerInventory[slotIndex].amount -= amount;

			updateSlot(slotIndex);
			return amount;
		}

		public void drop(string id, int amount)
		{
			amount = remove(id, amount);
			itemSpawner.spawnItem(id, amount);
		}

		// ================================
		//  Events
		// ================================

		private void OnInventoryInsert(Collectable collectable)
		{
			if (collectable.id == "item")
			{
				insert(collectable.item.id, collectable.amount);
			}
		}

		private void OnInventoryDrop(Collectable collectable)
		{
			if (collectable.id == "item")
			{
				drop(collectable.item.id, collectable.amount);
			}
		}
	}
}

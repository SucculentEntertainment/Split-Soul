using System;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SplitSoul.Core;
using SplitSoul.Data;
using SplitSoul.Data.Scriptable.Inventory;
using SplitSoul.Utility.Spawner;

namespace SplitSoul.UI.Inventory
{
	[SerializableAttribute]
	public class ItemSlot
	{
		public string id;
		public int amount;
		public InventorySlot slot;

		public ItemSlot(string id, int amount, InventorySlot slot)
		{
			this.id = id;
			this.amount = amount;
			this.slot = slot;
		}
	}

	//TODO: Remove unneeded conversions from ItemObj to ItemID and back to ItemObj
	public class InventoryController : MonoBehaviour
	{
		public GameObject slotPrefab;
		public Transform slotContainer;
		public ItemSpawner itemSpawner;

		private GameManager gm;

		private void Start()
		{
			gm = GameManager.current;
		}

		public void updateSlot(int index)
		{
			Item item = gm.existingItems.Find(x => x.id == gm.playerInventory[index].id);
			InventorySlot slot = gm.playerInventory[index].slot;

			slot.setValues(item, gm.playerInventory[index].amount);
			if (gm.playerInventory[index].amount <= 0) gm.playerInventory.RemoveAt(index);
		}

		public void createNewSlot(string id, int amount)
		{
			InventorySlot slot = Instantiate(slotPrefab, slotContainer).GetComponent<InventorySlot>();
			gm.playerInventory.Add(new ItemSlot(id, amount, slot));
		}

		public void insert(string id, int amount)
		{
			//INFO: Does not accomodate for non stackable items
			if (gm.existingItems.FindIndex(x => x.id == id) == -1) return;
			int slotIndex = gm.playerInventory.FindIndex(x => x.id == id);

			if (slotIndex == -1)
			{
				createNewSlot(id, amount);
				slotIndex = gm.playerInventory.Count - 1;
				gm.playerInventory[slotIndex].slot.setIndex(slotIndex);
			}
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

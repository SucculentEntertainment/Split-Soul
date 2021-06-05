using System;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class InventoryController : MonoBehaviour
{
	public GameObject slotPrefab;
	public Transform slotContainer;

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
		if(gm.playerInventory[index].amount <= 0) gm.playerInventory.RemoveAt(index);
	}

	public void createNewSlot(string id, int amount)
	{
		InventorySlot slot = Instantiate(slotPrefab, slotContainer).GetComponent<InventorySlot>();
		gm.playerInventory.Add(new ItemSlot(id, amount, slot));
	}

	public void insert(string id, int amount)
	{
		//INFO: Does not accomodate for non stackable items
		if(gm.existingItems.FindIndex(x => x.id == id) == -1) return;
		int slotIndex = gm.playerInventory.FindIndex(x => x.id == id);

		if(slotIndex == -1)
		{
			createNewSlot(id, amount);
			slotIndex = gm.playerInventory.Count - 1;
			gm.playerInventory[slotIndex].slot.setIndex(slotIndex);
		}
		else gm.playerInventory[slotIndex].amount += amount;

		updateSlot(slotIndex);
	}

	public int remove(string id, int amount)
	{
		int slotIndex = gm.playerInventory.FindIndex(x => x.id == id);
		if(slotIndex == -1) return -1;

		if(amount > gm.playerInventory[slotIndex].amount) amount = gm.playerInventory[slotIndex].amount;
		gm.playerInventory[slotIndex].amount -= amount;

		updateSlot(slotIndex);
		return amount;
	}

	// ================================
	//  Events
	// ================================

	private void OnInventoryInsert(Collectable collectable)
	{
        if(collectable.id == "item")
		{
			insert(collectable.item.id, collectable.amount);
		}
	}
}

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

	private GameManager gm = GameManager.current;

	public void updateSlot(int index)
	{
		Item item = gm.existingItems.Find(x => x.id == gm.playerInventory[index].id);
		InventorySlot slot = gm.playerInventory[index].slot;

		slot.item = item;
		slot.amount = gm.playerInventory[index].amount;
	}

	public void createNewSlot(string id, int amount)
	{
		InventorySlot slot = Instantiate(slotPrefab, slotContainer).GetComponent<InventorySlot>();
		gm.playerInventory.Add(new ItemSlot(id, amount, slot));
	}

	public void insert(string id, int amount)
	{
		//INFO: Does not accomodate for non stackable items
		if(gm.playerInventory.FindIndex(x => x.id == id) == -1) return;
		int slotIndex = gm.playerInventory.FindIndex(x => x.id == id);

		if(slotIndex == -1) createNewSlot(id, amount);
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

	private void OnPickup(Collectable collectable)
	{
        if (collectable.id == "coin")
        {
            gm.playerCoins++;
        }
        else if(collectable.id == "soul")
		{
            gm.playerSouls++;
        }
		else if(collectable.id == "item")
		{
			insert(collectable.item.id, collectable.amount);
		}
	}
}

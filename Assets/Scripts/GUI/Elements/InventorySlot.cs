using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	[Header("Slot Specific")]
    public Item item;
	public int amount;

	[Header("References")]
	public Text nameLabel;
	public Text amountLabel;
	public Animator animator;

	public AnimatorOverrideController missingIcon;
	public AnimatorOverrideController missingImage;

	private void Start() { setValues(item, amount); }

	public void setItem(Item item)
	{
		if(item == null)
		{
			Destroy(gameObject);
			return;
		}

		nameLabel.text = item.itemName;
		if(item.icon != null) animator.runtimeAnimatorController = item.icon;
		else animator.runtimeAnimatorController = missingIcon;

		this.item = item;
	}

	public void setAmount(int amount)
	{
		if(item == null) return;

		if(!item.stackable && amount > 1) amount = 1;
		if(amount <= 0)
		{
			Destroy(gameObject);
			return;
		}

		if(amount == 1) amountLabel.gameObject.SetActive(false);
		else
		{
			amountLabel.text = amount.ToString();
			amountLabel.gameObject.SetActive(true);
		}

		this.amount = amount;
	}

	public void setValues(Item item, int amount) { setItem(item); setAmount(amount); }

	private void OnDestroy()
	{

	}
}

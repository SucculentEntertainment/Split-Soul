using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	[Header("Slot Specific")]
	public int index = -1;
    public Item item = null;
	public int amount = 0;

	[Header("References")]
	public Text nameLabel;
	public Text amountLabel;
	public Animator animator;

	public AnimatorOverrideController missingIcon;
	public AnimatorOverrideController missingImage;

	private void Start() {if(item != null && amount != 0) setValues(item, amount); }
	public void setIndex(int index) { this.index = index; }

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
		if(amount > 9999) amount = 9999;
		if(amount <= 0)
		{
			Destroy(gameObject);
			return;
		}

		if(amount == 1) amountLabel.gameObject.SetActive(false);
		else
		{
			amountLabel.text = "x" + amount.ToString();
			amountLabel.gameObject.SetActive(true);
		}

		this.amount = amount;
	}

	public void setValues(Item item, int amount) { setItem(item); setAmount(amount); }

	private void OnDestroy()
	{

	}



	public void throwUIActionEvent(string action)
	{
		GameEventSystem.current.ThrowUIAction(new UIAction(action));
	}

	public void throwIndexedUIActionEvent(string action, int index = -1)
	{
		GameEventSystem.current.ThrowUIAction(new UIAction(action, index));
	}

	private void OnEnable() { GetComponent<Button>().onClick.AddListener(() => throwIndexedUIActionEvent("InventoryInspect", index)); }
	private void OnDisable() { GetComponent<Button>().onClick.RemoveAllListeners(); }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SplitSoul.Core;
using SplitSoul.Core.Events;
using SplitSoul.Data;
using SplitSoul.Data.Scriptable.Inventory;

namespace SplitSoul.UI.Inventory
{
	public class InspectController : MonoBehaviour
	{
		[Header("References")]
		public Animator imageAnimator;
		public Text itemName;
		public Text amount;
		public Text type;
		public Text description;

		private Item item;
		private int amountValue;
		private GameManager gm;
		private int index = -1;

		private void Start()
		{
			gm = GameManager.current;
		}

		public void setData(int index)
		{
			this.index = index;

			item = gm.existingItems.Find(x => x.id == gm.playerInventory[index].id);
			amountValue = gm.playerInventory[index].amount;

			imageAnimator.runtimeAnimatorController = item.highres;
			itemName.text = item.itemName;
			amount.text = "x" + amountValue.ToString();
			type.text = item.category;
			description.text = item.description;
		}

		public void throwUIActionEvent(string action)
		{
			GameEventSystem.current.ThrowUIAction(new UIAction(action));
		}

		public void throwIndexedUIActionEvent(string action, int index = -1)
		{
			GameEventSystem.current.ThrowUIAction(new UIAction(action, index));
		}

		public void drop()
		{
			int preDropAmount = gm.playerInventory[index].amount;
			GameEventSystem.current.Inventory("drop", new Collectable("item", 1, item));
			if (preDropAmount > 1) setData(index);
			else throwUIActionEvent("InspectClose");
		}

		public void dropAll()
		{
			GameEventSystem.current.Inventory("drop", new Collectable("item", amountValue, item));
			throwUIActionEvent("InspectClose");
		}
	}
}

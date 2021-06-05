using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectController : MonoBehaviour
{
	[Header("References")]
	public Animator imageAnimator;
	public Text itemName;
	public Text amount;
	public Text type;
	public Text description;

	private GameManager gm;

	private void Start()
	{
		gm = GameManager.current;
	}

    public void setData(int index)
	{
		Item item = gm.existingItems.Find(x => x.id == gm.playerInventory[index].id);
		int amountValue = gm.playerInventory[index].amount;

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
}

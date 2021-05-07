using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializableAttribute]
public class Menu
{
	public string menuID = "";
	public GameObject menuContainer;
}

public class UIController : MonoBehaviour
{
    public bool isTitleScreen;
	public List<Menu> menues;
	private string openedMenu = "";

    private void OnUIAction(string action)
    {
		switch(action)
		{
			case "ESC":
				if(openedMenu == "") openMenu("MainBook");
				else closeMenu(openedMenu);
				break;

			case "MainResume":
				closeMenu("MainBook");
				break;

			case "MainInventory":
				break;

			case "MainStats":
				break;

			case "MainSaves":
				break;

			case "MainSettings":
				break;

			case "MainExit":
				break;
		}
    }

	private void openMenu(string menuID)
	{
		openedMenu = menuID;
		menues.Find(x => x.menuID == menuID).menuContainer.SetActive(true);
	}

	private void closeMenu(string menuID)
	{
		if(openedMenu != menuID) return;

		menues.Find(x => x.menuID == menuID).menuContainer.SetActive(false);
		openedMenu = "";
	}
}

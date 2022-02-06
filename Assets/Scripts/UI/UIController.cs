using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using SplitSoul.Core;
using SplitSoul.Core.Events;
using SplitSoul.UI.Console;
using SplitSoul.UI.Inventory;
using SplitSoul.UI.Menu;

namespace SplitSoul.UI
{
	[SerializableAttribute]
	public class MenuObject
	{
		public string menuID = "";
		public GameObject menuContainer;
	}

	public class UIController : MonoBehaviour
	{
		[Header("Effects")]
		public UnityEngine.Rendering.VolumeProfile postProcessingProfile;

		public float deafultDoF = 4;
		public float blurDoF = 0.1f;
		public float blurAnimDuration = 0.5f;

		[Header("References")]
		public MainBook mainBook;

		[Header("UI Controls")]
		public bool titleScreen;

		public List<MenuObject> menues;
		private string openedMenu = "";

		UnityEngine.Rendering.Universal.DepthOfField dof;

		private void Start()
		{
			if (!postProcessingProfile.TryGet(out dof)) throw new System.NullReferenceException(nameof(dof));
			resetMenus();
		}

		public void setTitleScreenMode(bool mode)
		{
			titleScreen = mode;
			dof.focusDistance.Override(mode ? blurDoF : deafultDoF);

			if (mode) openMenu("TitleScreen", true);
			mainBook.setTitleScreenMode(mode);
		}

		// ================================
		//  Action Matching
		// ================================

		private void OnUIAction(UIAction uiAction)
		{
			Debug.Log("Thrown UI Action: " + uiAction.action + ", " + uiAction.index.ToString());

			// --------------------------------
			//  Actions
			// --------------------------------

			if (uiAction.action == "ESC")
			{
				if (openedMenu == "") openMenu("MainBook");
				else
				{
					if (!titleScreen) closeMenu(openedMenu);
					else openMenu("TitleScreen", true);
				}
			}

			if (uiAction.action == "Enter")
			{
				if (openedMenu == "Console") menues.Find(x => x.menuID == "Console").menuContainer.GetComponent<ConsoleController>().HandleInput();
				if (openedMenu == "TitleScreen") openMenu("MainBook", true); //TODO: Improve this
			}

			if (uiAction.action == "Console") { if (openedMenu == "") openMenu("Console"); }
			if (uiAction.action == "MainReturn")
			{
				if (!titleScreen) closeMenu("MainBook");
				else openMenu("TitleScreen", true);
			}

			if (uiAction.action == "MainInventory") { }
			if (uiAction.action == "MainStats") { }

			if (uiAction.action == "MainNewGame") { GameEventSystem.current.LevelChange((int)SceneIndecies.TestingLevel); }
			if (uiAction.action == "MainSaves") { }
			if (uiAction.action == "MainSettings") { }
			if (uiAction.action == "MainExit") { }

			if (uiAction.action == "InventoryInspect" && uiAction.index > -1)
			{
				openMenu("Inspect", true);
				menues.Find(x => x.menuID == "Inspect").menuContainer.GetComponent<InspectController>().setData(uiAction.index);
			}

			if (uiAction.action == "InspectClose") { openMenu("MainBook", true); }

			// --------------------------------
			//  Control
			// --------------------------------

			if (uiAction.action == "CTRL_SetNormalMode")
			{
				resetMenus();
				if(titleScreen) setTitleScreenMode(false);
			}

			if (uiAction.action == "CTRL_SetTitleMode") { setTitleScreenMode(true); }
		}

		// ================================
		//  Menu control
		// ================================

		public void resetMenus(bool noAnimation = true) { for (int i = 0; i < menues.Count; i++) closeMenu(menues[i].menuID, noAnimation); }

		private void openMenu(string menuID, bool noAnimation = false)
		{
			resetMenus(noAnimation);
			if (!noAnimation) StartCoroutine(blurAnimation(false, blurAnimDuration));

			openedMenu = menuID;
			menues.Find(x => x.menuID == menuID).menuContainer.SetActive(true);

			GameManager.current.playerDisableMovement = true;
		}

		private void closeMenu(string menuID, bool noAnimation = false)
		{
			if (!noAnimation) StartCoroutine(blurAnimation(true, blurAnimDuration));

			menues.Find(x => x.menuID == menuID).menuContainer.SetActive(false);
			openedMenu = "";

			GameManager.current.playerDisableMovement = false;
		}

		IEnumerator blurAnimation(bool invert, float duration)
		{
			float timePassed = 0;

			while (true)
			{
				timePassed += Time.unscaledDeltaTime;
				if (timePassed >= duration) break;

				float val = timePassed / duration;
				if (invert) val = 1 - val;

				dof.focusDistance.Override(Mathf.Lerp(deafultDoF, blurDoF, val));
				yield return null;
			}

			yield return null;
		}
	}
}
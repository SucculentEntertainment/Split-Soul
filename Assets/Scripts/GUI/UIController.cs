using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[SerializableAttribute]
public class Menu
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
	public Player player;

	[Header("UI Controls")]
    public bool isTitleScreen;

	public List<Menu> menues;
	private string openedMenu = "";

	UnityEngine.Rendering.Universal.DepthOfField dof;

	private void Start()
	{
		if(!postProcessingProfile.TryGet(out dof)) throw new System.NullReferenceException(nameof(dof));
		resetMenus();
		dof.focusDistance.Override(isTitleScreen ? blurDoF : deafultDoF);

		if(isTitleScreen) openMenu("TitleScreen", true);
		mainBook.setTitleScreenMode(isTitleScreen);
	}

	// ================================
	//  Action Matching
	// ================================

    private void OnUIAction(string action)
    {
		if(action == "ESC")
		{
			if(openedMenu == "") openMenu("MainBook");
			else
			{
				if(!isTitleScreen) closeMenu(openedMenu);
				else openMenu("TitleScreen", true);
			}
		}

		if(action == "Enter")
		{
			if(openedMenu == "Console") menues.Find(x => x.menuID == "Console").menuContainer.GetComponent<DebugController>().HandleInput();
			if(openedMenu == "TitleScreen") openMenu("MainBook", true); //TODO: Improve this
		}

		if(action == "Console") { if(openedMenu == "") openMenu("Console"); }
		if(action == "MainReturn")
		{
			if(!isTitleScreen) closeMenu("MainBook");
			else openMenu("TitleScreen", true);
		}

		if(action == "MainInventory") { }
		if(action == "MainStats") { }

		if(action == "MainNewGame") { GameEventSystem.current.LevelChange((int) SceneIndecies.TestingLevel); }
		if(action == "MainSaves") { }
		if(action == "MainSettings") { }
		if(action == "MainExit") { }
    }

	// ================================
	//  Menu control
	// ================================

	private void resetMenus(bool noAnimation = true) { for(int i = 0; i < menues.Count; i++) closeMenu(menues[i].menuID, noAnimation); }

	private void openMenu(string menuID, bool noAnimation = false)
	{
		resetMenus(noAnimation);
		if(!noAnimation) StartCoroutine(blurAnimation(false, blurAnimDuration));

		openedMenu = menuID;
		menues.Find(x => x.menuID == menuID).menuContainer.SetActive(true);

		if(player != null) player.setMovementActive(false);
	}

	private void closeMenu(string menuID, bool noAnimation = false)
	{
		if(!noAnimation) StartCoroutine(blurAnimation(true, blurAnimDuration));

		menues.Find(x => x.menuID == menuID).menuContainer.SetActive(false);
		openedMenu = "";

		if(player != null) player.setMovementActive(true);
	}

	IEnumerator blurAnimation(bool invert, float duration)
	{
		float timePassed = 0;

		while(true)
		{
			timePassed += Time.unscaledDeltaTime;
			if(timePassed >= duration) break;

			float val = timePassed / duration;
			if(invert) val = 1 - val;

			dof.focusDistance.Override(Mathf.Lerp(deafultDoF, blurDoF, val));
			yield return null;
		}

		yield return null;
	}
}

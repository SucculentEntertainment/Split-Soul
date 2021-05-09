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
	[Header("Effects")]
	public UnityEngine.Rendering.VolumeProfile postProcessingProfile;

	public float deafultDoF = 4;
	public float blurDoF = 0.1f;
	public float blurAnimDuration = 0.5f;

	[Header("UI Controls")]
    public bool isTitleScreen;

	public List<Menu> menues;
	private string openedMenu = "";

	UnityEngine.Rendering.Universal.DepthOfField dof;

	private void Start()
	{
		if(!postProcessingProfile.TryGet(out dof)) throw new System.NullReferenceException(nameof(dof));
		dof.focusDistance.Override(deafultDoF);
	}

	// ================================
	//  Action Matching
	// ================================

    private void OnUIAction(string action)
    {
		if(action == "ESC")
		{
			if(openedMenu == "") openMenu("MainBook");
			else closeMenu(openedMenu);
		}

		if(action == "Enter") { if(openedMenu == "Console") menues.Find(x => x.menuID == "Console").menuContainer.GetComponent<DebugController>().HandleInput(); }
		if(action == "Console") { if(openedMenu == "") openMenu("Console"); }
		if(action == "MainReturn") { closeMenu("MainBook"); }
		if(action == "MainInventory") { }
		if(action == "MainStats") { }
		if(action == "MainSaves") { }
		if(action == "MainSettings") { }
		if(action == "MainExit") { }
    }

	// ================================
	//  Menu control
	// ================================

	private void openMenu(string menuID)
	{
		StartCoroutine(blurAnimation(false, blurAnimDuration));

		openedMenu = menuID;
		menues.Find(x => x.menuID == menuID).menuContainer.SetActive(true);
	}

	private void closeMenu(string menuID)
	{
		StartCoroutine(blurAnimation(true, blurAnimDuration));

		menues.Find(x => x.menuID == menuID).menuContainer.SetActive(false);
		openedMenu = "";
	}

	IEnumerator blurAnimation(bool invert, float duration)
	{
		float timePassed = 0;

		while(true)
		{
			timePassed += Time.deltaTime;
			if(timePassed >= duration) break;

			float val = timePassed / duration;
			if(invert) val = 1 - val;

			dof.focusDistance.Override(Mathf.Lerp(deafultDoF, blurDoF, val));
			yield return null;
		}

		yield return null;
	}
}

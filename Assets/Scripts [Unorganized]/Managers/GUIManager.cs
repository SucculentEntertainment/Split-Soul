using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    // ================================
    //  UI Elements
    // ================================

    public UIBar healthBar;
    public UIBar manaBar;
    public UIBar staminaBar;

    public UICounter coinCounter;
    public UICounter soulCounter;

    // ================================
    //  Functions
    // ================================

    public void init(float maxHealth, float maxMana, float maxStamina)
	{
        healthBar.setMaxValue(maxHealth);
        manaBar.setMaxValue(maxMana);
        staminaBar.setMaxValue(maxStamina);
    }

    // ================================
    //  Events
    // ================================

    private void OnDimensionEnable(string dimension)
	{
        if(dimension == "dead")
		{
            staminaBar.gameObject.SetActive(false);
            manaBar.gameObject.SetActive(true);
		}
	}

    private void OnDimensionDisable(string dimension)
    {
        if (dimension == "alive")
        {
            staminaBar.gameObject.SetActive(true);
            manaBar.gameObject.SetActive(false);
        }
    }
}

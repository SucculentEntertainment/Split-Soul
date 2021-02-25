using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class altarController : MonoBehaviour
{
    public float cooldown = 10f;
    public string target = "alive";

    public UIBar cooldownBar;

    private bool enableInteractions = false;
    private bool onCooldown = false;
    private float cooldownTimer = 0f;

    private Animator animator;

	// ================================
	//  Functions
	// ================================

	private void Start()
	{
        cooldownBar.setMaxValue(cooldown);
        cooldownBar.gameObject.SetActive(false);

        animator = transform.Find("Sprite").GetComponent<Animator>();
        animator.SetBool("isBroken", true);
	}

	void Update()
    {
        if(onCooldown)
		{
            cooldownTimer += Time.deltaTime;
            cooldownBar.setValue(cooldownTimer);
            if(cooldownTimer >= cooldown)
            {
                onCooldown = false;
                cooldownBar.gameObject.SetActive(false);
            }
		}
    }

    // ================================
    //  Events
    // ================================

    private void OnDimensionEnable(string dimension)
	{
        enableInteractions = true;
        if(onCooldown) cooldownBar.gameObject.SetActive(true);
        animator.SetBool("isBroken", false);
	}

    private void OnDimensionDisable(string dimension)
	{
        enableInteractions = false;
        cooldownBar.gameObject.SetActive(false);
        animator.SetBool("isBroken", true);
    }

    private void OnInteract()
	{
        if(!enableInteractions) { return; }
        if(onCooldown) { return; }

        onCooldown = true;
        cooldownTimer = 0f;
        cooldownBar.gameObject.SetActive(true);

        GameEventSystem.current.Revive("Player");
	}

    private void OnInteractHighlight(bool activate)
	{
        if (!enableInteractions ||!activate)
        {
            transform.Find("Sprite").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            return;
        }

        transform.Find("Sprite").GetComponent<SpriteRenderer>().color = new Color(255, 255, 0);
    }
}

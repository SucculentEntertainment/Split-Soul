using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour
{
    public static Player current;

    // --------------------------------
    //  Parameters
    // --------------------------------

    public float maxHealth = 100f;
    public float baseAttack = 1f;
    public float baseAttackRate = 2f;
    public float speed = 1f;

    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public float interactRange = 0.5f;
    public LayerMask interactLayers;

    // --------------------------------
    //  Internal Values
    // --------------------------------

    private float health;
    private int deathState;

    private float nextAttackTime = 0f;

    private Vector2 dir;
    private Rigidbody2D rb;

    private GameObject sprite;
    private Animator animator;

    private int coins = 0;
    private int souls = 0;

    private Collider2D interactable;

    // --------------------------------
    //  Flags
    // --------------------------------

    private bool disableMovement = false;

    // --------------------------------
    //  References
    // --------------------------------

    public Transform attackPoint;
    public Transform interactPoint;
    public GUIManager guiManager; //TODO: Rename to HUDManager
    public Light2D lamp;

    // ================================
    //  Functions
    // ================================

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        health = maxHealth;
        deathState = 0;

        rb = GetComponent<Rigidbody2D>();
        sprite = transform.Find("Sprite").gameObject;
        animator = sprite.GetComponent<Animator>();

        guiManager.init(maxHealth, 10, 10);
        if(lamp != null) GameEventSystem.current.RegisterLight(lamp);
    }

    void Update()
    {
        animator.SetFloat("Mag", dir.magnitude);

        if (dir.magnitude != 0f)
        {
            animator.SetFloat("DirX", dir.x);
            animator.SetFloat("DirY", dir.y);
        }

        //Scan for interactables
        Collider2D newInteractable = Physics2D.OverlapCircle(interactPoint.position, interactRange, interactLayers);
        if (newInteractable != interactable)
        {
            if (interactable != null) GameEventSystem.current.InteractHighlight(interactable.name, false);
            interactable = newInteractable;
            if (interactable != null) GameEventSystem.current.InteractHighlight(interactable.name, true);
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(dir * speed, ForceMode2D.Impulse);
    }

    // ================================
    //  Damage
    // ================================

    private void attack()
	{
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
		{
            GameEventSystem.current.GiveDamage(enemy.name, baseAttack);
		}
	}

	public void OnReceiveDamage(float damage)
	{
	    health -= damage;
        guiManager.healthBar.setValue(health);

	    if(health <= 0) die();
	}

	public void OnReceiveHeal(float amount)
	{
	    health += amount;
        guiManager.healthBar.setValue(health);

        if (health > maxHealth) health = maxHealth;
	}

	private void die()
	{
	    if(deathState > 0)
        {
            animator.SetTrigger("Die4Real");
            deathState++;
            if(lamp != null) GameEventSystem.current.UnregisterLight(lamp);
            return;
        }

        health = maxHealth;
        deathState++;

        guiManager.healthBar.setValue(health);

        animator.SetTrigger("Die");
        LevelManager.dimension = "dead";
    }

    // ================================
    //  Events
    // ================================

    private void OnRevive()
	{
        health = maxHealth;
        deathState--;

        guiManager.healthBar.setValue(health);

        animator.SetTrigger("Revive");
        LevelManager.dimension = "alive";
	}

    private void OnPickup(Item item)
	{
        // IMPORTANT! Move this into inventory Item Handler
        if (item.type == "coin")
        {
            coins++;
            guiManager.coinCounter.setValue(coins);
        }
        else if(item.type == "soul")
		{
            souls++;
            guiManager.soulCounter.setValue(souls);
        }
	}

    private void OnProjectileHit(ProjectileData data)
    {
        OnReceiveDamage(data.damage);
    }

    // ================================
    //  Interact
    // ================================

    private void interact()
	{
        if(interactable != null) GameEventSystem.current.Interact(interactable.name);
    }

    // ================================
    //  Input
    // ================================

    private void OnAttack(InputValue val)
    {
        if (Time.time >= nextAttackTime)
        {
            attack();
            nextAttackTime = Time.time + 1f / baseAttackRate;
        }
    }

    private void OnMove(InputValue dirVal)
    {
        if (disableMovement) return;
        dir = dirVal.Get<Vector2>();
    }

    private void OnConsole(InputValue val)
    {
        disableMovement = !disableMovement;
        dir = new Vector2(0, 0);
    }

    private void OnEscape(InputValue val)
    {
		GameEventSystem.current.UIAction("ESC");
        disableMovement = false;
    }

    private void OnInteract(InputValue val)
	{
        interact();
	}

    // ================================
    //  Gizmos
    // ================================

    void OnDrawGizmosSelected()
	{
        Gizmos.color = new Color(255, 0, 0);
        if (attackPoint != null) Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        Gizmos.color = new Color(0, 255, 0);
        if (interactPoint != null) Gizmos.DrawWireSphere(interactPoint.position, interactRange);
    }
}

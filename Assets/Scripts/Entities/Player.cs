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

    private Vector2 dir;
    private Rigidbody2D rb;

    private GameObject sprite;
    private Animator animator;

    private Collider2D interactable;
	private GameManager gm;

    // --------------------------------
    //  Flags
    // --------------------------------

    private bool disableMovement = false;

    // --------------------------------
    //  References
    // --------------------------------

    public Transform attackPoint;
    public Transform interactPoint;
	public UIController uiController;
    public Light2D lamp;

    // ================================
    //  Functions
    // ================================

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
		gm = GameManager.current;

		rb = GetComponent<Rigidbody2D>();
        sprite = transform.Find("Sprite").gameObject;
        animator = sprite.GetComponent<Animator>();

        if(lamp != null) GameEventSystem.current.RegisterLight(lamp);

        gm.playerHealth = maxHealth;
        gm.playerDeathState = 0;
    }

	private void Update()
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

    private void FixedUpdate()
    {
        rb.AddForce(dir * speed, ForceMode2D.Impulse);
    }

	public void stopMovement()
	{
		dir = new Vector2(0, 0);
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
	    gm.playerHealth -= damage;
	    if(gm.playerHealth <= 0) die();
	}

	public void OnReceiveHeal(float amount)
	{
	    gm.playerHealth += amount;
        if (gm.playerHealth > maxHealth) gm.playerHealth = maxHealth;
	}

	private void die()
	{
		gm.playerDeathState++;

		animator.SetInteger("DeathState", gm.playerDeathState);
        animator.SetTrigger("Die");

	    if(gm.playerDeathState > 1)
        {
            if(lamp != null) GameEventSystem.current.UnregisterLight(lamp);
            return;
        }

        gm.playerHealth = maxHealth;
        gm.changeDimension("dead");
    }

    // ================================
    //  Events
    // ================================

    private void OnRevive()
	{
        gm.playerHealth = maxHealth;
        gm.playerDeathState--;

        animator.SetInteger("DeathState", gm.playerDeathState);
        animator.SetTrigger("Die");

        gm.changeDimension("alive");
	}

    private void OnPickup(Collectable collectable)
	{
        if (collectable.id == "coin")
        {
            gm.playerCoins++;
        }
        else if(collectable.id == "soul")
		{
            gm.playerSouls++;
        }
		else if(collectable.id == "item")
		{
			GameEventSystem.current.InventoryInsert(collectable);
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
        if (Time.time >= gm.playerNextAttackTime)
        {
            attack();
            gm.playerNextAttackTime = Time.time + 1f / baseAttackRate;
        }
    }

    private void OnMove(InputValue dirVal)
    {
        if (disableMovement) return;
        dir = dirVal.Get<Vector2>();
    }

    private void OnConsole(InputValue val)
    {
		GameEventSystem.current.ThrowUIAction(new UIAction("Console"));
    }

    private void OnEscape(InputValue val)
    {
		GameEventSystem.current.ThrowUIAction(new UIAction("ESC"));
    }

	private void OnReturn(InputValue val)
    {
		GameEventSystem.current.ThrowUIAction(new UIAction("Enter"));
    }

    private void OnInteract(InputValue val)
	{
        interact();
	}

	public void setMovementActive(bool active)
	{
		disableMovement = !active;
		if(!active) stopMovement();
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

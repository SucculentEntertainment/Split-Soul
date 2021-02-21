using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player current;

    // --------------------------------
    //  Parameters
    // --------------------------------

    public float maxHealth = 100f;
    public float baseAttack = 1f;
    public float speed = 1f;

    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    // --------------------------------
    //  Internal Values
    // --------------------------------

    private float health;
    private int deathState;

    private Vector2 dir;
    private Rigidbody2D rb;

    private GameObject sprite;
    private Animator animator;

    // --------------------------------
    //  Flags
    // --------------------------------

    private bool disableMovement = false;

    // --------------------------------
    //  References
    // --------------------------------

    public Transform attackPoint;

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
        sprite = transform.GetChild(0).gameObject;
        animator = sprite.GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetFloat("Mag", dir.magnitude);

        if (dir.magnitude != 0f)
        {
            animator.SetFloat("DirX", dir.x);
            animator.SetFloat("DirY", dir.y);
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
            Debug.Log(enemy.name);
            GameEventSystem.current.GiveDamage(enemy.name, baseAttack);
		}
	}
	
	public void OnReceiveDamage(float damage)
	{
	    health -= damage;
	    if(health <= 0) die();
	}
	
	public void OnReceiveHeal(float amount)
	{
	    health += amount;
	    if(health > maxHealth) health = maxHealth;
	}
	
	private void die()
	{
	    if(deathState == 0) health = maxHealth;
	    if(deathState == 1) { /*Really Really Dead*/ }
	    
	    deathState++;
	    animator.SetInteger("DeathState", deathState);
    }

    // ================================
    //  Input
    // ================================

    private void OnAttack(InputValue val)
    {
        attack();
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
        disableMovement = false;
    }

    // ================================
    //  Gizmos
    // ================================

    void OnDrawGizmosSelected()
	{
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}
}
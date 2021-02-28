using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    // --------------------------------
    //  States
    // --------------------------------

    private enum State
	{
        IDLE,
        MOVE,
        ATTACK,
        DEAD
	}

    private string[] animationTrigger = { "Idle", "Move", "Attack", "Die" };

    // --------------------------------
    //  Parameters
    // --------------------------------

    public float maxHealth;
    public float baseAttack;

    public float roamingProbability;

    public float roamingCooldown;
    public float interestCooldown;
    public float attackCooldown;
    
    public Transform detectPoint;
    public float detectRange;
    public LayerMask detectLayers;

    public Transform attackPoint;
    public float attackRange;
    public float attackRangePadding;

    // --------------------------------
    //  Internal Values
    // --------------------------------

    private NavMeshAgent agent;

    private float health;
    private State state = State.IDLE;
    
    private float roamingTimer = 0f;
    private float interestTimer = 0f;
    private float attackTimer = 0f;
    
    private GameObject targetObject;
    private Vector2 targetPosition;

    private Animator animator;

    // ================================
    //  Functions
    // ================================

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.stoppingDistance = attackRange - attackRangePadding;

        health = maxHealth;
        animator = GetComponent<Animator>();

        additionalStart();
    }

    private void Update()
    {
        Collider2D target = Physics2D.OverlapCircle(detectPoint.position, detectRange, detectLayers);

        if (target != null)
        {
            if (target.gameObject == targetObject) interestTimer = 0f;
            else
            {
                targetObject = target.gameObject;
                setState(State.MOVE);
            }
        }

        if (targetObject != null) interestTimer += Time.deltaTime;
        if (interestTimer >= interestCooldown)
        {
            targetObject = null;
            targetPosition = Vector2.zero;
            interestTimer = 0f;

            setState(State.IDLE);
        }

        attackTimer += Time.deltaTime;
        roamingTimer += Time.deltaTime;

        additionalUpdate();
    }

	private void FixedUpdate()
	{
		switch(state)
		{
            case State.IDLE:
                idleState();
                break;

            case State.MOVE:
                moveState();
                break;

            case State.ATTACK:
                attackState();
                break;

            case State.DEAD:
                deadState();
                break;
        }
	}

    private void additionalStart()
	{

	}

    private void additionalUpdate()
	{

	}

    // ================================
    //  State Handler
    // ================================

    private void idleState()
	{
        idle();
        
        if(roamingTimer >= roamingCooldown)
		{
            roamingTimer = 0f;
            
            if((int) Random.Range(1, 100) <= roamingProbability)
			{
                Vector2 dir = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
                Vector2 vel = dir * Random.Range(1, 2);
                targetPosition = (Vector2) transform.position * dir;

                setState(State.MOVE);
            }
		}
	}

    private void moveState()
    {
        if (targetObject != null) targetPosition = (Vector2) targetObject.transform.position;
        move();
    }

    private void attackState()
    {
        attack();
    }

    private void deadState()
    {
        dead();
    }

    private void setState(State state)
	{
        animator.SetTrigger(animationTrigger[(int) state]);
        this.state = state;
	}

    // ================================
    //  States
    // ================================

    private void idle()
    {
        
    }

    private void move()
    {
        if(Vector2.Distance(attackPoint.position, targetPosition) <= attackRange - attackRangePadding)
		{
            if (targetObject != null && attackTimer >= attackCooldown)
            {
                attackTimer = 0f;

                setState(State.ATTACK);
                StartCoroutine(WaitForAnimation(animator.GetCurrentAnimatorStateInfo(0).length, State.MOVE));
            }
            else
			{
                setState(State.IDLE);
            }
		}
        else
		{
            agent.SetDestination(targetPosition);
        }
    }

    private void attack()
    {
        GameEventSystem.current.GiveDamage(targetObject.name, baseAttack);
        setState(State.MOVE);
    }

    private void dead()
    {
        //TODO: Spwan Loot
        Destroy(gameObject);
    }

    // ================================
    //  Coroutines
    // ================================

    private IEnumerator WaitForAnimation(float _delay = 0, State _state = State.IDLE)
	{
        yield return new WaitForSeconds(_delay);
        setState(_state);
    }

    // ================================
    //  Damage
    // ================================

    void die()
    {
        StartCoroutine(WaitForAnimation(animator.GetCurrentAnimatorStateInfo(0).length, State.DEAD));
    }

    // ================================
    //  Events
    // ================================

    public void OnReceiveDamage(float damage)
	{
        health -= damage;
        if (health <= 0) die();
	}

    // ================================
    //  Gizmos
    // ================================

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 0, 0);
        if (attackPoint != null) Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        Gizmos.color = new Color(255, 255, 0);
        if (attackPoint != null) Gizmos.DrawWireSphere(attackPoint.position, attackRange - attackRangePadding);

        Gizmos.color = new Color(0, 0, 255);
        if (detectPoint != null) Gizmos.DrawWireSphere(detectPoint.position, detectRange);
    }
}

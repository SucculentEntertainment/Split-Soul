using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;

public class EnemyBase : MonoBehaviour
{
    // --------------------------------
    //  States
    // --------------------------------

    protected enum State
	{
        IDLE,
        MOVE,
        ATTACK,
        DEAD
	}

    protected string[] animationTrigger = { "Idle", "Move", "Attack", "Die" };

    // --------------------------------
    //  Parameters
    // --------------------------------

	[Header("Ranged")]
    public bool isRanged = false;
    public GameObject projectileContainer;
    public GameObject projectile;

	[Header("Base")]
    public float maxHealth;
    public float baseAttack;
    public string damageType;

	[Header("Cooldowns")]
    public float roamingProbability;

    public float roamingCooldown;
    public float interestCooldown;
    public float attackCooldown;
    
    [Header("Ranges")]
    public Transform detectPoint;
    public float detectRange;
    public LayerMask detectLayers;

    public Transform attackPoint;
    public float attackRange;
    public float attackRangePadding;

	[Header("Transformation")]
	public bool canTransform;
	public Dictionary<string, GameObject> transfomations;

    // --------------------------------
    //  Internal Values
    // --------------------------------

    protected NavMeshAgent agent;

    protected float health;
    protected State state = State.IDLE;
    
    protected float roamingTimer = 0f;
    protected float interestTimer = 0f;
    protected float attackTimer = 0f;
    
    protected GameObject targetObject;
    protected Vector2 targetPosition;

    protected Animator animator;
    protected LineRenderer pathLine;

    protected bool isDead = false;
    [HideInInspector] public bool spawnProjectile = false;

    // ================================
    //  Functions
    // ================================

    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        health = maxHealth;
        animator = GetComponent<Animator>();

        pathLine = GetComponent<LineRenderer>();

        additionalStart();
    }

    protected void Update()
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
            pathLine.positionCount = 0;
            agent.SetDestination(transform.position);

            setState(State.IDLE);
        }

        attackTimer += Time.deltaTime;
        roamingTimer += Time.deltaTime;

        additionalUpdate();
    }

	protected void FixedUpdate()
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

    public virtual void additionalStart()
	{

	}

    public virtual void additionalUpdate()
	{

	}

    // ================================
    //  State Handler
    // ================================

    protected void idleState()
	{
        if(isDead) return;
        idle();
        
        if(roamingTimer >= roamingCooldown)
		{
            roamingTimer = 0f;
            
            if((int) Random.Range(1, 100) <= roamingProbability)
			{
                Vector2 dir = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                targetPosition = (Vector2) transform.position + dir;

                setState(State.MOVE);
            }
		}
	}

    protected void moveState()
    {
        //FIXME: Fix Target detection for ranged

        if(isDead) return;

        if (targetObject != null) targetPosition = (Vector2) targetObject.transform.position;

        pathLine.positionCount = agent.path.corners.Length;

        bool isInRange = Vector2.Distance(attackPoint.position, targetPosition) <= attackRange;
        bool isTooClose = Vector2.Distance(attackPoint.position, targetPosition) <= attackRangePadding && isRanged;

        if(targetObject != null && isRanged && !isInRange)
        {
            Vector2 dir = (targetPosition - (Vector2) transform.position).normalized;
            float scalar = Vector2.Distance(targetPosition, transform.position) - attackRangePadding * 1.25f;
            targetPosition = (Vector2) transform.position + dir * scalar;
        }

        if (agent.path.corners != null && agent.path.corners.Length > 1)
        {
            for(int i = 0; i < agent.path.corners.Length; i++) pathLine.SetPosition(i, agent.path.corners[i]);
        }
        else pathLine.positionCount = 0;

        if(isInRange && !isTooClose)
		{
            if (targetObject != null && attackTimer >= attackCooldown)
            {
                attackTimer = 0f;
                setState(State.ATTACK);
                StartCoroutine(Wait(animator.GetCurrentAnimatorStateInfo(0).length, true, State.MOVE));
            }
            else setState(State.IDLE);
            return;
		}

        if(isTooClose) targetPosition -= (targetPosition - (Vector2) transform.position).normalized * attackRangePadding * 2;
        move();
    }

    protected void attackState()
    {
        if(isDead) return;
        
        if(!isRanged) attack();
        else attackRanged();
    }

    protected void deadState()
    {
        dead();
    }

    protected void setState(State state, bool changeAnim = true)
	{
        if(isDead && state != State.DEAD) return;

        if(changeAnim) animator.SetTrigger(animationTrigger[(int) state]);
        this.state = state;
	}

    // ================================
    //  States
    // ================================

    public virtual void idle()
    {
        
    }

    public virtual void move()
    {
        agent.SetDestination(targetPosition);
    }

    public virtual void attack()
    {
        GameEventSystem.current.GiveDamage(targetObject.name, baseAttack);
        setState(State.MOVE);
    }

    public virtual void attackRanged()
    {
        //FIXME: Only spawn one, not multiple

        if(!spawnProjectile) return;
        spawnProjectile = false;

        GameObject instance = Instantiate(projectile, transform.position, Quaternion.identity, projectileContainer.transform);

        Vector2 dir = (targetPosition - (Vector2) transform.position).normalized;
        instance.GetComponent<ProjectileBase>().init(dir, this.name);
    }

    public virtual void dead()
    {
        //TODO: Spwan Loot

        GetComponent<DimensionEvent>().unregister();
        GetComponent<DamageEvent>().unregister();
        GetComponent<DebugEvent>().unregister();

        Destroy(gameObject);
    }

    // ================================
    //  Coroutines
    // ================================

    protected IEnumerator Wait(float _delay = 0, bool setStateOnFinish = false, State _state = State.IDLE, bool changeAnim = true)
	{
        yield return new WaitForSeconds(_delay);
        if(setStateOnFinish) setState(_state, changeAnim);
    }

    // ================================
    //  Actions
    // ================================

    protected void die()
    {
        if(isDead) return;

        isDead = true;
        animator.SetTrigger(animationTrigger[(int) State.DEAD]);

        StartCoroutine(Wait(animator.GetCurrentAnimatorStateInfo(0).length, true, State.DEAD, false));
    }

    protected void setEnabled(bool active)
    {
        GetComponent<CapsuleCollider2D>().enabled = active;
        GetComponent<DamageEvent>().enabled = active;
        animator.enabled = active;
        agent.enabled = active;
        transform.Find("Sprite").gameObject.SetActive(active);
    }

    // ================================
    //  Events
    // ================================

    public void OnReceiveDamage(float damage)
	{
        health -= damage;
        if (health <= 0) die();
	}

    protected void OnDebug(string debugType)
	{
        if(debugType == "path") pathLine.enabled = !pathLine.enabled;
	}

    protected virtual void OnDimensionEnable(string dimension)
    {
        int index = LevelManager.dimensions.FindIndex(x => x.Contains(dimension));
        if(index == -1) index = 0;

        animator.SetInteger("Dim", index);
        animator.SetTrigger(animationTrigger[(int) state]);
        setEnabled(true);
    }

    public virtual void OnDimensionDisable(string dimension)
    {
        setEnabled(false);
    }

    // ================================
    //  Gizmos
    // ================================

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 0, 0);
        if (attackPoint != null) Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        Gizmos.color = new Color(255, 255, 0);
        if (attackPoint != null) Gizmos.DrawWireSphere(attackPoint.position, attackRangePadding);

        Gizmos.color = new Color(0, 0, 255);
        if (detectPoint != null) Gizmos.DrawWireSphere(detectPoint.position, detectRange);
    }
}

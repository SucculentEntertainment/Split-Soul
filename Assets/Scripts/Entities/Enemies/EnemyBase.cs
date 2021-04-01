using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering.Universal;

public class EnemyBase : MonoBehaviour
{
    // ================================
    //  Parameters
    // ================================

    // --------------------------------
    //  Parameters -> States
    // --------------------------------

    protected enum State
	{
        IDLE,
        MOVE,
        ATTACK,
        DEAD,
        MUTATE_INIT,
        MUTATE,
        SPECIAL_ATTACK
	}

    protected string[] animationTrigger = { "Idle", "Move", "Attack", "Die", "MutateInit", "", "SpecialAttack" };

    // --------------------------------
    //  Parameters -> Attributes
    // --------------------------------

	[Header("Ranged")]
    public bool isRanged = false;
    public ProjectileSpawner projectileSpawner;

	[Header("Base")]
    public float maxHealth;
    public float baseAttack;
    public string damageType;

	[Header("Cooldowns")]
    public float roamingProbability;

    public float roamingCooldown;
    public float interestCooldown;
    public float attackCooldown;
    public float specialAttackCooldown;
    
    [Header("Ranges")]
    public Transform detectPoint;
    public float detectRange;
    public LayerMask detectLayers;

    public Transform attackPoint;
    public float attackRange;
    public float attackRangePadding;

	[Header("Mutation")]
	public bool canMutate;
	public List<Mutation> mutations;

    [Header("Common Special Attack")]
    public float damageFactor = 1;

    [Header("References")]
    public Light2D lamp;

    // --------------------------------
    //  Parameters -> Internal Values
    // --------------------------------

    protected NavMeshAgent agent;

    protected float health;
    protected State state = State.IDLE;
    
    protected float roamingTimer = 0f;
    protected float interestTimer = 0f;
    protected float attackTimer = 0f;
    protected float specialAttackTimer = 0f;
    
    protected GameObject targetObject;
    protected Vector2 targetPosition;
    protected Vector2 aimPosition;

    protected Animator animator;
    protected LineRenderer pathLine;

    protected bool isDead = false;
    protected bool isInRange = false;
    protected bool isTooClose = false;

    protected GameObject mutationTarget;
    protected int mutationID; 
    protected bool isMutating = false;
    protected bool isMutateInit = false;
    
    [HideInInspector] public bool specialAttackEnded = true;

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
        GetComponent<DimensionEvent>().changeDimension(LevelManager.dimension);

        additionalStart();
    }

    protected void Update()
    {
        Collider2D target = Physics2D.OverlapCircle(detectPoint.position, detectRange, detectLayers);
        if(lamp != null) GameEventSystem.current.RegisterLight(lamp);

        if (target != null)
        {
            if (target.gameObject == targetObject) interestTimer = 0f;
            else
            {
                targetObject = target.gameObject;
                StartCoroutine(setState(State.MOVE));
            }
        }

        if(targetObject != null)
        {
            if(state != State.SPECIAL_ATTACK) interestTimer += Time.deltaTime;

            isTooClose = Vector2.Distance(attackPoint.position, targetObject.transform.position) <= attackRangePadding && isRanged;
            isInRange = Vector2.Distance(attackPoint.position, targetObject.transform.position) <= attackRange && !isTooClose;
        }
        else isInRange = isTooClose = false;

        if (interestTimer >= interestCooldown)
        {
            targetObject = null;
            targetPosition = aimPosition = Vector2.zero;
            interestTimer = 0f;
            pathLine.positionCount = 0;
            agent.SetDestination(transform.position);

            StartCoroutine(setState(State.IDLE));
        }

        attackTimer += Time.deltaTime;
        specialAttackTimer += Time.deltaTime;
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
            
            case State.MUTATE_INIT:
                mutateInitState();
                break;

            case State.MUTATE:
                mutateState();
                break;
            
            case State.SPECIAL_ATTACK:
                specialAttackState();
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

    // --------------------------------
    //  State Handler -> Idle
    // --------------------------------

    protected void idleState()
	{
        if(isDead || isMutating) return;
        if(isTooClose)
        {
            if(isInRange) StartCoroutine(setState(State.ATTACK));
            else StartCoroutine(setState(State.MOVE));
            return;
        }

        if(targetObject != null && isSpecialAttackEligable(targetObject))
        {
            StartCoroutine(setState(State.SPECIAL_ATTACK));
            return;
        }
        
        idle();
        
        if(roamingTimer >= roamingCooldown)
		{
            roamingTimer = 0f;
            
            if((int) UnityEngine.Random.Range(1, 100) <= roamingProbability)
			{
                Vector2 dir = new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));
                targetPosition = (Vector2) transform.position + dir;

                StartCoroutine(setState(State.MOVE));
            }
		}
	}

    // --------------------------------
    //  State Handler -> Move
    // --------------------------------

    protected void moveState()
    {
        if(isDead || isMutating) return;
        if (targetObject != null) targetPosition = (Vector2) targetObject.transform.position;
        aimPosition = targetPosition;

        pathLine.positionCount = agent.path.corners.Length;

        if(targetObject != null && !isInRange)
        {
            Vector2 dir = (targetPosition - (Vector2) transform.position).normalized;
            float scalar = Vector2.Distance(targetPosition, transform.position) - attackRangePadding * 1.25f;
            targetPosition = (Vector2) transform.position + dir * scalar;
        }

        if(agent.path.corners != null && agent.path.corners.Length > 1)
        {
            for(int i = 0; i < agent.path.corners.Length; i++) pathLine.SetPosition(i, agent.path.corners[i]);
        }

        if(isInRange && !isTooClose && targetObject != null)
		{
            StartCoroutine(setState(State.ATTACK));
            return;
		}

        if(targetObject != null && isSpecialAttackEligable(targetObject))
        {
            StartCoroutine(setState(State.SPECIAL_ATTACK));
            return;
        }

        if(isTooClose)
        {
            Vector2 dir = (targetPosition - (Vector2) transform.position).normalized;
            targetPosition += dir * attackRangePadding * 2;
        }
        
        move();
    }

    // --------------------------------
    //  State Handler -> Attack
    // --------------------------------

    protected void attackState()
    {
        if(isDead || isMutating) return;
        if(attackTimer < attackCooldown) return;
        
        attackTimer = 0f;
        if(!isRanged || (isRanged && isTooClose)) attack();
        else attackRanged();

        StartCoroutine(setState(State.IDLE));
    }

    // --------------------------------
    //  State Handler -> Death
    // --------------------------------

    protected void deadState()
    {
        dead();
    }

    // --------------------------------
    //  State Handler -> Mutation
    // --------------------------------

    protected void mutateInitState()
    {
        if(isDead) return;

        if(isMutateInit) return;
        isMutateInit = true;

        mutateInit();
    }

    protected void mutateState()
    {
        if(isDead) return;
        mutate();
        dead();
    }

    // --------------------------------
    //  State Handler -> Special Attack
    // --------------------------------
    
    protected bool isFirstExec = true;

    protected void specialAttackState()
    {
        if(isDead || isMutating) return;
        if(specialAttackTimer < specialAttackCooldown) return;
        
        if(isFirstExec)
        {
        	specialAttackTimer = 0f;
        	specialAttackEnded = false;
            isFirstExec = false;
        }
        		
        specialAttack();

        if(specialAttackEnded)
        {
        	StartCoroutine(setState(State.IDLE));
        	isFirstExec = true;
        }
    }

    // ================================
    //  States (To be overriden)
    // ================================

    // --------------------------------
    //  States -> Idle
    // --------------------------------

    public virtual void idle() { }

    // --------------------------------
    //  States -> Move
    // --------------------------------

    public virtual void move()
    {
        agent.SetDestination(targetPosition);
    }

    // --------------------------------
    //  States -> Attack
    // --------------------------------

    public virtual void attack()
    {
        float damage = baseAttack;
        if(state == State.SPECIAL_ATTACK) damage *= damageFactor;

        GameEventSystem.current.GiveDamage(targetObject.name, baseAttack);
    }

    public virtual void attackRanged() { }
    
    private void invokeSpawnProjectile() {
        if (targetObject != null) aimPosition = (Vector2) targetObject.transform.position;
        projectileSpawner.spawnProjectile(aimPosition);
    }

    // --------------------------------
    //  States -> Death
    // --------------------------------

    public virtual void dead()
    {
        //TODO: Spwan Loot

        GetComponent<ProjectileHitEvent>().unregister();
        GetComponent<DimensionEvent>().unregister();
        GetComponent<DamageEvent>().unregister();
        GetComponent<DebugEvent>().unregister();

        if(lamp != null) GameEventSystem.current.UnregisterLight(lamp);

        Destroy(gameObject);
    }

    // --------------------------------
    //  States -> Mutation
    // --------------------------------

    public virtual void mutateInit()
    {
        animator.SetInteger("MutationID", mutationID);
        animator.SetTrigger("Mutate");
        StartCoroutine(setState(State.MUTATE, animator.GetCurrentAnimatorStateInfo(0).length, false));
    }

    public virtual void mutate()
    {
        Instantiate(mutationTarget, transform.position, Quaternion.identity, transform.parent);
    }

    // --------------------------------
    //  States -> Special Attack
    // --------------------------------

    public virtual void specialAttack() { }

    // ================================
    //  Coroutines
    // ================================

    protected IEnumerator setState(State _state = State.IDLE, float _delay = 0, bool changeAnim = true)
	{
        yield return new WaitForSeconds(_delay);
        
        if(isDead && _state != State.DEAD) yield break;

        if(changeAnim) animator.SetTrigger(animationTrigger[(int) _state]);
        state = _state;
    }

    // ================================
    //  Actions
    // ================================

    protected void die()
    {
        if(isDead) return;

        isDead = true;
        animator.SetTrigger(animationTrigger[(int) State.DEAD]);

        StartCoroutine(setState(State.DEAD, animator.GetCurrentAnimatorStateInfo(0).length, false));
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

    // --------------------------------
    //  Events -> Damage
    // --------------------------------

    public void OnReceiveDamage(float damage)
	{
        health -= damage;
        if (health <= 0) die();
	}

    // --------------------------------
    //  Events -> Debug
    // --------------------------------

    protected void OnDebug(string debugType)
	{
        if(debugType == "path") pathLine.enabled = !pathLine.enabled;
	}

    // --------------------------------
    //  Events -> Dimension
    // --------------------------------

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

    // --------------------------------
    //  Events -> Projectile
    // --------------------------------

    protected virtual void OnProjectileHit(ProjectileData pData)
    {
        if(canMutate)
        {
            int i = 0;
            foreach(Mutation mData in mutations)
            {
                if(mData.useElement && pData.element == mData.element) mutateAction(i, mData.target);
                else if(pData.name == mData.projectileID) mutateAction(i, mData.target);

                i++;
            }

            return;
        }

        OnReceiveDamage(pData.damage);
    }

    // ================================
    //  Mutations
    // ================================

    protected virtual void mutateAction(int mutationID, GameObject target)
    {
        mutationTarget = target;
        this.mutationID = mutationID;

        isMutating = true;
        animator.SetTrigger(animationTrigger[(int) State.MUTATE_INIT]);
        StartCoroutine(setState(State.MUTATE_INIT, animator.GetCurrentAnimatorStateInfo(0).length, false));
    }

    // ================================
    //  Special Attack
    // ================================

    public virtual bool isSpecialAttackEligable(GameObject targetObject)
    {
        return false;
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

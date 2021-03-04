using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    // --------------------------------
    //  States
    // --------------------------------

    protected enum State
	{
        CREATE,
        TRAVEL,
        DESTROY
	}

    protected string[] animationTrigger = { "Create", "Travel", "Destroy" };
    
    // --------------------------------
    //  Parameters
    // --------------------------------

    public float lifespan;
    public float baseAttack;

    public bool canHitMultiple = false;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask attackLayers;

    // --------------------------------
    //  Internal Values
    // --------------------------------

    protected State state = State.CREATE; 
    protected float lifeTimer = 0;

    protected bool isDestroyed = false;

    // ================================
    //  Functions
    // ================================

    protected void Start() {
        
    }

    protected void Update() {
        lifeTimer += Time.deltaTime;

        if(lifeTimer >= lifespan)
        {
            isDestroyed = true;
            setState(State.DESTROY);
        }
    }

    protected void FixedUpdate()
	{
		switch(state)
		{
            case State.CREATE:
                createState();
                break;

            case State.TRAVEL:
                travelState();
                break;

            case State.DESTROY:
                destroyState();
                break;
        }
	}

    // ================================
    //  State Handler
    // ================================

    protected void createState()
    {

    }

    protected void travelState()
    {
        
    }

    protected void destroyState()
    {
        
    }

    protected void setState(State state, bool changeAnim = true)
	{
        if(isDestroyed && state != State.DEAD) return;

        if(changeAnim) animator.SetTrigger(animationTrigger[(int) state]);
        this.state = state;
	}

    // ================================
    //  Damage
    // ================================    

    public virtual void damage()
    {
        Collider2D[] hitEntities = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, attackLayers);
        foreach(Collider2D hit in hitEntities)
        {
            GameEventSystem.current.GiveDamage(hit.name, baseAttack);
            if(!canHitMultiple) break;
        }
    }

    // ================================
    //  Gizmos
    // ================================

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 0, 0);
        if (attackPoint != null) Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

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

    public string projectileName;
    public string element;

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

    protected Animator animator;
    protected bool isDestroyed = false;

    protected ProjectileData data;

    // ================================
    //  Functions
    // ================================

    protected void Start() {
        data = new ProjectileData(projectileName, element, baseAttack);
        animator = GetComponent<Animator>();
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
        StartCoroutine(Wait(animator.GetCurrentAnimatorStateInfo(0).length, true, State.TRAVEL));
    }

    protected void travelState()
    {
        if(hit()) setState(State.DESTROY);
    }

    protected void destroyState()
    {
        StartCoroutine(Wait(animator.GetCurrentAnimatorStateInfo(0).length));
        Destroy(gameObject);
    }

    protected void setState(State state, bool changeAnim = true)
	{
        if(changeAnim) animator.SetTrigger(animationTrigger[(int) state]);
        this.state = state;
	}

    // ================================
    //  Hit
    // ================================    

    public virtual bool hit()
    {
        Collider2D[] hitEntities = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, attackLayers);
        foreach(Collider2D hit in hitEntities)
        {
            GameEventSystem.current.ProjectileHit(hit.name, data);
            if(!canHitMultiple) return true;
        }

        return false;
    }

    // ================================
    //  Coroutines
    // ================================

    protected IEnumerator Wait(float _delay = 0, bool setStateOnFinish = false, State _state = State.TRAVEL, bool changeAnim = true)
	{
        yield return new WaitForSeconds(_delay);
        if(setStateOnFinish) setState(_state, changeAnim);
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

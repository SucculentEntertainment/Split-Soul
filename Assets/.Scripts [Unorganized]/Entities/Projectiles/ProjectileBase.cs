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
        DESTROY,
        UNINIT
	}

    protected string[] animationTrigger = { "Create", "Travel", "Destroy", "" };
    
    // --------------------------------
    //  Parameters
    // --------------------------------

    [Header("Projectile Attributes")]
    public string projectileID;
    public string element;
    public bool canHitMultiple = false;
    public float lifespan;
    public float baseAttack;
    public float speed;

    [Header("Ranges")]
    public Transform attackPoint;
    public float attackRange;
    public LayerMask attackLayers;

    [Header("Particles")]
    public bool useParticles = false;
    public ParticleSystem particles;

    // --------------------------------
    //  Internal Values
    // --------------------------------

    protected Rigidbody2D rb;
    protected Vector2 dir;
    protected string creator;
    
    protected State state = State.CREATE; 
    protected float lifeTimer = 0;

    protected Animator animator;
    protected bool isDestroyed = false;

    protected ProjectileData data;
    protected bool impulseGiven = false;

    // ================================
    //  Functions
    // ================================

    public void init(Vector2 dir, string creator)
    {
        this.dir = dir;
        this.creator = creator;

        setState(State.CREATE);
    }

    protected void Start() {
        data = new ProjectileData(projectileID, element, baseAttack);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected void Update() {
        lifeTimer += Time.deltaTime;

        if(lifeTimer >= lifespan) destruct();

        animator.SetFloat("DirX", dir.x);
        animator.SetFloat("DirY", dir.y);
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
        if(isDestroyed) return;
        StartCoroutine(Wait(animator.GetCurrentAnimatorStateInfo(0).length, true, State.TRAVEL));
        if(useParticles) particles.Play();
    }

    protected void travelState()
    {
        if(isDestroyed) return;

        if(!impulseGiven)
        {
            impulseGiven = true;
            rb.AddForce(speed * dir, ForceMode2D.Impulse);
        }

        if(hit()) destruct();
    }

    protected void destroyState()
    {
        GetComponent<DimensionEvent>().unregister();

        animator.speed = 0;

        if(useParticles && particles.particleCount > 0) return;
        Destroy(gameObject);
    }

    protected void setState(State state, bool changeAnim = true)
	{
        if(isDestroyed && state != State.DESTROY) return;

        if(changeAnim && animator != null) animator.SetTrigger(animationTrigger[(int) state]);
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
            if(hit.name == creator) continue;

            GameEventSystem.current.ProjectileHit(hit.name, data);
            if(!canHitMultiple) return true;
        }

        return false;
    }

    private void destruct()
    {
        if(isDestroyed) return;
        isDestroyed = true;
        if(useParticles) particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        rb.drag = 5f;

        animator.SetTrigger(animationTrigger[(int) State.DESTROY]);
        StartCoroutine(Wait(animator.GetCurrentAnimatorStateInfo(0).length, true, State.DESTROY, false));
    }

    // ================================
    //  Events
    // ================================

    private void OnDimensionEnable(string dimension)
    {

    }

    private void OnDimensionDisable(string dimension)
    {

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

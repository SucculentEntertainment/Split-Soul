using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : EnemyBase
{
    // ================================
    //  Parameters
    // ================================

    // --------------------------------
    //  Parameters -> Attributes
    // --------------------------------

    [Header("Slime Movement")]
    public float impulse;
    public float drag;

	[Header("Special Attack")]
	public bool isBase = false;
	public float impulseFactor = 1;
	public float attackFactor = 1;

    // --------------------------------
    //  Parameters -> Internal Values
    // --------------------------------

    [HideInInspector] public bool enableMovement = false;
    private bool impulseGiven = false;

    private Rigidbody2D rb;

    // ================================
    //  Functions (overriden)
    // ================================

	public override void additionalStart()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public override void additionalUpdate()
	{
		if(enableMovement) rb.drag = 0;
        else
        {
            rb.drag = drag;
            impulseGiven = false;
        }
	}

    // ================================
    //  States (overriden)
    // ================================

	public override void move()
	{
		moveHelper();
	}

	public override void specialAttack()
	{
		moveHelper(impulseFactor);
	}

    // ================================
    //  Events (overriden)
    // ================================

    public override void OnDimensionDisable(string dimension)
    {
        rb.drag = drag;

        base.OnDimensionDisable(dimension);
    }

    // ================================
    //  Special Attack (overriden)
    // ================================

    public override bool isSpecialAttackEligable(GameObject targetObject)
    {
        Vector2 dist = transform.position - targetPosition;
        
        if(dist >= detectRange * 1.5f && isBase) return true;
        return false;
    }
    
    // ================================
    //  Helper Functions
    // ================================
    
    private bool moveHelper(float impulseFactor = 1)
    {
    		agent.SetDestination(targetPosition);
        agent.isStopped = true;

        if(agent.path.corners.Length < 2) return;
        if(!enableMovement) return;

        Vector2 dir = (agent.path.corners[1] - transform.position).normalized;
        if(!impulseGiven)
        {
            rb.AddForce(dir * impulse * impulseFactor, ForceMode2D.Impulse);
            impulseGiven = true;
        }
    }
    
    public void OnSpecialAttackEnd()
    {
    		specialAttackEnded = true;
    	}
}
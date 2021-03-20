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
    public float rangeTriggerFactor = 1.5f;

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
		if(moveHelper()) return;
	}

	public override void specialAttack()
	{
		if(moveHelper(impulseFactor)) return;
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
        Vector2 dist = (Vector2) transform.position - targetPosition;
        
        if(dist.magnitude >= detectRange * rangeTriggerFactor && isBase) return true;
        return false;
    }
    
    // ================================
    //  Helper Functions
    // ================================
    
    private bool moveHelper(float impulseFactor = 1)
    {
    	agent.SetDestination(targetPosition);
        agent.isStopped = true;

        if(agent.path.corners.Length < 2) return true;
        if(!enableMovement) return true;

        Vector2 dir = (agent.path.corners[1] - transform.position).normalized;
        if(!impulseGiven)
        {
            rb.AddForce(dir * impulse * impulseFactor, ForceMode2D.Impulse);
            impulseGiven = true;
        }

        return false;
    }
    
    public void OnSpecialAttackEnd()
    {
    	specialAttackEnded = true;
    }
}
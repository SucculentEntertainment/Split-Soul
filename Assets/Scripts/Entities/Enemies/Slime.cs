using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : EnemyBase
{
    public float impulse;
    public float drag;

    public bool enableMovement = false;
    private bool impulseGiven = false;

    private Rigidbody2D rb;

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

	public override void move()
	{
        agent.isStopped = true;
        base.move();

        if(agent.path.corners.Length == 0) return;
        if(!enableMovement) return;

        Vector2 dir = (agent.path.corners[1] - transform.position).normalized;
        if(!impulseGiven)
        {
            rb.AddForce(dir * impulse, ForceMode2D.Impulse);
            impulseGiven = true;
        }
	}
}
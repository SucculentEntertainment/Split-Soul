using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // --------------------------------
    //  States
    // --------------------------------

    private enum State
	{
        IDLE,
        MOVE_WANDER,
        MOVE_TARGET,
        ATTACK,
        DEAD
	}

    // --------------------------------
    //  Parameters
    // --------------------------------

    public float maxHealth;

    // --------------------------------
    //  Internal Values
    // --------------------------------

    private float health;
    private State state = State.IDLE;

    // ================================
    //  Functions
    // ================================

    private void Start()
    {
        health = maxHealth;
    }

	private void Update()
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

    // ================================
    //  State Handler
    // ================================

    private void idleState()
	{
        idle();


	}

    private void moveState()
    {
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

    // ================================
    //  States
    // ================================

    private void idle()
    {
        
    }

    private void move()
    {
        
    }

    private void attack()
    {
        
    }

    private void dead()
    {
        
    }

    // ================================
    //  Damage
    // ================================

    void die()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders) collider.enabled = false;
        this.enabled = false;
    }

    // ================================
    //  Functions
    // ================================

    public void OnReceiveDamage(float damage)
	{
        health -= damage;
        if (health <= 0) die();
	}
}

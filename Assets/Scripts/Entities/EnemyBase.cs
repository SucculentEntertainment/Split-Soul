using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // --------------------------------
    //  Parameters
    // --------------------------------

    public float maxHealth;

    // --------------------------------
    //  Internal Values
    // --------------------------------

    private float health;

    // ================================
    //  Functions
    // ================================

    void Start()
    {
        health = maxHealth;
    }

    public void OnReceiveDamage(float damage)
	{
        health -= damage;
        if (health <= 0) die();
	}

    void die()
	{
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders) collider.enabled = false;
        this.enabled = false;
	}
}

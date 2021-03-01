using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    // --------------------------------
    //  Parameters
    // --------------------------------

    public float lifespan;
    public float baseAttack;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask attackLayers;

    // --------------------------------
    //  Internal Values
    // --------------------------------

    // ================================
    //  Functions
    // ================================

    // TODO: Implement Projectiles

    // ================================
    //  Damage
    // ================================    

    public virtual void damage()
    {
        Collider2D[] hitEntities = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, attackLayers);
        foreach(Collider2D hit in hitEntities)
        {
            GameEventSystem.current.GiveDamage(hit.name, baseAttack);
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

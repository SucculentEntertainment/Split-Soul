using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DefaultMoveBehaviour : BaseBehaviour
{
    private Rigidbody2D rb;
    public float speed = 1f;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void exec(Vector2 moveDir, Vector2 aimDir, string returnState = "", float delta = 0)
    {
        base.exec(moveDir, aimDir, returnState, delta);
        rb.AddForce(moveDir * speed, ForceMode2D.Impulse);
    }
}

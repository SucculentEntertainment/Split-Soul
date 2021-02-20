﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 1f;

    private Vector2 dir;
    private Rigidbody2D rb;

    private GameObject sprite;
    private Animator animator;

    private bool disableMovement = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = transform.GetChild(0).gameObject;
        animator = sprite.GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetFloat("Mag", dir.magnitude);

        if (dir.magnitude != 0f)
        {
            animator.SetFloat("DirX", dir.x);
            animator.SetFloat("DirY", dir.y);
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(dir * speed, ForceMode2D.Impulse);
    }

    // ================================
    //  Input
    // ================================

    private void OnMove(InputValue dirVal)
    {
        if (disableMovement) return;

        dir = dirVal.Get<Vector2>();
        Debug.Log(dir);
    }

    private void OnConsole(InputValue val)
    {
        disableMovement = !disableMovement;
        dir = new Vector2(0, 0);
    }

    private void OnEscape(InputValue val)
    {
        disableMovement = false;
    }
}

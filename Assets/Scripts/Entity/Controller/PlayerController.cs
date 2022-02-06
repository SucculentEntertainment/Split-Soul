using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private Vector2 movDir = new Vector2(0, 0);
    private Vector2 aimDir = new Vector2(0, 0);

    [Header("References")]
    public Camera mainCamera;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        // Only change to idle state when no longer moving
        if(parent.state == "move" && rb.velocity == Vector2.zero) parent.setData(movDir, aimDir, "idle");
    }

    private void OnMove(InputValue dirVal)
    {
        movDir = dirVal.Get<Vector2>();
        parent.setData(movDir, aimDir, "move");
    }

    private void OnAim(InputValue posVal)
    {
        Vector2 worldPos = mainCamera.ScreenToWorldPoint(posVal.Get<Vector2>());
        aimDir = worldPos - (Vector2)transform.position;
        aimDir.Normalize();
    }
}

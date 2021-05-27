using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Transform pickupPoint;
    public float pickupRange = 0.25f;
    public LayerMask playerLayer;

    public string id;
    public int amount;

    private bool enablePickup = true;

    // ================================
    //  Functions
    // ================================

    void Update()
    {
        if (!enablePickup) return;

        Collider2D player = Physics2D.OverlapCircle(pickupPoint.position, pickupRange, playerLayer);
        if (player == null) return;

        GameEventSystem.current.Pickup(player.name, new Collectable(id, amount));
        pickedUp();
    }

    private void pickedUp()
	{
        GetComponent<DimensionEvent>().unregister();
        Destroy(gameObject);
        this.enabled = false;
	}

    // ================================
    //  Events
    // ================================

    private void OnDimensionEnable(string dimension)
	{
        enablePickup = true;
        foreach(Transform c in transform) { c.gameObject.SetActive(true); }
	}

    private void OnDimensionDisable(string dimension)
    {
        enablePickup = false;
        foreach (Transform c in transform) { c.gameObject.SetActive(false); }
    }

    // ================================
    //  Gizmos
    // ================================

    void OnDrawGizmosSelected()
    {
        if (pickupPoint == null) return;
        Gizmos.DrawWireSphere(pickupPoint.position, pickupRange);
    }
}

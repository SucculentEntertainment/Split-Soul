using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SplitSoul.Core;
using SplitSoul.Core.Events;
using SplitSoul.Data;
using SplitSoul.Data.Scriptable.Inventory;

namespace SplitSoul.Entity.Legacy
{
	public class Pickup : MonoBehaviour
	{
		public Transform pickupPoint;
		public float pickupRange = 0.25f;
		public LayerMask playerLayer;

		public Animator animator;
		public AnimatorOverrideController missingIcon;

		public string id;
		public int amount;
		public Item item = null;

		private bool enablePickup = true;

		// ================================
		//  Functions
		// ================================

		private void Start() { if (item != null) setItem(item); }

		void Update()
		{
			if (!enablePickup) return;

			Collider2D player = Physics2D.OverlapCircle(pickupPoint.position, pickupRange, playerLayer);
			if (player == null) return;

			GameEventSystem.current.Pickup(player.name, new Collectable(id, amount, item));
			pickedUp();
		}

		private void pickedUp()
		{
			GetComponent<DimensionEvent>().unregister();
			Destroy(gameObject);
			this.enabled = false;
		}

		public void setItem(Item item)
		{
			if (item == null || id != "item")
			{
				Destroy(gameObject);
				return;
			}

			if (item.icon != null) animator.runtimeAnimatorController = item.icon;
			else animator.runtimeAnimatorController = missingIcon;

			this.item = item;
		}

		// ================================
		//  Events
		// ================================

		private void OnDimensionEnable(string dimension)
		{
			enablePickup = true;
			foreach (Transform c in transform) { c.gameObject.SetActive(true); }
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
}

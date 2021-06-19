using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using SplitSoul.Core;
using SplitSoul.Core.Events;
using SplitSoul.Data.Entity;

namespace SplitSoul.Entity.InputProvider
{
	public class PlayerInputProvider : BaseInputProvider
	{
		//TODO: Move interact to seperate behaviour
		public Transform interactPoint;
		public float interactRange = 0.5f;
		public LayerMask interactLayers;

		private GameManager gm;
		private Vector2 dir = Vector2.zero;

		private Collider2D interactable = null;

		private void Awake()
		{
			gm = GameManager.current;
		}

		public override void setDisableDirectional(bool state)
		{
			if (state) dir = Vector2.zero;
			disableDirectional = state;
		}

		private void throwUIAction(string action) { GameEventSystem.current.ThrowUIAction(new UIAction(action)); }
		private void Update()
		{
			//Scan for interactables
			Collider2D newInteractable = Physics2D.OverlapCircle(interactPoint.position, interactRange, interactLayers);
			if (newInteractable != interactable)
			{
				if (interactable != null) GameEventSystem.current.InteractHighlight(interactable.name, false);
				interactable = newInteractable;
				if (interactable != null) GameEventSystem.current.InteractHighlight(interactable.name, true);
			}
		}

		// ================================
		//  Input
		// ================================

		private void OnMove(InputValue dirVal)
		{
			if (disableDirectional) return;
			dir = dirVal.Get<Vector2>();

			inputData.setData(dir.magnitude > 0 ? "move" : "idle", dir);
		}

		//TODO: Rework Attack reset (Callback from core)
		private void OnAttack(InputValue val) { /*if (!gm.playerDisableMovement)*/ inputData.setData("attack", dir); }
		private void OnConsole(InputValue val) { throwUIAction("Console"); }
		private void OnEscape(InputValue val) { throwUIAction("ESC"); }
		private void OnReturn(InputValue val) { throwUIAction("Enter"); }
		private void OnInteract(InputValue val) { if (interactable != null && !gm.playerDisableMovement) GameEventSystem.current.Interact(interactable.name); }
	}
}

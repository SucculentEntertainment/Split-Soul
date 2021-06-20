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
		public Camera mainCamera;

		//TODO: Move interact to seperate behaviour
		public Transform interactPoint;
		public float interactRange = 0.5f;
		public LayerMask interactLayers;

		private GameManager gm;
		private Vector2 movDir = Vector2.zero;
		private Vector2 mouseDir = new Vector2(0, -1);

		private Collider2D interactable = null;

		private void Awake()
		{
			gm = GameManager.current;
		}

		public override void setDisableDirectional(bool state)
		{
			if (state) movDir = Vector2.zero;
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
			movDir = dirVal.Get<Vector2>();

			if(movDir.magnitude > 0) inputData.setData("move", movDir);
			else inputData.setData("idle", mouseDir);
		}

		private void OnAttack(InputValue val) { /*if (!gm.playerDisableMovement)*/ inputData.setData(val.isPressed ? "attack" : "idle", mouseDir); }
		private void OnConsole(InputValue val) { throwUIAction("Console"); }
		private void OnEscape(InputValue val) { throwUIAction("ESC"); }
		private void OnReturn(InputValue val) { throwUIAction("Enter"); }
		private void OnInteract(InputValue val) { if (interactable != null && !gm.playerDisableMovement) GameEventSystem.current.Interact(interactable.name); }
		private void OnAim(InputValue posVal)
		{
			Debug.Log("Updating Mouse position");
			Vector2 worldPos = mainCamera.ScreenToWorldPoint(posVal.Get<Vector2>());
			mouseDir = worldPos - gm.playerPosition;
			mouseDir.Normalize();
			Debug.Log("Mouse Dir: " + mouseDir);
		}
	}
}

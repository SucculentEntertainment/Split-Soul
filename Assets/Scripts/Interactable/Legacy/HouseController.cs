using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using SplitSoul.Core;

namespace SplitSoul.Interactable.Legacy
{
	public class HouseController : MonoBehaviour
	{
		public SceneIndecies targetScene;
		public Color highlightColor;

		private bool enableInteractions = true;
		private SpriteRenderer[] sprites;
		private Color normalColor;

		private void Start()
		{
			sprites = transform.Find("Sprites").GetComponentsInChildren<SpriteRenderer>();
			normalColor = Color.white;

			foreach (UnityEngine.Rendering.Universal.Light2D light in transform.Find("Lighting").GetComponentsInChildren<UnityEngine.Rendering.Universal.Light2D>())
			{
				GameEventSystem.current.RegisterLight(light);
			}
		}

		// ================================
		//  Events
		// ================================

		private void OnDimensionEnable(string dimension) { enableInteractions = true; }
		private void OnDimensionDisable(string dimension) { enableInteractions = false; }

		private void OnInteract()
		{
			if (!enableInteractions) { return; }
			GameEventSystem.current.LevelChange((int)targetScene);
		}

		private void OnInteractHighlight(bool activate)
		{
			if (!enableInteractions || !activate)
			{
				foreach (SpriteRenderer sr in sprites) sr.color = normalColor;
				return;
			}

			foreach (SpriteRenderer sr in sprites) sr.color = highlightColor;
		}
	}
}

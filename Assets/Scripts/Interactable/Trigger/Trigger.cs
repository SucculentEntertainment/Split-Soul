using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitSoul.Interactable.Trigger
{
	public class Trigger : MonoBehaviour
	{
		private bool activated = false;

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (activated) return;
			if (other.gameObject.name != "Player") return;
			if (!conditions()) return;

			activated = true;
			actions();
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.gameObject.name != "Player") return;
			activated = false;
		}

		protected virtual bool conditions() { return true; }
		protected virtual void actions() { }
	}
}

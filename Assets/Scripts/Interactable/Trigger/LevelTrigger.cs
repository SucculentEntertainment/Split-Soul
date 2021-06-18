using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SplitSoul.Core;
using SplitSoul.Core.Level;

namespace SplitSoul.Interactable.Trigger
{
	public class LevelTrigger : Trigger
	{
		public bool returnToPrevious = true;
		public SceneIndecies destination;

		private int target = -1;

		private void Start()
		{
			StartCoroutine(setTarget());
		}

		protected override void actions() { GameEventSystem.current.LevelChange(target); }
		protected override bool conditions() { return target != -1; }

		IEnumerator setTarget()
		{
			while (target == -1)
			{
				if (returnToPrevious) target = LevelManager.current.previousLevel;
				else target = (int)destination;

				yield return null;
			}

			yield return null;
		}
	}
}

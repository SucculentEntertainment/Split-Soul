using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

using SplitSoul.Data.Scriptable.Collection;

namespace SplitSoul.Entity.Behaviour
{
	public abstract class BaseBehaviour : MonoBehaviour
	{
		public string type;
		public AnimatorController animation;
		public AnimationOverrideCollection dimensionOverrides;
		public abstract void exec();

		public bool finished = false;
	}
}

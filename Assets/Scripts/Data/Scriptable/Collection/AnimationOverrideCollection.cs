using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitSoul.Data.Scriptable.Collection
{
	[System.Serializable]
	public class AnimationOverrideProfile
	{
		public AnimatorOverrideController animationOverride;
		public Gradient gradient;
	}

	[CreateAssetMenu(menuName = "Animation Override Collection")]
	public class AnimationOverrideCollection : ScriptableObject
	{
		public List<AnimationOverrideProfile> profiles;
	}
}

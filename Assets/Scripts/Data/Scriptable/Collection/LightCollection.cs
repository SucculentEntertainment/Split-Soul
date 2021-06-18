using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitSoul.Data.Scriptable.Collection
{
	[System.Serializable]
	public class LightProfile
	{
		public string dimension;
		public Color color;
	}

	[CreateAssetMenu(menuName = "Light Collection")]
	public class LightCollection : ScriptableObject
	{
		public List<LightProfile> profiles;
	}
}

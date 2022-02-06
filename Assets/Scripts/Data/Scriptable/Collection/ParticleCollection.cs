using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitSoul.Data.Scriptable.Collection
{
	[System.Serializable]
	public class ParticleProfile
	{
		public string dimension;
		public Gradient gradient;
	}

	[CreateAssetMenu(menuName = "Particle Collection")]
	public class ParticleCollection : ScriptableObject
	{
		public List<ParticleProfile> profiles;
	}
}
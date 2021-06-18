using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SplitSoul.Core;
using SplitSoul.Entity.Legacy.Projectiles;

namespace SplitSoul.Entity.Spawner
{
	[System.Serializable]
	public class ProjectileProfile
	{
		public string dimension;
		public GameObject projectile;
	}

	public class ProjectileSpawner : MonoBehaviour
	{
		public bool useDifferentProfiles = false;
		public List<ProjectileProfile> profiles;
		public Transform projectileContainer;

		public void spawnProjectile(Vector2 aimPosition)
		{
			ProjectileProfile profile = profiles[0];
			if (useDifferentProfiles) profile = profiles.Find(x => x.dimension == GameManager.current.dimension);

			GameObject instance = Instantiate(profile.projectile, transform.position, Quaternion.identity, projectileContainer);
			Vector2 dir = (aimPosition - (Vector2)transform.position).normalized;
			instance.GetComponent<ProjectileBase>().init(dir, this.name);
		}
	}
}

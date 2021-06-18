using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SplitSoul.Data.Entity;

namespace SplitSoul.Core.Events
{
	public class ProjectileHitEvent : MonoBehaviour
	{
		public List<string> ignored;

		private void Start()
		{
			GameEventSystem.current.onProjectileHit += onProjectileHit;
		}

		private void onProjectileHit(string id, ProjectileData data)
		{
			if (ignored.Contains(data.name)) return;
			if (id == gameObject.name) gameObject.SendMessage("OnProjectileHit", data, SendMessageOptions.DontRequireReceiver);
		}

		public void unregister()
		{
			GameEventSystem.current.onProjectileHit -= onProjectileHit;
		}
	}
}

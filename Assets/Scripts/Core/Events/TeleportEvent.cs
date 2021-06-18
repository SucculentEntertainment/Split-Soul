using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitSoul.Core.Events
{
	public class TeleportEvent : MonoBehaviour
	{
		private void Start()
		{
			GameEventSystem.current.onTeleport += onTeleport;
		}

		private void onTeleport(string id, Vector2 pos)
		{
			if (id == gameObject.name)
			{
				transform.position = pos;
				gameObject.SendMessage("OnTeleport", pos, SendMessageOptions.DontRequireReceiver);
			}
		}

		public void unregister()
		{
			GameEventSystem.current.onTeleport -= onTeleport;
		}
	}
}

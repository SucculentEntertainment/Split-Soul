using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SplitSoul.Core.Events
{
	public class LightRegisterEvent : MonoBehaviour
	{
		private void Start()
		{
			GameEventSystem.current.onLightRegister += onLightRegister;
			GameEventSystem.current.onLightUnregister += onLightUnregister;
		}

		private void onLightRegister(UnityEngine.Rendering.Universal.Light2D light)
		{
			gameObject.SendMessage("OnLightRegister", light, SendMessageOptions.DontRequireReceiver);
		}

		private void onLightUnregister(UnityEngine.Rendering.Universal.Light2D light)
		{
			gameObject.SendMessage("OnLightUnregister", light, SendMessageOptions.DontRequireReceiver);
		}

		public void unregister()
		{
			GameEventSystem.current.onLightRegister += onLightRegister;
			GameEventSystem.current.onLightUnregister -= onLightUnregister;
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitSoul.Core.Events
{
	public class DimensionEvent : MonoBehaviour
	{
		public List<string> dimensions;
		public List<GameObject> autoToggleObj;

		private void Start()
		{
			GameEventSystem.current.onDimensionChange += onDimensionChange;
		}

		private void onDimensionChange(string dimension)
		{
			if (dimensions.Contains(dimension))
			{
				gameObject.SendMessage("OnDimensionEnable", dimension, SendMessageOptions.DontRequireReceiver);
				foreach (GameObject obj in autoToggleObj) obj.SetActive(true);
			}
			else
			{
				gameObject.SendMessage("OnDimensionDisable", dimension, SendMessageOptions.DontRequireReceiver);
				foreach (GameObject obj in autoToggleObj) obj.SetActive(false);
			}
		}

		public void unregister()
		{
			GameEventSystem.current.onDimensionChange -= onDimensionChange;
		}

		private void OnEnable()
		{
			onDimensionChange(GameManager.current.dimension);
		}
	}
}

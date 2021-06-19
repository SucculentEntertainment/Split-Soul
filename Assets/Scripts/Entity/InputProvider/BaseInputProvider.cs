using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SplitSoul.Data.Entity;

namespace SplitSoul.Entity.InputProvider
{
	public abstract class BaseInputProvider : MonoBehaviour
	{
		protected InputData inputData = new InputData("idle", Vector2.zero);
		public bool disableDirectional = false;
		public abstract void setDisableDirectional(bool state);
		public virtual InputData getInputData()
		{
			return inputData;
		}
	}
}

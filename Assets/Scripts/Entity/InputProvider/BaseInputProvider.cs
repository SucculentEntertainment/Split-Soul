using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SplitSoul.Data.Entity;

namespace SplitSoul.Entity.InputProvider
{
	public abstract class BaseInputProvider : MonoBehaviour
	{
		public bool disableDirectional = false;
			
		protected InputData inputData = new InputData("idle", Vector2.zero, Vector2.zero);
		protected Vector2 moveDir = Vector2.zero;
		protected Vector2 aimDir = Vector2.zero;
		
		public abstract void setDisableDirectional(bool state);
		public virtual InputData getInputData()
		{
			return inputData;
		}
	}
}

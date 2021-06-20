using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SplitSoul.Data.Entity;
using SplitSoul.Entity.Behaviour;
using SplitSoul.Entity.InputProvider;

namespace SplitSoul.Entity
{
	public class EntityCore : MonoBehaviour
	{
		public BaseInputProvider inputProvider;
		public List<BaseBehaviour> behaviours;

		private InputData inputData;
		private InputData tmp;

		private void Update()
		{
			tmp = inputData;
			inputData = inputProvider.getInputData();
			// if (inputData != null && tmp != inputData)
			// Debug.Log("InputData: \"" + inputData.behaviourType + "\"" + inputData.dir);
		}
	}
}

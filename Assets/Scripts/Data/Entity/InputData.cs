using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitSoul.Data.Entity
{
	public class InputData
	{
		private string _behaviourType;
		private Vector2 _dir;

		public string behaviourType { get { return _behaviourType; } set { _behaviourType = value; } }
		public Vector2 dir { get { return _dir; } set { _dir = value; } }

		public InputData(string behaviourType, Vector2 dir) { setData(behaviourType, dir); }
		public InputData(InputData inputData) { setData(inputData.behaviourType, inputData.dir); }
		public void setData(string behaviourType, Vector2 dir)
		{
			_behaviourType = behaviourType;
			_dir = dir;
		}

		public void resetData()
		{
			behaviourType = "idle";
			dir = Vector2.zero;
		}
	};
}

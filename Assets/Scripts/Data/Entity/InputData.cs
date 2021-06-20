using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitSoul.Data.Entity
{
	public class InputData
	{
		private string _behaviourType;
		private Vector2 _moveDir;
		private Vector2 _aimDir;

		public string behaviourType { get { return _behaviourType; } set { _behaviourType = value; } }
		public Vector2 moveDir { get { return _moveDir; } set { _moveDir = value; } }
		public Vector2 aimDir { get { return _aimDir; } set { _aimDir = value; } }

		public InputData(string behaviourType, Vector2 moveDir, Vector2 aimDir) { setData(behaviourType, moveDir, aimDir); }
		public InputData(InputData inputData) { setData(inputData.behaviourType, inputData.moveDir, inputData.aimDir); }
		public void setData(string behaviourType, Vector2 moveDir, Vector2 aimDir)
		{
			_behaviourType = behaviourType;
			_moveDir = Dir;
			_aimDir = aimDir;
		}

		public void resetData()
		{
			behaviourType = "idle";
			moveDir = Vector2.zero;
			aimDir = Vector2.zero;
		}
	};
}

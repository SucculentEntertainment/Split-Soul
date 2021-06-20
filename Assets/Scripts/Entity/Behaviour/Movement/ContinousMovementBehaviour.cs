using System.Collection;
using System.Collection.Generic;
using UnityEngine;

using SplitSoul.Entity.Behaviour;

namespace SplitSoul.Entity.Behaviour.Movement
{
	public class ContinousMovementBehaviour : BaseBehaviour
	{
		public RigidBody2D rb;
		public float speed;
	
		public override void exec(Vector2 moveDir, Vector2 aimDir)
		{
			finished = false;
			
			rb.AddForce(moveDir * speed, ForceMode2D.Impulse);
			
			finished = true;
		}
	}
}
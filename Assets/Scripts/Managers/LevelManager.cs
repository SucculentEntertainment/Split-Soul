using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private bool firstUpdate = true;

    private static string _dimension;
    public static string dimension {
        set { GameEventSystem.current.DimensionChange(value); _dimension = value; }
        get { return _dimension; }
    }

	// ================================
	//  Functions
	// ================================

    private void Start()
    {
        _dimension = "alive";
    }

	private void Update()
	{
		if(firstUpdate)
		{
            firstUpdate = false;
            dimension = "alive";
		}
	}
	
	// ================================
	//  Actions
	// ================================
	
	public void spawnProjectile(string name, Vector2 dir, string creator)
	{
		GameObject projectile = Instantiate();
	}
}

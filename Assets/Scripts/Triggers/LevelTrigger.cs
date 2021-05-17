using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
	public bool returnToPrevious = true;
	public SceneIndecies destination;

	private int target = -1;
	private bool activated = false;

	private void Start()
	{
		StartCoroutine(setTarget());
	}

    private void OnTriggerEnter2D(Collider2D other)
	{
		if(activated) return;
		if(other.gameObject.name != "Player") return;
		if(target == -1) return;

		activated = true;
		GameEventSystem.current.LevelChange(target);
	}

	IEnumerator setTarget()
	{
		while(target == -1)
		{
			if(returnToPrevious) target = LevelManager.current.previousLevel;
			else target = (int) destination;

			yield return null;
		}

		yield return null;
	}
}

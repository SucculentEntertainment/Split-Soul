using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
	public Transform spawnCenter;
	public float spawnRadius;
	public LayerMask cannotOverlap;
	public float overlapRange;

	public GameObject spawnPrefab;
	public Transform container;

	private GameManager gm;

	// ================================
	//  Functions
	// ================================

	private void Start() { gm = GameManager.current; }

	public void spawnItem(string id, int amount)
	{
		//TODO: Move this into seperate utility function
		Item item = gm.existingItems.Find(x => x.id == id);

		GameObject obj = Instantiate(spawnPrefab, container);
		obj.GetComponent<Pickup>().setItem(item);
		obj.GetComponent<Pickup>().amount = amount;

		while(true)
		{
			obj.transform.position = rndPointInCircle(spawnRadius, spawnCenter.position);
			if(Physics2D.OverlapCircle(obj.transform.position, overlapRange, cannotOverlap) == null) break;
		}
	}

	public Vector2 rndPointInCircle(float r, Vector2 c)
	{
		int a = Random.Range(0, 359);
		Vector2 point = new Vector2(0, 0);

		point.x = c.x + r * Mathf.Cos(a);
		point.y = c.y + r * Mathf.Sin(a);

		return point;
	}

	// ================================
    //  Gizmos
    // ================================

    private void OnDrawGizmosSelected()
	{
        Gizmos.color = new Color(0, 255, 255);
        if (spawnCenter != null) Gizmos.DrawWireSphere(spawnCenter.position, spawnRadius);
    }
}

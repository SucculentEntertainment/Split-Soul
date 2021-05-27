using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Item")]
public class Item : ScriptableObject
{
	public string id;
    public string name;
    public string description;
    public AnimatorOverrideController icon;
    public AnimatorOverrideController highres;
    public bool stackable;
}

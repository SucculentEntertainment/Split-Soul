﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Item")]
public class Item : ScriptableObject
{
	public string id;
    public string itemName;
    public string description;
	public string category;
    public AnimatorOverrideController icon;
    public AnimatorOverrideController highres;
    public bool stackable;
}

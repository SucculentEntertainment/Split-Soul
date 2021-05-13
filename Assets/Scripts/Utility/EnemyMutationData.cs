using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializableAttribute]
public class EnemyMutationData
{
    [Tooltip("If true, transition will occur when hit with elemental damage, else if projectile hits")]
    public bool useElement;
    [Tooltip("Specifies which projectile triggers the transition")]
    public string projectileID;
    [Tooltip("Specifies which element triggers the transition")]
    public string element;
    [Tooltip("Specifies which enemy to transition to")]
    public GameObject target;
}
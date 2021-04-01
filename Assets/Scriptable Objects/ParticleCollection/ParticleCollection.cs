using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParticleProfile
{
    public string dimension;
    public ParticleSystem particleSystem;
}

[CreateAssetMenu(menuName="Particle Collection")]
public class ParticleCollection : ScriptableObject
{
    public List<ParticleProfile> profiles;
}
